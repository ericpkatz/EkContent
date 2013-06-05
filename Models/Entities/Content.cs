using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace EKContent.web.Models.Entities
{
    public class Content : BaseContent
    {
        public string Body { get; set; }
        public int ImageId { get; set; }
        public bool ShowAddThis { get; set; }

        public DateTime DatePublished { get; set; }

        [ScriptIgnore]
        public Image Image { get; set; }
    }
}