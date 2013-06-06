using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using EKContent.web.Models.Database.Abstract;
using EKContent.web.Models.Entities;

namespace EKContent.web.Models.Database.Concrete
{
    public class ColorDataProvider : IColorProvider
    {
        string file = System.Web.HttpContext.Current.Server.MapPath(String.Format("~/App_Data/colors.js"));

        public List<Color> Get()
        {
            List<Color> settings = null;
            if (!File.Exists(file))
            {
                settings = new List<Color>();
                Set(settings);
                return settings;
            }
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(file);
                var str = sr.ReadToEnd();
                var serializer = new JavaScriptSerializer();
                settings = serializer.Deserialize<List<Color>>(str);
            }
            finally
            {
                sr.Close();
                sr.Dispose();
                sr = null;
            }
            return settings;
        }

        public void Set(List<Color> colors)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(file);
                var serializer = new JavaScriptSerializer();
                var str = serializer.Serialize(colors);
                sw.Write(str);
                sw.Flush();
            }
            finally
            {
                sw.Close();
                sw.Dispose();
                sw = null;
            }
        }
    }
}
