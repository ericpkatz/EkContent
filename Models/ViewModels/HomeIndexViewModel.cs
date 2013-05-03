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
        public List<Page> TopLevelPages()
        {
            return Pages.Where(p =>  p.ParentId == HomePage().Id).ToList();
        }
        public Page HomePage()
        {
            return Pages.Single(p => p.IsHomePage());
        }

        public List<Page> ChildPages()
        {
            if (this.Page.IsHomePage())
                return new List<Page>();
            return Pages.Where(p => p.ParentId == Page.Id).ToList();
        }

        public Site Site { get; set; }
        public Page Page { get; set; }
        public bool HasChildren()
        {
            return Pages.Any(p => p.ParentId == Page.Id);
        }

        public bool ShowChildPages()
        {
            return ChildPages().Count > 0;
        }
        public bool CanDelete()
        {
            if (Page.IsHomePage())
                return false;
            if (HasChildren())
                return false;
            return true;
        }

        public bool ShowBreadCrumb()
        {
            return PagePathList().Count > 2;
        }

        public bool UserMode
        {
            get;
            set;
        }

        public bool EditMode()
        {
            return !UserMode;
        }


        private List<Page> _pagePathList = null;
        public List<Page> PagePathList()
        {
            if (_pagePathList != null)
                return _pagePathList;
            var page = Page;
            var pages = new List<Page>();
            do
            {
                pages.Add(page);
                page = this.Pages.SingleOrDefault(p => p.Id == page.ParentId);
            } while (page != null);
            pages.Reverse();
            _pagePathList = pages;
            return _pagePathList;
        }
    }
}