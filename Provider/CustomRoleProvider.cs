using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Helpers;
using System.Security.Cryptography;
using System.Web.WebPages;
using Microsoft.Internal.Web.Utils;
using RegStaff.Models;

namespace RegStaff.Provider
{
    public class CustomRoleProvider: RoleProvider
    {
        public string GetOneRoleForUser (string UserName)
        {
            try
            {
                if (string.IsNullOrEmpty(UserName))
                {
                    return default(string);
                }
                else
                { 
                    string[] roles = GetRolesForUser(UserName);
                    return roles[0].ToString();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public override string[] GetRolesForUser(string UserName)
        {
            string[] role = new string[] {};
            using (UsersContext _db = new UsersContext())
            {
                try
                {
                    // Получаем пользователя
                    var user = (from u in _db.UserProfiles
                                 where u.UserName == UserName
                                 select u).FirstOrDefault();
                    if (user != null)
                    {
                        // получаем роль
                        webpages_Roles userRole = _db.webpages_Roles.Find(user.UserId);

                        if (userRole != null)
                        {
                            role = new string []{userRole.RoleName};
                        }
                    }
                }
                catch
                {
                    role = new string[]{};
                }
            }
            return role;
        }
        public override void CreateRole(string roleName)
        {
            webpages_Roles newRole = new webpages_Roles() { RoleName = roleName };
            UsersContext db = new UsersContext();
            db.webpages_Roles.Add(newRole);
            db.SaveChanges();
        }
        public override bool IsUserInRole(string username, string roleName)
        {
            bool outputResult = false;
            
            using (UsersContext _db = new UsersContext())
            {
                try
                {                    
                    var user = (from u in _db.UserProfiles
                                 where u.UserName == username
                                 select u).FirstOrDefault();
                    if (user != null)
                    {
                        
                        webpages_UserinRoles userRole = _db.webpages_UserinRoles.Find(user.UserId);
                        webpages_Roles roles = _db.webpages_Roles.Find(userRole.RoleId);

                        if (userRole != null && roles.RoleName == roleName)
                        {
                            outputResult = true;
                        }
                    }
                }
                catch
                {
                    outputResult = false;
                }
            }
            return outputResult;
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
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

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}