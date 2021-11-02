using JustTradeIt.Software.API.Models.Models;
using JustTradeIt.Software.API.Repositories.Interfaces;
using System.Linq;

namespace JustTradeIt.Software.API.Repositories.Implementations
{
    public class TokenRepository : ITokenRepository
    {
        private JustTradeItContext _context;

        public TokenRepository(JustTradeItContext context)
        {
            this._context = context;
        }
        public JwtToken CreateNewToken(string token)
        {
            //The Token will be created when adding the JWT
            JwtToken jwt = new JwtToken(0,token);
            this._context.JwtToken.Add(jwt);
            this._context.SaveChanges();
            return jwt;
        }

        public bool IsTokenBlacklisted(string tokenValue)
        {
            JwtToken jwt =this._context.JwtToken.Where(j => j.tokenValue.Equals(tokenValue)).First();
            return jwt.Blacklisted == 1;
        }

        public void VoidToken(int tokenId)
        {
            JwtToken jwt = (JwtToken)this._context.JwtToken.Where(j => j.Id == tokenId);
            jwt.Blacklisted = 1;
            this._context.Update(jwt);
            this._context.SaveChanges();
        }
    }
}