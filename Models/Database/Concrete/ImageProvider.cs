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
    public class ImageProvider : IImageDataProvider
    {

        string file = System.Web.HttpContext.Current.Server.MapPath(String.Format("~/App_Data/images.js"));

        public List<Image> Get()
        {
            if (!File.Exists(file))
            {
                return new List<Image>();
            }
            List<Image> images = null;
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(file);
                var str = sr.ReadToEnd();
                var serializer = new JavaScriptSerializer();
                images = serializer.Deserialize<Image[]>(str).ToList();
            }
            finally
            {
                sr.Close();
                sr.Dispose();
                sr = null;
            }
            return images;
        }


        public void Save(Image image)
        {
            var images = Get();
            if (image.IsNew())
            {
                var maxId = images.Count == 0 ? 1 : images.Max(i => i.Id) + 1;
                image.Id = maxId;
                image.DateCreated = DateTime.Now;
                images.Add(image);
            }
            else
            {
                var imageToEdit = images.Where(i => i.Id == image.Id).Single();
                imageToEdit.Title = image.Title;
                imageToEdit.Description = image.Description;
                imageToEdit.FileName = image.FileName;
                imageToEdit.DateModified = DateTime.Now;
            }
            Persist(images);
        }

        public void Delete(int id)
        {

            var images = Get();
            var image = images.Where(i => i.Id == id).Single();
            images.Remove(image);
            Persist(images);
        }

        private void Persist(List<Image> images)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(file);
                var serializer = new JavaScriptSerializer();
                var str = serializer.Serialize(images);
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