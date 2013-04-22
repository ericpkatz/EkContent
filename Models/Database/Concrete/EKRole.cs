using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EKContent.web.Models.Database.Concrete
{
    public class EKRole
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsNew()
        {
            return Id == 0;
        }

        private List<EKUser> _users = null;

        public List<EKUser> Users
        {
            set { _users = value; }
            get { return _users = _users ?? new List<EKUser>(); }
        }
    }
}