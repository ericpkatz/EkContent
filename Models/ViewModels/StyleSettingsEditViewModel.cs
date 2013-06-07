using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EKContent.web.Models.Entities;

namespace EKContent.web.Models.ViewModels
{
    public class StyleSettingsEditViewModel : BaseViewModel
    {
        public StyleSettings StyleSettings { get; set; }
        public List<StyleSetting> Settings { get; set; }
        public List<Color> Colors { get; set; }
    }
}