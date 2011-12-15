﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VoxPoliticus.Models;

namespace VoxPoliticus
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public static class VoxPoliticusDatabase
    {
        //
        public static User[] Users = new[]
                                  {
                                      new User{Id = "beblavy", Name = "Miroslav Beblavý", Sources = new Source[]
                                                                                                        {
                                                                                                            new RssSource("http://beblavy.blog.sme.sk/rss/")
                                                                                                        }},
                                      new User{Id = "sulik", Name = "Richard Sulík", Sources = new Source[]
                                                                                                        {
                                                                                                            new RssSource("http://richardsulik.blog.sme.sk/rss/")
                                                                                                        }},
                                      new User{Id = "fico", Name = "Robert Fico", Sources = new Source[]
                                                                                                        {
                                                                                                            new RssSource("http://fico.blog.sme.sk/rss/")
                                                                                                        }},
                                      new User{Id = "kanik", Name = "Ľudovít Kaník", Sources = new Source[]
                                                                                                        {
                                                                                                            new RssSource("http://moje.hnonline.sk/blog/1946/feed")
                                                                                                        }},

                                  };
    }

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Feed", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}