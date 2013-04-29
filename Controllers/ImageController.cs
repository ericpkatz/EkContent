using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EKContent.web.Models.Database.Abstract;
using EKContent.web.Models.Services;
using EKContent.web.Models.ViewModels;
using EKContent.web.Models.Entities;
using System.IO;

namespace EKContent.web.Controllers
{
    [Authorize(Roles="Admin")]
    public class ImageController : Controller
    {
        private PageService _service;

        public ImageController(INavigationProvider navigationProvider, IEkDataProvider dataProvider, IEkSiteDataProvider siteProvider, IImageDataProvider imageProvider)
        {
            _service = new PageService(navigationProvider, dataProvider, siteProvider, imageProvider);
        }

        public ActionResult List(int id)
        {
            var model = new ImageListViewModel{Images = _service.GetImages()};
            model.NavigationModel = HomeIndexViewModelLoader.Create(id, _service);
            return View(model);
        }

        public ActionResult Create(int pageId, int? id)
        {
            var image = id.HasValue ? _service.GetImages().Single(i=>i.Id == id.Value) : new Image{};
            var model = new ImageCreateViewModel { Image = image };
            model.NavigationModel = HomeIndexViewModelLoader.Create(pageId, _service);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ImageCreateViewModel model)
        {
            if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
            {
                var fileName = this.Request.Files[0].FileName;
                var extension = Path.GetExtension(fileName);
                model.Image.FileName = String.Format("{0}{1}", Guid.NewGuid(), extension);
                //delete the old file
                if(!model.Image.IsNew())
                {
                    var oldImage = _service.GetImages().Single(i => i.Id == model.Image.Id).FileName;
                    var oldImagePath = this.Server.MapPath(String.Format("~/user_images/{0}", oldImage));
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }
                
            }
            var saveTo = this.Server.MapPath(String.Format("~/user_images/{0}", model.Image.FileName));
            _service.SaveImage(model.Image);
            this.Request.Files[0].SaveAs(saveTo);
            TempData["message"] = "Image Saved";
            return RedirectToAction("List", new {id = model.NavigationModel.Page.Id });
        }

        [HttpPost]
        public ActionResult Delete(int pageId, int id)
        {
            var image = _service.GetImages().Single(i => i.Id == id);
            _service.DeleteImage(image.Id);
            var savedImage = this.Server.MapPath(String.Format("~/user_images/{0}", image.FileName));
            if (System.IO.File.Exists(savedImage))
                System.IO.File.Delete(savedImage);
            TempData["message"] = "Image Removed";
            return RedirectToAction("List", new { id = pageId });
        }

        public ActionResult Logout(int id)
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home", new { id = id });
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!Membership.ValidateUser(model.Username, model.Password))
            {
                ModelState.AddModelError("", "Username and/or Password are incorrect");
                model.NavigationModel = HomeIndexViewModelLoader.Create(model.NavigationModel.Page.Id, _service);
                return View(model);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(model.Username, false);
                return RedirectToAction("Index", "Home", new { id = model.NavigationModel.Page.Id });
            }
        }

        public ActionResult ChangePassword(int id)
        {
            var model = new PasswordChangeViewModel { };
            model.NavigationModel = HomeIndexViewModelLoader.Create(id, _service);
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePassword(PasswordChangeViewModel model)
        {
            model.NavigationModel = HomeIndexViewModelLoader.Create(model.NavigationModel.Page.Id, _service);
            if (!Membership.ValidateUser(Membership.GetUser().UserName, model.OldPassword))
                ModelState.AddModelError("OldPassword", "Incorrect Password");
            ModelState["NavigationModel.Page.Title"].Errors.Clear();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = Membership.GetUser();
            user.ChangePassword(model.OldPassword, model.NewPassword);
            TempData["message"] = "Password has been changed";
            return RedirectToAction("Index", "Home", new { id = model.NavigationModel.Page.Id});

            
        }

    }
}
