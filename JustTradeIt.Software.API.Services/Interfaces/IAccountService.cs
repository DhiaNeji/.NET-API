using JustTradeIt.Software.API.Models.DTOs;
using JustTradeIt.Software.API.Models.InputModels;
using JustTradeIt.Software.API.Models.Models;
using System.Threading.Tasks;

namespace JustTradeIt.Software.API.Services.Interfaces
{
    public interface IAccountRepository
    {
        UserDto CreateUser(RegisterInputModel inputModel);
        UserDto AuthenticateUser(LoginInputModel loginInputModel);
        void Logout(int tokenId);
        UserDto UpdateProfile(ProfileInputModel profile);
        UserDto GetProfileInformation();
    }
}