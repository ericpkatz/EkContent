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
    public class StyleSettingsController : BaseController
    {

        public StyleSettingsController(IEKProvider dal) : base(dal)
        {
        }



        public ActionResult Edit(int id)
        {
            var model = new StyleSettingsEditViewModel {  };
            model.Settings = _service.StylesProvider().VariablesList();
            model.Colors = _service.Dal.ColorProvider.Get();
            model.NavigationModel = HomeIndexViewModelLoader.Create(id, _service);
            return View(model);
        }

        [HttpPost]
        public ActionResult Generate(StyleSettingsEditViewModel model)
        {
            _service.SetStyleSettings(new StyleSettings { Settings = model.Settings });
            _service.StylesProvider().Generate();
            TempData["message"] = "Style sheets have been generated";
            return RedirectToAction("Edit", new { id = model.NavigationModel.Page.PageNavigation.Id });
        }

        [HttpPost]
        public ActionResult Revert(int id)
        {
            _service.StylesProvider().Revert();
            TempData["message"] = "Style sheets have been reverted";
            return RedirectToAction("Edit", new { id = id });
        }

        [HttpPost]
        public ActionResult Edit(StyleSettingsEditViewModel model)
        {
            _service.SetStyleSettings(new StyleSettings{Settings =  model.Settings});
            TempData["message"] = "Style settings have been set";
            return RedirectToAction("Edit", new { id = model.NavigationModel.Page.PageNavigation.Id });
        }

    }
}
