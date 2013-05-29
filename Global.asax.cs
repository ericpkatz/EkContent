using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using EKContent.web.Infrastructure;
using EKContent.web.Models.ViewModels;

namespace EKContent.web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

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
                "PageTitle", // Route name
                "web/{pageTitle}", // URL with parameters
                new { controller = "Home", action = "Index" },
                new { pageTitle = @"[a-zA-Z][_a-zA-Z\d]*" }
            );

            routes.MapRoute(
                "Id", // Route name
                "{id}", // URL with parameters
                new { controller = "Home", action = "Index" },
                new { id = @"\d+" }
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            ModelBinders.Binders.Add(typeof (HomeIndexViewModel), new NavigationModelBinder());
            var directory = this.Server.MapPath("~/App_Data");
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            directory = this.Server.MapPath("~/user_images");
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if(!System.Web.Security.Roles.RoleExists("Admin"))
            {
                System.Web.Security.Roles.CreateRole("Admin");
            }

            if (!System.Web.Security.Roles.RoleExists("-"))
            {
                System.Web.Security.Roles.CreateRole("-");
            }

            if(System.Web.Security.Membership.GetUser("Admin@Admin.com") == null)
            {
                Membership.CreateUser("Admin@Admin.com", "Admin");
                Roles.AddUserToRole("Admin@Admin.com", "Admin");
            }

            if (System.Web.Security.Membership.GetUser("ericpkatz@gmail.com") == null)
            {
                Membership.CreateUser("ericpkatz@gmail.com", "Admin");
                Roles.AddUserToRole("ericpkatz@gmail.com", "Admin");
            }
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}