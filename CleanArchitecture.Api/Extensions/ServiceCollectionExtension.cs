using System.Text;
using CleanArchitecture.Api.Swagger;
using CleanArchitecture.Domain.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

namespace CleanArchitecture.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "CleanArchitecture",
                Version = "v1",
                Description = "A clean architecture API"
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. " +
                              "Use the /api/v1/user/login endpoint to generate a token",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });

            c.ParameterFilter<SortableFieldsAttributeFilter>();

            c.SupportNonNullableReferenceTypes();
            
            c.AddSecurityRequirement((document) => new OpenApiSecurityRequirement()
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = ["readAccess", "writeAccess"]
            });
        });
        return services;
    }

    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddAuthentication(
                options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
            .AddJwtBearer(
                jwtOptions =>
                {
                    jwtOptions.TokenValidationParameters = CreateTokenValidationParameters(configuration);
                });

        services
            .AddOptions<TokenSettings>()
            .Bind(configuration.GetSection("Auth"))
            .ValidateOnStart();

        return services;
    }

    public static TokenValidationParameters CreateTokenValidationParameters(IConfiguration configuration)
    {
        var result = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Auth:Issuer"],
            ValidAudience = configuration["Auth:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    configuration["Auth:Secret"]!)),
            RequireSignedTokens = false
        };

        return result;
    }
}