using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EKContent.web.Models.Entities;

namespace EKContent.web.Models.Database.Abstract
{
    public interface IEkDataProvider
    {
        List<Module> Get(int id);
        void Save(Page page);
        void Delete(int id);
    }
}