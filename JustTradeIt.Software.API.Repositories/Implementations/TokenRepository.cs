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
        public JwtToken CreateNewToken(string email,string token)
        {
            JwtToken jwt = new JwtToken(0,token);
            this._context.JwtToken.Add(jwt);
            User user=this._context.User.Where(u => u.Email == email).First();
            user.JwtToken = jwt;
            this._context.User.Update(user);
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
            JwtToken jwt =this._context.JwtToken.Where(j => j.Id == tokenId).First();
            jwt.Blacklisted = 1;
            this._context.Update(jwt);
            this._context.SaveChanges();
        }
    }
}