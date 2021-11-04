using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using JustTradeIt.Software.API.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using JWT;
using JWT.Exceptions;
using JWT.Algorithms;
using JWT.Serializers;
using JustTradeIt.Software.API.Services.Implementations;
using System.Security.Claims;
using JustTradeIt.Software.API.Models.Models;
using System.Net.Http;
using System.Net;
using System.Web.Http;

namespace JustTradeIt.Software.API.Models.Helpers
{
    class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context, IUserService userService,ITokenService tokenService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
                attachUserToContext(context,token,userService,tokenService);

            await _next(context);
        }

        private void attachUserToContext(HttpContext context, string token, IUserService userService, ITokenService tokenService)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(c => c.Type == "id").Value);
                context.Items["User"] = userService.GetUserInformation(userId.ToString());

                if (tokenService.isTokenBlackListed(token))
                {
                    context.Items["BlackListed"] = 1;
                }
                else
                    context.Items["BlackListed"] = 0;
            }
            catch
            {

            }

        }
    }
}
