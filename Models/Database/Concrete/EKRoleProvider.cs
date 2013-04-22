using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using EKContent.web.Models.Database.Abstract;
using EKContent.web.Models.Entities;

namespace EKContent.web.Models.Database.Concrete
{
    public class EKRoleProvider : IEKRoleProvider
    {
        private string userFile = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/users.js");

        public void Delete(int id)
        {
            var roles = Get().ToList();
            roles.Remove(roles.Single(d => d.Id == id));
            Set(roles);
        }

        public void Save(EKRole role)
        {
            var roles = Get().ToList();
            if (role.Users.Count > 0)
            {
                var maxEmployeeId = roles.SelectMany(d => d.Users).Count() == 0
                                        ? 0
                                        : roles.SelectMany(d => d.Users).Max(e => e.Id);
                foreach (var user in role.Users.Where(e => e.IsNew()))
                    user.Id = ++maxEmployeeId;
            }


            if (role.IsNew())
            {
                role.Id = roles.Count == 0 ? 1 : (roles.Max(d => d.Id) + 1);
                roles.Add(role);
            }
            else
            {
                var departmentToEdit = roles.Single(d => d.Id == role.Id);
                departmentToEdit.Name = role.Name;
                departmentToEdit.Users = role.Users;
            }
            Set(roles);

        }

        public void Set(List<EKRole> roles)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(userFile);
                var serializer = new JavaScriptSerializer();
                var str = serializer.Serialize(roles);
                sw.Write(str);
                sw.Flush();
            }
            finally
            {
                sw.Close();
                sw.Dispose();
                sw = null;
            }
        }

        public IEnumerable<EKRole> Get()
        {
            if (!File.Exists(userFile))
                return new List<EKRole>();

            EKRole[] roles  = null;
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(userFile);
                var str = sr.ReadToEnd();
                var serializer = new JavaScriptSerializer();
                roles = serializer.Deserialize<EKRole[]>(str);
            }
            finally
            {
                sr.Close();
                sr.Dispose();
                sr = null;
            }
            return roles.ToList();
        }
    }
}