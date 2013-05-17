using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EKContent.web.Models.Entities
{
    public class TwitterKeys
    {
        public string ApplicationKey { get; set; }
        public string ApplicationSecret { get; set; }
        public string ApplicationAuthorizationKey { get; set; }
        public string ApplicationAuthorizationSecret { get; set; }
        public string TwitterHandle { get; set; }
        public bool Configured { get; set; }
        public bool Enabled { get; set; }
    }
}