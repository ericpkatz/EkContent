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
    public class EkSiteDataProvider : IEkSiteDataProvider
    {
        string file = System.Web.HttpContext.Current.Server.MapPath(String.Format("~/App_Data/site.js"));




        public Site Get()
        {
            Site site = null;
            if (!File.Exists(file))
            {
                site = new Site
                               {
                                   Title = "New Site",
                                   Email = "Test@Test.com"
                               };
                Save(site);
                return site;
            }
            site  = null;
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(file);
                var str = sr.ReadToEnd();
                var serializer = new JavaScriptSerializer();
                site = serializer.Deserialize<Site>(str);
            }
            finally
            {
                sr.Close();
                sr.Dispose();
                sr = null;
            }
            return site;
        }


        public void Save(Site site)
        {
            string pageFile = System.Web.HttpContext.Current.Server.MapPath(String.Format("~/App_Data/site.js"));

            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(pageFile);
                var serializer = new JavaScriptSerializer();
                var str = serializer.Serialize(site);
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