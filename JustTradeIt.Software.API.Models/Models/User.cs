using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JustTradeIt.Software.API.Models.Models

{
    public class User
    {
        [Key]

        public int Id { get; set; }
        public string PublicIdentifier { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ProfileImageUrl { get; set; }
        public string hashedPassword { get; set; }

        public IList<Item> userItems {get;set;}

        public IList<Trade> userTrades { get; set; }
        public IList<Trade> userTrade { get; set; }
        public IList<TradeItem> tradeItems { get; set; }
    public User( string PublicIdentifier,string FullName,string Email,string profileImageUrl,string hashedPassword)
        {
            this.PublicIdentifier = PublicIdentifier;
            this.FullName = FullName;
            this.Email = Email;
            this.ProfileImageUrl = profileImageUrl;
            this.hashedPassword = hashedPassword;
        }
}

}