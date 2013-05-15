using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EKContent.web.Models.Entities
{
    public class Site : BaseContent
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Logo { get; set; }
        public bool ShowLogo { get; set; }
        public string TwitterConsumerKey { get; set; }
        public string TwitterConsumerSecret { get; set; }
        public string SmtpSiteEmail { get; set; }
        public string SmtpSiteEmailPassword { get; set; }
        public string SmtpServer { get; set; }
        public bool HideLogo()
        {
            return !ShowLogo;
        }
        public string IconLogo()
        {
            if (ShowLogo && Logo != null)
                return "icon_logo";
            else
                return "icon_home";
        }
        public string IconStyleString()
        {
            var str = String.Empty;
            if(ShowLogo || Logo != null)
            {
                str = String.Format(".icon_logo{{background-image : url({0}); display:block;float:left;width:20px;height:20px;}}", new UrlHelper(System.Web.HttpContext.Current.Request.RequestContext).Content(String.Format("~/user_images/{0}", Logo) ) );
            }
            return str;

        }
    }
}