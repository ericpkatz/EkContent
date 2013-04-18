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
    public class EkDataProvider : IEkDataProvider
    {

        public void Save(Page page)
        {
            string pageFile = System.Web.HttpContext.Current.Server.MapPath(String.Format("~/App_Data/page{0}.js", page.Id));

                StreamWriter sw = null;
                try
                {
                    sw = new StreamWriter(pageFile);
                    var serializer = new JavaScriptSerializer();
                    var str = serializer.Serialize(page.Modules);
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

        public void Delete(int id)
        {
            string pageFile = System.Web.HttpContext.Current.Server.MapPath(String.Format("~/App_Data/page{0}.js", id));
            if (!File.Exists(pageFile))
            {
                File.Delete(pageFile);
            }
        }

        public List<Module> Get(int id)
        {
            string pageFile = System.Web.HttpContext.Current.Server.MapPath(String.Format("~/App_Data/page{0}.js", id));
            if (!File.Exists(pageFile))
            {
                StreamWriter sw = null;
                try
                {
                    sw = new StreamWriter(pageFile);
                    var page = new Page
                    {
                        Id = 1,
                        ParentId = null,
                        Title = "Home",
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        Active = true,
                        Modules = new List<Module>
                                      {
                                          new Module
                                              {
                                                  Id = 1,
                                                  Title = "Module 1",
                                                  DateCreated = DateTime.Now,
                                                  DateModified = DateTime.Now
                                              }
                                      }
                    };
                    var serializer = new JavaScriptSerializer();
                    var str = serializer.Serialize(page.Modules);
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
            Module[] modules = null;
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(pageFile);
                var str = sr.ReadToEnd();
                var serializer = new JavaScriptSerializer();
                modules = serializer.Deserialize<Module[]>(str);
            }
            finally
            {
                sr.Close();
                sr.Dispose();
                sr = null;
            }
            return modules.ToList();
        }
    }
}