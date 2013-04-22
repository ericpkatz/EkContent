using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EKContent.web.Models.Database.Concrete;

namespace EKContent.web.Models.Database.Abstract
{
    public interface IEKRoleProvider
    {
        IEnumerable<EKRole> Get();
        void Set(List<EKRole> roles);
        void Delete(int id);
        void Save(EKRole role);
    }
}