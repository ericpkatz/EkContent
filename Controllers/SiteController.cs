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
    public class SiteController : Controller
    {

        private PageService _service;
        public SiteController(IEKProvider dal)
        {
            _service = new PageService(dal);
        }

        public ActionResult EditTwitterKeys(int id)
        {
            var model = new TwitterKeysEditViewModel { TwitterKeys = _service.GetTwitterKeys() };
            model.NavigationModel = HomeIndexViewModelLoader.Create(id, _service);
            return View(model);
        }

        private ServiceProviderDescription ServiceProviderDescription()
        {

            return new ServiceProviderDescription

            {
                AccessTokenEndpoint = new MessageReceivingEndpoint("     https://api.twitter.com/oauth/access_token", HttpDeliveryMethods.PostRequest),
                RequestTokenEndpoint = new MessageReceivingEndpoint("     https://api.twitter.com/oauth/request_token", HttpDeliveryMethods.PostRequest),
                UserAuthorizationEndpoint = new MessageReceivingEndpoint("https://api.twitter.com/oauth/authorize", HttpDeliveryMethods.PostRequest),
                TamperProtectionElements = new ITamperProtectionChannelBindingElement[] { new HmacSha1SigningBindingElement() },
                ProtocolVersion = ProtocolVersion.V10a
            };

        }

        public ActionResult StartAuth()
        {
            var consumer = new DotNetOpenAuth.OAuth.WebConsumer(this.ServiceProviderDescription(), new ShortTermTokenProvider());
            var uri = new Uri(this.Request.Url.AbsoluteUri.Replace("Start", "End"));
            
            consumer.Channel.Send(consumer.PrepareRequestUserAuthorization(uri, null, null));
            return View();

        }

        public ActionResult EndAuth(int id)
        {
            var consumer = new DotNetOpenAuth.OAuth.WebConsumer(
this.ServiceProviderDescription(),
new ShortTermTokenProvider());
            var authorizationTokenResponse = consumer.ProcessUserAuthorization();
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
                var saveTo = this.Server.MapPath(String.Format("~/user_images/{0}", model.Site.Logo));
                this.Request.Files[0].SaveAs(saveTo);
            };
            _service.SetSite(model.Site);
            TempData["message"] = "Site settings have been set";
            return RedirectToAction("Index", "Home", new {id = model.NavigationModel.Page.PageNavigation.Id});
        }

    }
}
