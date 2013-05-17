using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EKContent.web.Infrastructure;
using EKContent.web.Models.Database.Concrete;
using EKContent.web.Models.Entities;
using EKContent.web.Models.Services;

namespace EKContent.web.ThirdParty.Twitter
{
    public class TwitterBaseClass
    {
        protected PageService Service()
        {
            //return (PageService)NinjectControllerFactory.NinjectKernel.GetService(typeof(PageService));
            return new PageService(new EKProvider());
        }

        protected TwitterKeys Keys()
        {
            return Service().GetTwitterKeys();
        }


        protected string ConsumerKey
        {
            get { return Keys().ApplicationKey; }
        }

        protected string ConsumerSecret
        {
            get { return Keys().ApplicationSecret; }
        }
    }
}