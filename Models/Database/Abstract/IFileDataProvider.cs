using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EKContent.web.Models.Entities;

namespace EKContent.web.Models.Database.Abstract
{
    public interface IFileDataProvider
    {
        List<EKFile> Get();
        void Save(EKFile image);
        void Delete(int id);
    }
}