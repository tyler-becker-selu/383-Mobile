using GamesTore.AuthenticationStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Dispatcher;

namespace GamesTore
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Web API configuration and services
            //Configure Web API to use only bearer token authentication.
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType))

            //Creating custom pipeline to add GamesToreAuth message handler to pipeline
            DelegatingHandler[] AuthHandler = new DelegatingHandler[] { new GamesToreAuth() };
            var authRouteHandler = HttpClientFactory.CreatePipeline(new HttpControllerDispatcher(config), AuthHandler);

            // Enable Cross-Origin Requests
            // doc: http://www.asp.net/web-api/overview/security/enabling-cross-origin-requests-in-web-api
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Web API routess
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "LoginRoute",
                routeTemplate: "api/ApiKey",
                defaults: new { controller = "ApiKey" }
            );

            config.Routes.MapHttpRoute(
                name: "GamesToreApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: null,
                handler: authRouteHandler
            );
            //config.Routes.MapHttpRoute(
            //    name: "EmployeeRoutes",
            //    routeTemplate: "api/Sales/Employee/{id}",
            //    defaults: new { controller = "Sales", action = "Employee" },
            //    constraints: null,
            //    handler: authRouteHandler
            //);
            //config.Routes.MapHttpRoute(
            //    name: "CustomRoutes",
            //    routeTemplate: "api/Sales/User/{id}",
            //    defaults: new { controller = "Sales", action = "User" },
            //    constraints: null,
            //    handler: authRouteHandler
            //);
            //config.Routes.MapHttpRoute(
            //    name: "RemoveRoute",
            //    routeTemplate: "api/Carts/Remove/{id}",
            //    defaults: new { controller = "Carts", action = "Remove" },
            //    constraints: null,
            //    handler: authRouteHandler
            //);
            config.Routes.MapHttpRoute(
                name: "UserCart",
                routeTemplate: "api/Cart/User/{id}",
                defaults: new { controller = "Carts", action = "User" },
                constraints: null,
                handler: authRouteHandler
            );
            config.Routes.MapHttpRoute(
                name: "UserSingleSale",
                routeTemplate: "api/Sales/User/{id}/{saleid}",
                defaults: new { controller = "Sales", action = "User" },
                constraints: null,
                handler: authRouteHandler
            );
            config.Routes.MapHttpRoute(
                name: "TagRoute",
                routeTemplate: "api/Tags/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: null,
                handler: authRouteHandler
            );
            config.Routes.MapHttpRoute(
                name: "GenreRoute",
                routeTemplate: "api/Genres/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: null,
                handler: authRouteHandler
            );
            config.Routes.MapHttpRoute(
                name: "GameRoute",
                routeTemplate: "api/Games/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: null,
                handler: authRouteHandler
            );
            config.Routes.MapHttpRoute(
                name: "CartRoute",
                routeTemplate: "api/Carts/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: null,
                handler: authRouteHandler
            );
        }
    }
}
