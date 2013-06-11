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
    public class TwitterController : BaseController
    {

        public TwitterController(IEKProvider dal) : base(dal)
        {
        }

        public ViewResult Index(TwitterTimelineViewModel model, int id)
        {
            model.Tweets = _service.GetTweets();
            model.TwitterLink =
                String.Format("https://twitter.com/{0}",
                              _service.Dal.TwitterKeysProvider.Get().TwitterHandle);
            model.ShowFeed = _service.ShowTwitterFeed();
            return View(model);
        }



    }
}
