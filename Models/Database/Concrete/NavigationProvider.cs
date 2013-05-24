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
    public class NavigationProvider : INavigationProvider
    {
        private string navigationFile = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/pages.js");

        public void Save(List<PageNavigation> pages)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(navigationFile);
                var serializer = new JavaScriptSerializer();
                var str = serializer.Serialize(pages);
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

        public List<PageNavigation> GetNavigation()
        {
            if(!File.Exists(navigationFile))
            {
                StreamWriter sw = null;
                try
                {
                    sw = new StreamWriter(navigationFile);
                    var page = new PageNavigation
                                   {
                                       Id = 1,
                                       ParentId = null,
                                       Title = "Home",
                                       DateCreated = DateTime.Now,
                                       DateModified = DateTime.Now,
                                       Active = true
                                   };
                    var pages = new List<PageNavigation> {page};
                    var serializer = new JavaScriptSerializer();
                    var str = serializer.Serialize(pages);
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
            PageNavigation[] navigation = null;
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(navigationFile);
                var str = sr.ReadToEnd();
                var serializer = new JavaScriptSerializer();
                navigation = serializer.Deserialize<PageNavigation[]>(str);
            }
            finally
            {
                sr.Close();
                sr.Dispose();
                sr = null;
            }
            return navigation.ToList();
        }
    }
}