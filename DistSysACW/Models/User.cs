using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DistSysACW.Controllers;


namespace DistSysACW.Models
{
    public class User
    {
        #region Task2
        // TODO: Create a User Class for use with Entity Framework
        [Key]
        public string ApiKey { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public User()
        {
            
        }
        // Note that you can use the [key] attribute to set your ApiKey Guid as the primary key 
        #endregion
    }

    #region Task13?
    // TODO: You may find it useful to add code here for Logging
    #endregion

    public static class UserDatabaseAccess
    {
        #region Task3 
        // TODO: Make methods which allow us to read from/write to the database 
        
        public static User CreateUser(string UserName, UserContext context)
        {
            int admins = 0;
            User newUser = new User();
            newUser.UserName = UserName;
            newUser.ApiKey = Guid.NewGuid().ToString();
            context.Users.Add(newUser);
            context.SaveChanges();
            foreach (User user in context.Users)
            {
                if (user.Role.Contains("Admin"))
                {
                    admins++;
                }
            }
            if (admins == 0)
            {
                SetUserRole(newUser.ApiKey, "Admin", context);
            }
            else
            {
                SetUserRole(newUser.ApiKey, "User", context);
            }
            return newUser;

        }

        public static bool CheckUserNameExists(string UserName, UserContext userContext)
        {
            User u = userContext.Users.FirstOrDefault(user => user.UserName == UserName);
            if (u == default)
            {
                return false;
            }
            return true;
        }

        public static bool CheckUserExists(string ApiKey, UserContext context)
        {
            User u = context.Users.First(user => user.ApiKey == ApiKey);
            if (u == default)
            {
                return false;
            }
            return true;
        }
        
        public static bool CheckUserExists(string ApiKey, string UserName, UserContext context)
        {
            User u = context.Users.FirstOrDefault(user => user.ApiKey == ApiKey && user.UserName == UserName);
            if (u == default)
            {
                return false;
            }
            return true;
        }

        public static bool CheckUserExists(string ApiKey, UserContext context, out User user)
        {
            user = context.Users.FirstOrDefault(u => u.ApiKey == ApiKey);
            if (user == default)
            {
                return false;
            }
            return true;
        }

        public static User GetUserFromApi(string ApiKey, UserContext context)
        {
            User user = context.Users.FirstOrDefault(u => u.ApiKey == ApiKey);
            return user;
        }

        public static void DeleteUser(string ApiKey, UserContext context)
        {
            User user = context.Users.FirstOrDefault(u => u.ApiKey == ApiKey);
            context.Users.Remove(user);
            context.SaveChanges();
        }

        public static string SetUserRole(string ApiKey, string role, UserContext context)
        {
            int admins = 0;
            User user = GetUserFromApi(ApiKey, context);
            if (!role.Contains("Admin") && user.Role.Contains("Admin"))
            {
                foreach (User u in context.Users)
                {
                    if (user.Role.Contains("Admin"))
                    {
                        admins++;
                    }
                }
                if (admins <= 1)
                {
                    return "User " + user.UserName + " role not changed as this is the last admin";
                }
            }
            user.Role = role;
            context.SaveChanges();
            return "User " + user.UserName + " role set to " + user.Role;
        }
        #endregion
    }


}