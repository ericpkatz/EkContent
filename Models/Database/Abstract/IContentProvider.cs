using System.Linq;
using EKContent.web.Models.Entities;

namespace EKContent.web.Models.Database.Abstract
{
    public interface IContentProvider
    {
        IQueryable<Content> Content { get; set; }
    }
}