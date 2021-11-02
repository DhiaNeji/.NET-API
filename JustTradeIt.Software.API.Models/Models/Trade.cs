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
        [Key]
        public int Id { get; set; }

        public String PublicIdentifier { get; set; }

        public DateTime IssueDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public String ModifiedBy { get; set; }

        public TradeStatus TradeStatus { get; set; }
        [ForeignKey("ReceiverId")]
        public int ReceiverId { get; set; }
        public virtual User Receiver { get; set; }
        [ForeignKey("SenderId")]
        public int SenderId { get; set; }
        public virtual User Sender { get; set; }

        public IList<Item> SendingtradeItems { get; set; }

        public IList<Item> ReceivingtradeItems { get; set; }

        public Trade(string publicIdentifier, DateTime issueDate, DateTime modifiedDate, string modifiedBy, 
            TradeStatus tradeStatus)
        {
            PublicIdentifier = publicIdentifier;
            IssueDate = issueDate;
            ModifiedDate = modifiedDate;
            ModifiedBy = modifiedBy;
            TradeStatus = tradeStatus;
        }
    }
}
