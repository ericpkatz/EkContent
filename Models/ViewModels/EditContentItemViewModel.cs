using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EKContent.web.Models.Entities;

namespace EKContent.web.Models.ViewModels
{
    public class EditContentItemViewModel : BaseViewModel
    {
        public int Idx { get; set; }
        public int Mdx { get; set; }
        public int PageId { get; set; }
        public bool Inserting()
        {
            return Idx == -1;
        }
        public Content Content { get; set; }

        public string TitleText()
        {
            var action = Inserting() ? "Create" : "Update";
            return String.Format("{0} {1}", action, " Content");
        }

    }
}