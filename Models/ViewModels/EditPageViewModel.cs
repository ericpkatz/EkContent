using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EKContent.web.Models.Entities;

namespace EKContent.web.Models.ViewModels
{
    public class EditPageViewModel
    {
        public Page Page { get; set; }
        public int? ParentId { get; set; }
        public int? PageId { get; set; }
        public HomeIndexViewModel NavigationModel { get; set; }
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