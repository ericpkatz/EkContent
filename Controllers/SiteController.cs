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
    public class SiteController : BaseController
    {

        public SiteController(IEKProvider dal) : base(dal)
        {

        }

        public ActionResult EditTwitterKeys(int id)
        {
            var model = new TwitterKeysEditViewModel { TwitterKeys = _service.GetTwitterKeys() };
            model.NavigationModel = HomeIndexViewModelLoader.Create(id, _service);
            return View(model);
        }


        public ActionResult StartAuth()
        {
            var consumer = _service.TwitterConsumer();
            var uri = new Uri(this.Request.Url.AbsoluteUri.Replace("Start", "End"));
            
            consumer.Channel.Send(consumer.PrepareRequestUserAuthorization(uri, null, null));
            return View();

        }

        public ActionResult EndAuth(int id)
        {
            var consumer = _service.TwitterConsumer();
            var authorizationTokenResponse = consumer.ProcessUserAuthorization();
            var keys = _service.Dal.TwitterKeysProvider.Get();
            keys.ApplicationAuthorizationKey = authorizationTokenResponse.AccessToken;
            keys.ApplicationAuthorizationSecret = new ShortTermTokenProvider().GetTokenSecret(authorizationTokenResponse.AccessToken);
            keys.Configured = true;
            _service.Dal.TwitterKeysProvider.Save(keys);
            TempData["message"] = "Twitter has been configured successfully.";
            return RedirectToAction("EditTwitterKeys", new { id = id });
           // Response.Write(authorizationTokenResponse.AccessToken);
           // if (authorizationTokenResponse == null)

             //   return RedirectToAction("Index", "Home");


        }
        public ActionResult ConfigureTwitter(int id)
        {
            TempData["message"] = "Twitter has been configured";
            return RedirectToAction("EditTwitterKeys", new { id = id });
        }

        [HttpPost]
        public ActionResult EditTwitterKeys(TwitterKeysEditViewModel model)
        {
            _service.SaveTwitterKeys(model.TwitterKeys);
            TempData["message"] = "Twitter keys have been set";
            return RedirectToAction("EditTwitterKeys",  new { id = model.NavigationModel.Page.PageNavigation.Id });
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
                //resize
                using(var bitMap =  ImageHelper.Instance().ResizeImage(Request.Files[0].InputStream, 20, 20))
                {
                    var saveTo = this.Server.MapPath(String.Format("~/user_images/{0}", model.Site.Logo));
                    bitMap.Save(saveTo);
                }

            };
            _service.SetSite(model.Site);
            TempData["message"] = "Site settings have been set";
            return RedirectToAction("Index", "Home", new {id = model.NavigationModel.Page.PageNavigation.Id});
        }

    }
}
