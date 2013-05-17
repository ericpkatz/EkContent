using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using EKContent.web.Models.Database.Abstract;
using EKContent.web.Models.Services;
using EKContent.web.Models.ViewModels;
using EKContent.web.ThirdParty.Twitter;
using Newtonsoft.Json.Linq;

namespace EKContent.web.Controllers
{
    public class TwitterController : Controller
    {
        private PageService _service;
        public TwitterController(IEKProvider dal)
        {
            _service = new PageService(dal);
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
        public ViewResult Index()
        {
            var model = new TwitterTimelineViewModel
                            {
                                Tweets = _service.GetTweets(),
                                TwitterLink =
                                    String.Format("https://twitter.com/{0}",
                                                  _service.Dal.TwitterKeysProvider.Get().TwitterHandle),
                                                  ShowFeed = _service.ShowTwitterFeed()
                            };
            return View(model);
        }



    }
}
