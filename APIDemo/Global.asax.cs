using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Net;

namespace APIDemo
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // 1. Force TLS 1.2 configuration for external API calls
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // 2. Configure Web API Routing directly inline
            GlobalConfiguration.Configure(config =>
            {
                // Enables [Route] attributes on controllers (called exactly once)
                config.MapHttpAttributeRoutes();

                // Sets up the fallback standard routing template
                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );
            });

           
        }
    }
}
