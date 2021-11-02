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

namespace JustTradeIt.Software.API.Controllers
{
    
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private AccountService _accoutService;

        private TokenService _tokenService;

        private IUserRepository _IUserRepository;
        public AccountController(AccountService _accoutService, IUserRepository _IUserRepository,TokenService tokenService)
        {
            this._accoutService = _accoutService;
            this._IUserRepository = _IUserRepository;
            this._tokenService = tokenService;
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
            return this._accoutService.GetProfileInformation();
        }

        [HttpPut("profile")]
        public UserDto updateUser(ProfileInputModel profileInputModel)
        {
            return this._accoutService.UpdateProfile(profileInputModel);
        }

        [HttpGet("createToken")]
        public void createToken()
        {
            var token = JwtBuilder.Create()
                      .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                      .WithSecret("GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk")
                      .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
                      .AddClaim("claim2", "claim2-value")
                      .Encode();

            System.Diagnostics.Debug.WriteLine(token);
        }

        [HttpGet("validate/{token}")]
        public void createToken(string token)
        {
            const string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                var json = decoder.Decode(token, secret, verify: true);
                Console.WriteLine(json);
            }
            catch (TokenExpiredException)
            {
                Console.WriteLine("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                Console.WriteLine("Token has invalid signature");
            }

            System.Diagnostics.Debug.WriteLine(token);
        }
    }
}
