using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Buy
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
              name: "Ticket",
              url: "t/{action}/{id}",
              defaults: new { controller = "Tickets", action = "Details", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "User",
                url: "u/{action}/{id}",
                defaults: new { controller = "Company", action = "Index", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Coupon", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
