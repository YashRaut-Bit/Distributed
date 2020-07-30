using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DistSysACW.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace DistSysACW.Controllers
{
    [Route("api/user")]
    public class UserController : BaseController
    {
        /// <summary>
        /// Constructs a TalkBack controller, taking the UserContext through dependency injection
        /// </summary>
        /// <param name="context">DbContext set as a service in Startup.cs and dependency injected</param>
        public UserController(Models.UserContext context) : base(context) { }

        [HttpGet("New")]     //user/new?username=UserOne
        public IActionResult Get([FromQuery] string username)
        {
            #region TASK4

            string view = "False - User Does Not Exist! Did you mean to do a POST to create a new user?";
            if (UserDatabaseAccess.CheckUserNameExists(username, _context))
            {
                view = "True - User Does Exist! Did you mean to do a POST to create a new user?";
            }
            return Content(view);
            #endregion
        }

        [HttpPost("New")]    //user/new/UserOne
        public IActionResult Post([FromBody] CreateUserName name)
        {
            #region TASK4
            string view;
            if (string.IsNullOrWhiteSpace(name.Username))
            {
                view = "Oops. Make sure your body contains a string with your username and your Content-Type is Content - Type:application / json";
                return BadRequest(view);
            }
            if (UserDatabaseAccess.CheckUserNameExists(name.Username, _context))
            {
                view = "Oops. This username is already in use. Please try again with a new username.";
                return Forbid(view);
            }
            User newUser = UserDatabaseAccess.CreateUser(name.Username, _context);
            view = newUser.ApiKey;
            return Content(view);
            #endregion
        }

        [HttpDelete("Removeuser")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult Delete([FromQuery] string username, [FromHeader] string ApiKey)
        {
            bool view = false;
            if (UserDatabaseAccess.CheckUserExists(ApiKey, _context))
            {
                if (UserDatabaseAccess.GetUserFromApi(ApiKey,_context).UserName == username)
                {
                    UserDatabaseAccess.DeleteUser(ApiKey, _context);
                    view = true;
                }
            }
            return Ok(view);
        }
        [HttpPut("ChangeRole")]
        [Authorize(Roles = "Admin")]
        public IActionResult ChangeRole([FromHeader] string Apikey, [FromBody] ChangeRoleUser changeRoleUser)
        {
            try
            {
                if (!UserDatabaseAccess.CheckUserNameExists(changeRoleUser.Username, _context))
                {
                    return BadRequest("NOT DONE: Username does not exist");
                }
                if (changeRoleUser.Role != "User" || changeRoleUser.Role != "Admin")
                {
                    return BadRequest("NOT DONE: Role does not exist");
                }
                UserDatabaseAccess.SetUserRole(UserDatabaseAccess.GetUserFromUsername(changeRoleUser.Username, _context).ApiKey, changeRoleUser.Role, _context);
                return Ok("DONE");
            }
            catch (Exception)
            {
                return BadRequest("NOT DONE: An error occured");
            }
            
        }
    }
    public class CreateUserName
    {
        public string Username { get; set; }
    }
    public class ChangeRoleUser
    {
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
