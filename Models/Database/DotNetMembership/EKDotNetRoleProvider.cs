using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EKContent.web.Models.Database.Concrete;
using EKContent.web.Models.Entities;

namespace EKContent.web.Models.Database.DotNetMembership
{
    public class EKDotNetRoleProvider : System.Web.Security.RoleProvider
    {
        private EKRoleProvider database = new EKRoleProvider();
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            var username = usernames[0];
            var role = roleNames[0];
            var data = database.Get().ToList();
            var currentRole = data.Single(r => r.Users.Select(u => u.UserName.ToLower()).Contains(username.ToLower()));
            var roleToAddUserTo = data.Single(r => r.Name.ToLower() == role.ToLower());
            var user = data.SelectMany(r => r.Users).Single(u => u.UserName.ToLower() == username.ToLower());
            currentRole.Users.Remove(user);
            roleToAddUserTo.Users.Add(user);
            database.Set(data);

        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            database.Save(new EKRole {Name = roleName});
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            var roles = database.Get().Where(r => r.Users.Select(u=>u.UserName.ToLower()).Contains(username.ToLower()));
            if(roles.Count() == 0)
                return new string[] {};
            return new string[]
                       {
                           
                           roles.First().Name
                       };
        }

        public override string[] GetUsersInRole(string roleName)
        {
            return
                database.Get().Single(r => r.Name.ToLower() == roleName.ToLower()).Users.Select(u => u.UserName).ToArray
                    ();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            return database.Get().Any(r => String.Equals(roleName, r.Name, StringComparison.OrdinalIgnoreCase));
        }
    }
}