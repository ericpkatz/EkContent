using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EKContent.web.Models.Entities
{
    public class BaseContent
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int SortOrder { get; set; }
    }
}