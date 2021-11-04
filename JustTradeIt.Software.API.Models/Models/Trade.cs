using JustTradeIt.Software.API.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTradeIt.Software.API.Models.Models
{
    public class Trade
    {
        private string v;
        private DateTime now1;
        private DateTime now2;
        private string fullName;
        private TradeStatus pending;

        public Trade()
        {
        }

        [Key]
        public int Id { get; set; }

        public String PublicIdentifier { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public String ModifiedBy { get; set; }

        public TradeStatus TradeStatus { get; set; }
        public int ReceiverId { get; set; }
        public User Receiver { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        
        public ICollection<TradeItem> RelatedtradeItems { get; set; }

        public Trade(string publicIdentifier, DateTime issueDate, DateTime modifiedDate, string modifiedBy, TradeStatus tradeStatus, User receiver, User sender)
        {
            PublicIdentifier = publicIdentifier;
            IssueDate = issueDate;
            ModifiedDate = modifiedDate;
            ModifiedBy = modifiedBy;
            TradeStatus = tradeStatus;
            Receiver = receiver;
            Sender = sender;
        }
    }
}
