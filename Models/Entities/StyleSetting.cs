using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EKContent.web.Models.Entities
{
    public class StyleSetting
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string DefaultValue { get; set; }

        public bool isDefault()
        {
            return Value == DefaultValue;
        }
    }
}