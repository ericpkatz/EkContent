using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EKContent.web.Models.Database.Abstract;
using EKContent.web.Models.Entities;
using EKContent.web.Models.Services;
using EKContent.web.Models.ViewModels;

namespace EKContent.web.Controllers
{
    public class SiteController : Controller
    {

        private PageService _service;
        public SiteController(IEKProvider dal)
        {
            _service = new PageService(dal);
        }

        public ActionResult Edit(int id)
        {
            var model = new SiteEditViewModel { Site = _service.GetSite() };
            model.NavigationModel = HomeIndexViewModelLoader.Create(id, _service);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(SiteEditViewModel model)
        {
            if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
            {
                var fileName = this.Request.Files[0].FileName;
                var extension = Path.GetExtension(fileName);
                //delete the old file
                if (!model.Site.IsNew())
                {
                    var oldImage = _service.GetSite().Logo;
                    var oldImagePath = this.Server.MapPath(String.Format("~/user_images/{0}", oldImage));
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }
                model.Site.Logo = String.Format("{0}{1}", Guid.NewGuid(), extension);
                var saveTo = this.Server.MapPath(String.Format("~/user_images/{0}", model.Site.Logo));
                this.Request.Files[0].SaveAs(saveTo);
            };
            _service.SetSite(model.Site);
            TempData["message"] = "Site settings have been set";
            return RedirectToAction("Index", "Home", new {id = model.NavigationModel.Page.Id});
        }

    }
}
