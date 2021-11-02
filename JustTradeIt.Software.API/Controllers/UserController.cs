using JustTradeIt.Software.API.Models.DTOs;
using JustTradeIt.Software.API.Repositories.Interfaces;
using JustTradeIt.Software.API.Services.Implementations;
using JustTradeIt.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace JustTradeIt.Software.API.Controllers
{
    [Route("api/users")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private UserService _userService;

        private IUserRepository _IUserRepository;

        public UserController(UserService userService,IUserRepository userRepository)
        {
            this._userService = userService;
            this._IUserRepository = userRepository;
        }

        [HttpGet("{identifier}")]
        public UserDto getUserProfile(string identifier)
        {
            return this._userService.GetUserInformation(identifier);
        }

        [HttpGet("{identifier}/trades")]
        public IEnumerable<TradeDto> getUserTrades(string identifier)
        {
            return this._userService.GetUserTrades(identifier);
        }
    }
}