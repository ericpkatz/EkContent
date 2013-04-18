using System.Linq;
using EKContent.web.Models.Entities;

namespace EKContent.web.Models.Database.Abstract
{
    public interface IPageProvider
    {
        IQueryable<Page> Pages { get; set; }
    }
}