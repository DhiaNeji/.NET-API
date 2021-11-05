using JustTradeIt.Software.API.Models.DTOs;
using JustTradeIt.Software.API.Models.InputModels;
using JustTradeIt.Software.API.Models.Models;
using JustTradeIt.Software.API.Repositories.Interfaces;
using JustTradeIt.Software.API.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using JWT.Algorithms;
using System;
using JWT.Builder;
using JWT;
using JWT.Serializers;
using JWT.Exceptions;
using JustTradeIt.Software.API.Models.Helpers;
using RabbitMQ.Client;
using System.Net.Http;
using System.Net;
using System.IO;
using Microsoft.Extensions.Primitives;

namespace JustTradeIt.Software.API.Controllers
{
    
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private AccountService _accoutService;

        private TokenService _tokenService;

        private ImageService imageService;

        private IUserRepository _IUserRepository;
        public AccountController(AccountService _accoutService, IUserRepository _IUserRepository,TokenService tokenService,ImageService imageService)
        {
            this._accoutService = _accoutService;
            this._IUserRepository = _IUserRepository;
            this._tokenService = tokenService;
            this.imageService = imageService;
        }
       
        [HttpPost("register")]
        public UserDto createUser(RegisterInputModel inputModel)
        {
            return this._accoutService.CreateUser(inputModel);
        }

        [HttpPost("login")]
        public UserDto Login(LoginInputModel inputModel)
        {
            return this._accoutService.AuthenticateUser(inputModel);
        }
        [Authorize]
        [HttpGet("profile")]
        public UserDto getUser()
        {
          UserDto user = (UserDto)HttpContext.Items["User"];
          return this._accoutService.GetProfileInformation(user.Email);
        }

        [Authorize]
        [HttpPut("profile")]
        public UserDto updateUser()
        {
            var filePath = Path.GetTempFileName();
            StringValues FullName="";
            HttpContext.Request.Form.TryGetValue("FullName",out FullName);
            if (filePath.Length < 0)
            {
                return null;
            }
            UserDto user = (UserDto)HttpContext.Items["User"];
            string imgUrl =this.imageService.UploadImageToBucket(user.Email, HttpContext.Request.Form.Files[0]).Result;
            
            return this._accoutService.UpdateProfile(FullName,user.Email,imgUrl);
        }

        [Authorize]
        [HttpGet("logout")]
        public void logout()
        {
            int TokenId = (int)HttpContext.Items["TokenId"];
            this._accoutService.Logout(TokenId);
        }
    }
}
