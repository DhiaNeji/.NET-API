using JustTradeIt.Software.API.Models.Models;

namespace JustTradeIt.Software.API.Repositories.Interfaces
{
    public interface ITokenRepository
    {
        JwtToken CreateNewToken(string email,string token);
        bool IsTokenBlacklisted(string tokenValue);
        void VoidToken(int tokenId);
    }
}