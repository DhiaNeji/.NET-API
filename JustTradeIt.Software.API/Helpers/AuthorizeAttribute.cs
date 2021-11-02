using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


using JustTradeIt.Software.API.Models.Models;
using JustTradeIt.Software.API.Models.DTOs;

namespace JustTradeIt.Software.API.Models.Helpers
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            UserDto user = (UserDto)context.HttpContext.Items["User"];
            int blackListed= (int)context.HttpContext.Items["BlackListed"];
            if (user == null)
            {
                // not logged in
                context.Result = new JsonResult(new { Message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            if(blackListed==1)
            {
                context.Result = new JsonResult(new { Message = "User BlackListed" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
