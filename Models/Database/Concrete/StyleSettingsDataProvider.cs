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
    public class StyleSettingsDataProvider : IStyleSettingsDataProvider
    {
        string file = System.Web.HttpContext.Current.Server.MapPath(String.Format("~/App_Data/styleSettings.js"));

        public StyleSettings Get()
        {
            StyleSettings styleSettings = null;
            if (!File.Exists(file))
            {
                styleSettings = new StyleSettings
                               {
                                  
                               };
                Save(styleSettings);
                return styleSettings;
            }
            styleSettings = null;
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(file);
                var str = sr.ReadToEnd();
                var serializer = new JavaScriptSerializer();
                styleSettings = serializer.Deserialize<StyleSettings>(str);
            }
            finally
            {
                sr.Close();
                sr.Dispose();
                sr = null;
            }
            return styleSettings;
        }


        public void Save(StyleSettings styleSettings)
        {

            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(file);
                var serializer = new JavaScriptSerializer();
                var str = serializer.Serialize(styleSettings);
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