using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistSysACW.Filters
{
    public class AuthFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                AuthorizeAttribute authAttribute = (AuthorizeAttribute)context.ActionDescriptor.EndpointMetadata.Where(e => e.GetType() == typeof(AuthorizeAttribute)).FirstOrDefault();

                if (authAttribute != null)
                {
                    string[] roles = authAttribute.Roles.Split(',');
                    if (roles.Length == 1 && roles[0] == "Admin" && !context.HttpContext.User.IsInRole("Admin"))
                    {
                        throw new UnauthorizedAccessException();
                    }
                    foreach (string role in roles)
                    {
                        if (context.HttpContext.User.IsInRole(role))
                        {
                            return;
                        }
                    }
                    throw new Exception();
                }
            }
            catch (UnauthorizedAccessException)
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new JsonResult("Unauthorized. Admin access only.");
            }
            catch(Exception)
            {
                context.HttpContext.Response.StatusCode = 401;
                context.Result = new JsonResult("Unauthorized. Check ApiKey in Header is correct.");
            }
            
        }
    }
}
