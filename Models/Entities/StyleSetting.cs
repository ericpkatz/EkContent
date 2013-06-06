using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace EKContent.web.Models.Entities
{
    public class StyleSetting
    {
        public string Key { get; set; }
        public string Value { get; set; }
       [ScriptIgnore]
        public string DefaultValue { get; set; }

        public bool isDefault()
        {
            return Value == DefaultValue;
        }
    }
}