using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EKContent.web.Models.Entities;

namespace EKContent.web.Models.Database.Abstract
{
    public interface IImageDataProvider
    {
        List<Image> Get();
        void Save(Image image);
        void Delete(int id);
    }
}