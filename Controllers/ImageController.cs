using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EKContent.web.Models.Database.Abstract;
using EKContent.web.Models.Services;
using EKContent.web.Models.ViewModels;
using System.IO;
using Image = EKContent.web.Models.Entities.Image;

namespace EKContent.web.Controllers
{
    [Authorize(Roles="Admin")]
    public class ImageController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.Service = _service;
        }
        private void ResizeImage(string lcFilename, int lnWidth = 150, int lnHeight = 150)
        {
            System.Drawing.Bitmap bmpOut = null;

            try
            {
                Bitmap loBMP = new Bitmap(lcFilename);
                ImageFormat loFormat = loBMP.RawFormat;

                decimal lnRatio;
                int lnNewWidth = 0;
                int lnNewHeight = 0;

                if (loBMP.Width < lnWidth && loBMP.Height < lnHeight)
                {
                    loBMP.Dispose();
                    return;
                }
                //return loBMP;

                if (loBMP.Width > loBMP.Height)
                {
                    lnRatio = (decimal)lnWidth / loBMP.Width;
                    lnNewWidth = lnWidth;
                    decimal lnTemp = loBMP.Height * lnRatio;
                    lnNewHeight = (int)lnTemp;
                }
                else
                {
                    lnRatio = (decimal)lnHeight / loBMP.Height;
                    lnNewHeight = lnHeight;
                    decimal lnTemp = loBMP.Width * lnRatio;
                    lnNewWidth = (int)lnTemp;
                }


                bmpOut = new Bitmap(lnNewWidth, lnNewHeight);
                Graphics g = Graphics.FromImage(bmpOut);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
                g.DrawImage(loBMP, 0, 0, lnNewWidth, lnNewHeight);

                loBMP.Dispose();
            }
            catch
            {
                return;
            }
            bmpOut.Save(lcFilename);
            bmpOut.Dispose();
        }
        private PageService _service;

        public ImageController(IEKProvider provider)
        {
            _service = new PageService(provider);
        }

        public ActionResult List(int id, HomeIndexViewModel homeIndexViewModel)
        {
            var model = new ImageListViewModel{Images = _service.GetImages()};
            model.NavigationModel = homeIndexViewModel;
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
            string bitMapName = String.Empty;
            if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
            {
                var fileName = this.Request.Files[0].FileName;
                var extension = Path.GetExtension(fileName);
                model.Image.FileName = String.Format("{0}{1}", Guid.NewGuid(), extension);
                bitMapName = String.Format("bitmap-{0}", model.Image.FileName);
                //delete the old file
                if(!model.Image.IsNew())
                {
                    var oldImage = _service.GetImages().Single(i => i.Id == model.Image.Id).FileName;
                    var oldImagePath = this.Server.MapPath(String.Format("~/user_images/{0}", oldImage));
                    var oldBitMap = this.Server.MapPath(String.Format("~/user_images/bitmap-{0}", oldImage));
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                    if (System.IO.File.Exists(oldBitMap))
                        System.IO.File.Delete(oldBitMap);
                }
                var saveTo = this.Server.MapPath(String.Format("~/user_images/{0}", model.Image.FileName));
                var saveToBitMap = this.Server.MapPath(String.Format("~/user_images/{0}", bitMapName));
                this.Request.Files[0].SaveAs(saveTo);
                ResizeImage(saveTo, 500, 500);
                System.IO.File.Copy(saveTo, saveToBitMap);
                ResizeImage(saveToBitMap, 50, 50);
            }

            _service.SaveImage(model.Image);

            TempData["message"] = "Image Saved";
            return RedirectToAction("List", new {id = model.NavigationModel.Page.PageNavigation.Id });
        }

        [HttpPost]
        public ActionResult Delete(int pageId, int id)
        {
            var image = _service.GetImages().Single(i => i.Id == id);
            _service.DeleteImage(image.Id);
            var savedImage = this.Server.MapPath(String.Format("~/user_images/{0}", image.FileName));
            var saveToBitMap = this.Server.MapPath(String.Format("~/user_images/bitmap-{0}", image.FileName));
            if (System.IO.File.Exists(savedImage))
                System.IO.File.Delete(savedImage);
            if (System.IO.File.Exists(saveToBitMap))
                System.IO.File.Delete(saveToBitMap);
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
                model.NavigationModel = HomeIndexViewModelLoader.Create(model.NavigationModel.Page.PageNavigation.Id, _service);
                return View(model);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(model.Username, false);
                return RedirectToAction("Index", "Home", new { id = model.NavigationModel.Page.PageNavigation.Id });
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
            model.NavigationModel = HomeIndexViewModelLoader.Create(model.NavigationModel.Page.PageNavigation.Id, _service);
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
            return RedirectToAction("Index", "Home", new { id = model.NavigationModel.Page.PageNavigation.Id});

            
        }

    }
}
