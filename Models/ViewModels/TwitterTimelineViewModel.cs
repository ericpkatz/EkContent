using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EKContent.web.Models.ViewModels
{
    public class TwitterTimelineViewModel : BaseViewModel
    {
        public string TwitterLink { get; set; }
        public Newtonsoft.Json.Linq.JArray Tweets { get; set; }
        public bool ShowFeed { get; set; }

    }
}