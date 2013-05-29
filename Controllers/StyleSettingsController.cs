using System;
using System.Collections.Generic;
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

namespace EKContent.web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StyleSettingsController : Controller
    {

        private PageService _service;
        public StyleSettingsController(IEKProvider dal)
        {
            _service = new PageService(dal);
        }

      

        public ActionResult Edit(int id)
        {
            var model = new StyleSettingsEditViewModel { StyleSettings = _service.GetStyleSettings() };
            model.NavigationModel = HomeIndexViewModelLoader.Create(id, _service);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(StyleSettingsEditViewModel model)
        {
            _service.SetStyleSettings(model.StyleSettings);
            TempData["message"] = "Style settings have been set";
            return RedirectToAction("Edit",  new {id = model.NavigationModel.Page.PageNavigation.Id});
        }

    }
}
