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
        [RegularExpression(@"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$", ErrorMessage="Email is not in valid format.")]
        public string Email { get; set; }
        public string Body { get; set; }
        public bool IsReview { get; set; }
    }
}