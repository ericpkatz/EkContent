using System.Linq;
using EKContent.web.Models.Entities;

namespace EKContent.web.Models.Database.Abstract
{
    public interface IModuleProvider
    {
        IQueryable<Module> Modules { get; set; }
    }
}