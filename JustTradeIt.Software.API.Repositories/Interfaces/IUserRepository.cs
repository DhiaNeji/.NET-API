using JustTradeIt.Software.API.Models.DTOs;
using JustTradeIt.Software.API.Models.InputModels;
using JustTradeIt.Software.API.Models.Models;

namespace JustTradeIt.Software.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        UserDto CreateUser(RegisterInputModel inputModel);
        UserDto AuthenticateUser(LoginInputModel loginInputModel);
        UserDto UpdateProfile(string FullName,string email,string imgUrl);
        UserDto GetProfileInformation(string email);
        UserDto GetUserInformation(string userIdentifier);
        public User getUserByEmail(string email);
        public User getUserToNotifyByTrade(string email, int tradeId);

    }
}