using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EKContent.web.Models.Database.Abstract;

namespace EKContent.web.Models.Database.Concrete
{
    public class EKProvider : IEKProvider
    {
        public INavigationProvider NavigationProvider
        {
            get { return new NavigationProvider(); }
        }

        public IEkDataProvider DataProvider
        {
            get { return new EkDataProvider(); }
        }

        public IEkSiteDataProvider SiteProvider
        {
            get { return new EkSiteDataProvider(); }
        }

        public IImageDataProvider ImageProvider
        {
            get { return new ImageProvider(); }
        }


        public IEKRoleProvider RoleProvider
        {
            get { return new EKRoleProvider(); }
        }
    }
}