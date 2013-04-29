using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EKContent.web.Models.Entities;

namespace EKContent.web.Models.ViewModels
{
    public class ImageListViewModel : BaseViewModel
    {
        public List<Image> Images { get; set; }
    }
}