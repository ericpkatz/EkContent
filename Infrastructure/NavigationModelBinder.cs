using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EKContent.web.Models.Database.Concrete;
using EKContent.web.Models.Services;
using EKContent.web.Models.ViewModels;

namespace EKContent.web.Infrastructure
{
    public class NavigationModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext ctx, ModelBindingContext modelCtx)
        {
            var service = new PageService(new EKProvider());
            int? id = null;
            var userMode = false;
            if (modelCtx.ValueProvider.GetValue("NavigationModel.Page.Id") != null)
                id = int.Parse(modelCtx.ValueProvider.GetValue("NavigationModel.Page.Id").AttemptedValue);
            else if (modelCtx.ValueProvider.GetValue("id") != null)
                id = int.Parse(modelCtx.ValueProvider.GetValue("id").AttemptedValue);
            else if (modelCtx.ValueProvider.GetValue("pageTitle") != null && service.GetNavigation().FirstOrDefault(p => modelCtx.ValueProvider.GetValue("pageTitle").AttemptedValue == p.PagePath()) != null)
            {
                id = service.GetNavigation().First(p => modelCtx.ValueProvider.GetValue("pageTitle").AttemptedValue == p.PagePath()).Id;
            }
            if (modelCtx.ValueProvider.GetValue("UserMode") != null)
                userMode = bool.Parse(modelCtx.ValueProvider.GetValue("UserMode").AttemptedValue);
            var model = HomeIndexViewModelLoader.Create(id, new PageService(new EKProvider()));
            model.UserMode = userMode;
            return model;
            
            return null;
        }
    }
}