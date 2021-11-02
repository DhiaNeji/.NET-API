using JustTradeIt.Software.API.Models.DTOs;

namespace JustTradeIt.Software.API.Services.Interfaces
{
    public interface ITokenService
    {
         string GenerateJwtToken(UserDto user);

        bool isTokenBlackListed(string tokenValue);
    }
}