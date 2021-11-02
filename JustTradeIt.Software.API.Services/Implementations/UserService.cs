using System.Collections.Generic;
using JustTradeIt.Software.API.Models.DTOs;
using JustTradeIt.Software.API.Models.InputModels;
using JustTradeIt.Software.API.Models.Models;
using JustTradeIt.Software.API.Models.Responses;
using JustTradeIt.Software.API.Repositories.Interfaces;
using JustTradeIt.Software.API.Services.Interfaces;

namespace JustTradeIt.Software.API.Services.Implementations
{
    public class UserService : IUserService
    {

        private IUserRepository userRepository;

        private ITradeRepository tradeRepository;
        
        public UserService(IUserRepository userRepository,ITradeRepository tradeRepository)
        {
            this.tradeRepository = tradeRepository;
            this.userRepository = userRepository;
        }
        public UserDto GetUserInformation(string identifier)
        {
            return this.userRepository.GetUserInformation(identifier);
        }

        public IEnumerable<TradeDto> GetUserTrades(string userIdentifier)
        {
            return this.tradeRepository.GetUserTrades(userIdentifier);
        }
        
    }
}