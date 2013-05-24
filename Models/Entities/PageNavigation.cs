using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EKContent.web.Models.Entities
{


    public class PageNavigation : BaseContent
    {
        public PageNavigation Clone()
        {
 
            return new PageNavigation
                       {
 
                           Id = 0,
                           ParentId = this.Id,
                           Title = this.Title,
                           Active = this.Active,
                           Description = this.Description,
                           ShowTitle = this.ShowTitle,
                           ShowTwitterFeed = this.ShowTwitterFeed
                       };
        }
        public int? ParentId { get; set; }
        public bool ShowTwitterFeed { get; set; }

        public bool ShowPageDescriptionInHeroUnit
        {
            get;
            set;
        }

        public bool ShowHeroUnit()
        {
            return this.ShowPageDescriptionInHeroUnit && !String.IsNullOrEmpty(Description);
        }

        public bool ShowSimpleDescription()
        {
            return !String.IsNullOrEmpty(Description) && !ShowPageDescriptionInHeroUnit;
        }

        public PageTypes PageType { get; set; }

        public bool IsHomePage()
        {
            return !ParentId.HasValue;
        }
    }
}