using DistSysACW.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DistSysACW.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, Models.UserContext dbContext)
        {
            #region Task5
            // TODO:  Find if a header ‘ApiKey’ exists, and if it does, check the database to determine if the given API Key is valid
            string ApiKey = null;
            var header = context.Request.Headers;
            Claim[] claims = null;
            
            
            if (header.ContainsKey("ApiKey"))
            {
                StringValues headerValues;
                if (header.TryGetValue("ApiKey", out headerValues))
                {
                    ApiKey = headerValues.First();
                }

                if (UserDatabaseAccess.CheckUserExists(ApiKey, dbContext))
                {
                    User u = UserDatabaseAccess.GetUserFromApi(ApiKey, dbContext);
                    Claim userName = new Claim(u.UserName, ClaimTypes.Name);
                    Claim role = new Claim(u.Role, ClaimTypes.Role);
                    claims[0] = userName;
                    claims[1] = role;
                }
            }
            
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, ApiKey);
            //        Then set the correct roles for the User, using claims
            context.User.AddIdentity(claimsIdentity);
            #endregion

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }

    }
}
