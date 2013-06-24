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
        public int FileId { get; set; }
        public bool ShowAddThis { get; set; }

        public DateTime DatePublished { get; set; }

        [ScriptIgnore]
        public Image Image { get; set; }

        [ScriptIgnore]
        public EKFile File { get; set; }

        public bool BorderImage { get; set; }
    }
}