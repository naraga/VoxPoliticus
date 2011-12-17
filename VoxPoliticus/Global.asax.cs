using System;
using System.Collections.Generic;
using System.Configuration;
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
        public static string GetConfigValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        //
        public static User[] Users = new[]
                                         {
                                             new User
                                                 {
                                                     Id = "beblavy", Name = "Miroslav Beblavý",
                                                     PhotoUrl = GetConfigValue("beblavy_photo"),
                                                     Tags = new[]{"sdku"},
                                                     Sources = new Source[]
                                                                   {
                                                                       new RssSource(GetConfigValue("beblavy_smeblog_rss")),
                                                                       new TwitterSource(GetConfigValue("beblavy_twitter_atom")),
                                                                   }
                                                 },
                                             new User
                                                 {
                                                     Id = "sulik", Name = "Richard Sulík",
                                                     PhotoUrl = GetConfigValue("sulik_photo"),
                                                     Tags = new[]{"sas"},
                                                     Sources = new Source[]
                                                                   {
                                                                       new RssSource(GetConfigValue("sulik_smeblog_rss"))
                                                                   }
                                                 },
                                             new User
                                                 {
                                                     Id = "fico", Name = "Robert Fico",
                                                     PhotoUrl = GetConfigValue("fico_photo"),
                                                     Tags = new[]{"smer"},
                                                     Sources = new Source[]
                                                                   {
                                                                       new RssSource(GetConfigValue("fico_smeblog_rss"))
                                                                   }
                                                 },
                                             new User
                                                 {
                                                     Id = "kanik", Name = "Ľudovít Kaník",
                                                     PhotoUrl = GetConfigValue("kanik_photo"),
                                                     Tags = new[]{"sdku"},
                                                     Sources = new Source[]
                                                                   {
                                                                       new RssSource(GetConfigValue("kanik_hnonlineblog_rss"))
                                                                   }
                                                 },

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