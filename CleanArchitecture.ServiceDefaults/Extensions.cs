using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ServiceDiscovery;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace CleanArchitecture.ServiceDefaults;

public static class Extensions
{
    private const string AspireEnabled = "ASPIRE_ENABLED";

    public static void AddServiceDefaults<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        if (builder.Configuration[AspireEnabled] != "true")
        {
            return;
        }

        builder.ConfigureOpenTelemetry();

        builder.AddDefaultHealthChecks();

        builder.Services.AddServiceDiscovery();

        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            http.AddStandardResilienceHandler();
            http.AddServiceDiscovery();
        });

        builder.Services.Configure<ServiceDiscoveryOptions>(options => { options.AllowedSchemes = ["https"]; });
    }

    private static void ConfigureOpenTelemetry<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        var enableHttpTraces = builder.Configuration.GetValue<bool?>("APP_ENABLE_HTTP_TRACES") ?? false;

        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation();
            })
            .WithTracing(tracing =>
            {
                tracing
                    .AddSource(builder.Environment.ApplicationName)
                    .AddSource("MassTransit")
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.EnableAspNetCoreSignalRSupport = true;
                        options.EnrichWithHttpResponse = HttpEnricher.HttpRouteEnricher;
                    })
                    .AddGrpcClientInstrumentation(options =>
                    {
                        if (enableHttpTraces)
                        {
                            options.EnrichWithHttpRequestMessage = HttpEnricher.RequestEnricher;
                            options.EnrichWithHttpResponseMessage = HttpEnricher.ResponseEnricher;
                        }
                    })
                    .AddEntityFrameworkCoreInstrumentation(options =>
                    {
                        options.EnrichWithIDbCommand = (activity, dbCommand) =>
                        {
                            activity.SetTag("sql.statement", dbCommand.CommandText);
                        };
                    })
                    .AddHttpClientInstrumentation(options =>
                    {
                        if (enableHttpTraces)
                        {
                            options.EnrichWithHttpRequestMessage = HttpEnricher.RequestEnricher;
                            options.EnrichWithHttpResponseMessage = HttpEnricher.ResponseEnricher;
                        }
                    });
            });

        builder.AddOpenTelemetryExporters();
    }

    private static void AddOpenTelemetryExporters<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (useOtlpExporter)
        {
            builder.Services.AddOpenTelemetry().UseOtlpExporter();
        }
    }

    private static void AddDefaultHealthChecks<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);
    }

    public static void MapDefaultEndpoints(this WebApplication app)
    {
        if (app.Configuration[AspireEnabled] != "true")
        {
            return;
        }

        if (app.Environment.IsDevelopment())
        {
            app.MapHealthChecks("/health");

            app.MapHealthChecks("/alive", new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live")
            });
        }
    }
}