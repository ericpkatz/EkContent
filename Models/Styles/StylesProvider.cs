using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EKContent.web.Models.Database.Abstract;
using EKContent.web.Models.Entities;

namespace EKContent.web.Models.Styles
{
    public class StylesProvider
    {
        IEkSiteDataProvider siteProvider = null;
        public StylesProvider(IEkSiteDataProvider siteProvider)
        {
            this.siteProvider = siteProvider;
        }

        private Site _site = null;

        private Site Site
        {
            get { return _site = _site ?? siteProvider.Get(); }
        }

        private bool HasCustomStyle()
        {
            return !String.IsNullOrEmpty(Site.CustomStyleSheetIdentifier);
        }
        public string GetStyleSheet(string baseStyleSheet)
        {
            return String.Format("{0}{1}{2}.css", HasCustomStyle() ? siteProvider.Get().CustomStyleSheetIdentifier: String.Empty, HasCustomStyle() ? "-" : String.Empty,  baseStyleSheet);
        }
    }
}