using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace EKContent.web.Models.Entities
{
    public enum PageTypes
    {
        Blog,
        HTML,
        Contact,
        Testimonial
    }

    public class Page 
    {
        [ScriptIgnore]
        public PageNavigation PageNavigation { get; set; }

        public Page Clone()
        {
            return new Page
                       {
                           PageNavigation = this.PageNavigation.Clone(),
                           Modules = this.Modules
                       };
        }
         [ScriptIgnore]
        public string Title
        {
            get { return PageNavigation.Title; }
        }
         [ScriptIgnore]
        public PageTypes PageType
        {
            get { return PageNavigation.PageType; }
        }
         [ScriptIgnore]
        public bool ShowTwitterFeed
        {
            get { return PageNavigation.ShowTwitterFeed; }
        }
         [ScriptIgnore]
        public int Id
        {
            get { return PageNavigation.Id; }
        }
         [ScriptIgnore]
        public int Priority
        {
            get { return PageNavigation.Priority; }
        }
         [ScriptIgnore]
        public string Description
        {
            get { return PageNavigation.Description; }
        }
         [ScriptIgnore]
        public bool Active
        {
            get { return PageNavigation.Active; }
        }
        private List<Module> _modules = null;
        public List<Module> Modules
        {
            get { return _modules = _modules ?? new List<Module>(); }
            set { _modules = value; }
        }
        public bool HasContent()
        {
            return Modules.SelectMany(m => m.Content).Count() > 0;
        }
        public bool IsHomePage()
        {
            return !PageNavigation.ParentId.HasValue;
        }
    }
}