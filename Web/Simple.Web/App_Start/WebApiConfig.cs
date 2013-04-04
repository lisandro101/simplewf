using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Simple.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Remove xml support
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            // Customize the json formatter:
            // - Do not serialize properties with null values.
            // - Convert enum values to their corresponding string representations.
            // - Use camel case.
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            json.SerializerSettings.Converters.Add(new StringEnumConverter());
            json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Resources
            config.Routes.MapHttpRoute("Flow", "domains/{domain}/workflows/{workflowId}/runs/{runId}/flow", new { controller = "Flow", lastEventId = 1 });
            config.Routes.MapHttpRoute("Workflow", "domains/{domain}/workflows/{workflowId}/runs/{runId}", new { controller = "Workflow" });
            config.Routes.MapHttpRoute("Workflows", "domains/{domain}/workflows", new { controller = "Workflows" });
            config.Routes.MapHttpRoute("Domains", "domains", new { controller = "Domains" });
        }
    }
}