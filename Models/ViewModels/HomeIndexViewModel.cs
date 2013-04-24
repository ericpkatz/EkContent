using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EKContent.web.Models.Entities;

namespace EKContent.web.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<Page> Pages { get; set; }
        public Site Site { get; set; }
        public Page Page { get; set; }
        public bool HasChildren()
        {
            return Pages.Any(p => p.ParentId == Page.Id);
        }
        public bool CanDelete()
        {
            if (Page.IsHomePage())
                return false;
            if (HasChildren())
                return false;
            return true;
        }

        public Page SelectedNavigation()
        {
            return Pages.Single(p => p.Id == Page.Id);
        }
    }
}