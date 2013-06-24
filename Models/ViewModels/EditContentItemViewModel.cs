using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        private List<Image> _images;
        public List<Image> Images
        {
            set { _images = value; }
            get { return _images = _images ?? new List<Image>(); }
        }

        public List<SelectListItem> SelectListImages()
        {
            var items = 
                Images.Select(
                    i =>
                    new SelectListItem
                        {Value = i.Id.ToString(), Text = i.Title.ToString(), Selected = Content.ImageId == i.Id}).ToList();
            items.Insert(0, new SelectListItem {Text = "None", Value = "0"});
            return items;
        }

        private List<EKFile> _files;
        public List<EKFile> Files
        {
            set { _files = value; }
            get { return _files = _files ?? new List<EKFile>(); }
        }

        public List<SelectListItem> SelectListFiles()
        {
            var items =
                Files.Select(
                    i =>
                    new SelectListItem { Value = i.Id.ToString(), Text = i.Title.ToString(), Selected = Content.ImageId == i.Id }).ToList();
            items.Insert(0, new SelectListItem { Text = "None", Value = "0" });
            return items;
        }
    }
}