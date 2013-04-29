using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using EKContent.web.Models.Database.Concrete;
using EKContent.web.Models.Entities;

namespace EKContent.web.Models.Database.DotNetMembership
{
    public class EKDotNetMembershipProvider : System.Web.Security.MembershipProvider
    {
        private EKRoleProvider database = new EKRoleProvider();
   
        public override string ApplicationName
        {
            get
            {
                return "EKContent";
            }
            set
            {
                ;
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var roles = database.Get();
            var role = (from r in roles
                       let _role = r
                       from u in r.Users
                       where u.UserName == username && u.Password == oldPassword
                       select _role).Single();

            var user = role.Users.Single(u => u.UserName == username && u.Password == oldPassword);
            user.Password = newPassword;
            database.Save(role);
            return true;
            
            
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out System.Web.Security.MembershipCreateStatus status)
        {
            var roles = database.Get();
            var emptyRole = roles.Single(r => r.Name == "-");
            emptyRole.Users.Add(new EKUser {UserName = username, Password = password});
            database.Save(emptyRole);
            status = MembershipCreateStatus.Success;
            return GetUser(username, false);
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override System.Web.Security.MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUser GetUser(string username, bool userIsOnline)
        {
            var user =
                database.Get().SelectMany(m => m.Users).SingleOrDefault(
                    u => String.Equals(username, u.UserName, StringComparison.InvariantCultureIgnoreCase));
            if (user == null)
                return null;
            return new MembershipUser(Membership.Provider.Name, user.UserName, user.Id, user.UserName, null, null, true, false, DateTime.Now,
                                      DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
        }

        public override System.Web.Security.MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override System.Web.Security.MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(System.Web.Security.MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            return database.Get().SelectMany(m => m.Users).Any(
                    u => String.Equals(username, u.UserName, StringComparison.InvariantCultureIgnoreCase) && String.Equals(password, u.Password, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}