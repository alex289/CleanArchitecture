using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;

namespace CleanArchitecture.ServiceDefaults;

public static class HttpEnricher
{
    public static Action<System.Diagnostics.Activity, HttpRequestMessage>? RequestEnricher = static (activity, request) =>
    {
        if (request.Content is not null)
        {
            try
            {
                var requestContent = request.Content.ReadAsStringAsync().Result;
                activity.SetTag("http.request.body", requestContent);
            }
            catch (HttpRequestException)
            {

            }
        }
        else
        {
            activity.SetTag("http.response.body", null);
        }
    };

    public static Action<System.Diagnostics.Activity, HttpResponseMessage>? ResponseEnricher = static (activity, response) =>
    {
        if (response.Content is not null)
        {
            try
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                activity.SetTag("http.response.body", responseContent);
            }
            catch (HttpRequestException)
            {

            }
            catch (ObjectDisposedException)
            {

            }
        }
        else
        {
            activity.SetTag("http.response.body", null);
        }
    };

    public static Action<System.Diagnostics.Activity, HttpResponse>? HttpRouteEnricher = static (activity, request) =>
    {
        var endpoint = request.HttpContext.GetEndpoint();

        if (endpoint is RouteEndpoint routeEndpoint)
        {
            var descriptor = routeEndpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

            if (descriptor is null)
            {
                return;
            }

            var controller = descriptor.ControllerName;
            var action = descriptor.ActionName;

            var pathParameters = descriptor.Parameters
                .Where(p => p.BindingInfo is null || p.BindingInfo.BindingSource?.Id == "Path")
                .Select(p => $"{{{p.Name}}}");

            var route = string.Join("/", [controller, action, .. pathParameters]);

            activity.DisplayName = route;
            activity.SetTag("http.route", route);
            activity.SetTag("Name", route);
        }
    };
}
