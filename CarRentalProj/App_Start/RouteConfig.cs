using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CarRentalProj
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Client",
                url: "{controller}/{action}/{UserId}",
                defaults: new { controller = "CarRentz", action = "Home", UserId = UrlParameter.Optional }
            );
             
            routes.MapRoute(
                 name: "Server",
                 url: "{controller}/{action}/id",
                 defaults: new { contoller = "CRSS", action = "Main" }
            );

            
        }
    }
}
