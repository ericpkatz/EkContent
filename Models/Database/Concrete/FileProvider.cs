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
    public class FileProvider : IFileDataProvider
    {

        string file = System.Web.HttpContext.Current.Server.MapPath(String.Format("~/App_Data/files.js"));

        public List<EKFile> Get()
        {
            if (!File.Exists(file))
            {
                return new List<EKFile>();
            }
            List<EKFile> files = null;
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(file);
                var str = sr.ReadToEnd();
                var serializer = new JavaScriptSerializer();
                files = serializer.Deserialize<EKFile[]>(str).ToList();
            }
            finally
            {
                sr.Close();
                sr.Dispose();
                sr = null;
            }
            return files;
        }


        public void Save(EKFile file)
        {
            var files = Get();
            if (file.IsNew())
            {
                var maxId = files.Count == 0 ? 1 : files.Max(i => i.Id) + 1;
                file.Id = maxId;
                file.DateCreated = DateTime.Now;
                files.Add(file);
            }
            else
            {
                var fileToEdit = files.Where(i => i.Id == file.Id).Single();
                fileToEdit.Title = file.Title;
                fileToEdit.Description = file.Description;
                fileToEdit.FileName = file.FileName;
                fileToEdit.DateModified = DateTime.Now;
            }
            Persist(files);
        }

        public void Delete(int id)
        {

            var files = Get();
            var file = files.Where(i => i.Id == id).Single();
            files.Remove(file);
            Persist(files);
        }

        private void Persist(List<EKFile> files)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(file);
                var serializer = new JavaScriptSerializer();
                var str = serializer.Serialize(files);
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