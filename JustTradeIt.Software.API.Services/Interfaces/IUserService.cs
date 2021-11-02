using JustTradeIt.Software.API.Models.DTOs;
using JustTradeIt.Software.API.Models.InputModels;
using JustTradeIt.Software.API.Models.Models;
using JustTradeIt.Software.API.Models.Responses;
using System.Collections.Generic;

namespace JustTradeIt.Software.API.Services.Interfaces
{
    public interface IUserService
    {
        public IEnumerable<TradeDto> GetUserTrades(string userIdentifier);
        public UserDto GetUserInformation(string identifier);
        
    }
}