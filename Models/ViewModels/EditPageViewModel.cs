using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EKContent.web.Models.Entities;

namespace EKContent.web.Models.ViewModels
{
    public class EditPageViewModel : BaseViewModel
    {
        public Page Page { get; set; }
        public int? ParentId { get; set; }
        public int? PageId { get; set; }
        public List<PageTypes> PageTypes()
        {
            return Enum.GetValues(typeof (PageTypes)).Cast<PageTypes>().ToList();
        }

        public List<SelectListItem> PageTypesSelectList()
        {
            return
                PageTypes().Select(
                    pt =>
                    new SelectListItem {Text = pt.ToString(), Value = pt.ToString(), Selected = Page.PageType == pt}).
                    ToList();
        }

        public bool Inserting()
        {
            return ParentId.HasValue;
        }
        public int Id()
        {
            if (ParentId.HasValue)
                return ParentId.Value;
            else
                return PageId.Value;
        }

        public string TitleText()
        {
            var action =  Inserting() ? "Create" : "Update";
            return String.Format("{0} {1}", action, " Page");
        }

    }
}