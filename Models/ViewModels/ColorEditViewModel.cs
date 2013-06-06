using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EKContent.web.Models.Entities;

namespace EKContent.web.Models.ViewModels
{
    public class ColorEditViewModel : BaseViewModel
    {
        private List<Color> _colors = null;
        public List<Color> Colors
        {
            set { _colors = value; }
            get { return _colors = _colors ?? new List<Color>(); }
        }
    }
}