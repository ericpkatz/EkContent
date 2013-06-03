using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EKContent.web.Models.Entities
{
    public class Message
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Body { get; set; }
    }
}