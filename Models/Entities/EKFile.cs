using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace EKContent.web.Models.Entities
{
    public class EKFile : BaseContent
    {
        public string FileName { get; set; }

        public string RelPath()
        {
            return String.Format("~/user_files/{0}", this.FileName);
        }

        public string AbsolutePath()
        {
            return System.Web.HttpContext.Current.Server.MapPath(RelPath());
        }

        public string NameWithExtension()
        {
            return String.Format("{0}{1}", Title, Path.GetExtension(FileName));
        }
    }
}