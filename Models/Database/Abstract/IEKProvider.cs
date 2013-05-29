using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EKContent.web.Models.Database.Abstract
{
    public interface IEKProvider
    {
        INavigationProvider NavigationProvider{get;}
        IEkDataProvider DataProvider{ get;} 
        IEkSiteDataProvider SiteProvider{get; }
        IImageDataProvider ImageProvider{get; }
        IEKRoleProvider RoleProvider { get; }
        ITwitterKeysProvider TwitterKeysProvider { get;  }
        IStyleSettingsDataProvider StyleSettingsProvider { get;  }
    }
}
