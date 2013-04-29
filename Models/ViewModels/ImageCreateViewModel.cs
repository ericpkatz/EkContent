using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EKContent.web.Models.Entities;

namespace EKContent.web.Models.ViewModels
{
    public class ImageCreateViewModel : BaseViewModel
    {
        public Image Image { get; set; }
    }
}