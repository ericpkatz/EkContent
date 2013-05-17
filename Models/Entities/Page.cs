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