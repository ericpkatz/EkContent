using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EKContent.web.Models.Entities
{
    public enum PageTypes
    {
        Blog,
        HTML,
        Contact
    }

    public class Page : BaseContent
    {
        public Page Clone()
        {
            return new Page
                       {
                           Id = 0,
                           ParentId = this.Id,
                           Title = this.Title,
                           Active = this.Active,
                           Description = this.Description,
                           Modules = this.Modules,
                           ShowTitle = this.ShowTitle,
                           ShowTwitterFeed = this.ShowTwitterFeed
                       };
        }
        public int? ParentId { get; set; }
        public bool ShowTwitterFeed { get; set; }
        public PageTypes PageType { get; set; }
        private List<Module> _modules = null;
        public List<Module> Modules
        {
            get { return _modules = _modules ?? new List<Module>(); }
            set { _modules = value; }
        }
        public bool IsHomePage()
        {
            return !ParentId.HasValue;
        }
    }
}