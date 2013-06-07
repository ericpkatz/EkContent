using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using EKContent.web.Models.Database.Abstract;
using EKContent.web.Models.Entities;
using EKContent.web.Models.Services;
using EKContent.web.Models.ViewModels;
using EKContent.web.ThirdParty.Twitter;
using EKContent.web.Utilities;

namespace EKContent.web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ColorController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.Service = _service;
        }
        private PageService _service;
        public ColorController(IEKProvider dal)
        {
            _service = new PageService(dal);
        }




        public ActionResult Edit(int id)
        {
            var model = new ColorEditViewModel { Colors = _service.Dal.ColorProvider.Get() };
            model.NavigationModel = HomeIndexViewModelLoader.Create(id, _service);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ColorEditViewModel model)
        {

            _service.Dal.ColorProvider.Set(model.Colors);
            TempData["message"] = "Color settings have been set";
            return RedirectToAction("Edit", new {id = model.NavigationModel.Page.PageNavigation.Id});
        }

        [HttpPost]
        public ActionResult Add(ColorEditViewModel model)
        {
            ModelState.Clear();
            model.NavigationModel = HomeIndexViewModelLoader.Create(model.NavigationModel.Page.PageNavigation.Id, _service);
            model.Colors.Add(new EKContent.web.Models.Entities.Color {});
            return View("Edit", model);
        }
        [HttpPost]
        public ActionResult Remove(ColorEditViewModel model, int colorIdx)
        {
            ModelState.Clear();
            model.NavigationModel = HomeIndexViewModelLoader.Create(model.NavigationModel.Page.PageNavigation.Id, _service);
            model.Colors.RemoveAt(colorIdx);
            return View("Edit",  model);
        }

    }
}
