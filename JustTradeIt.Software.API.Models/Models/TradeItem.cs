using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTradeIt.Software.API.Models.Models
{
   public class TradeItem
    {
        public int tradeId { get; set; }
        public virtual Trade trade { get; set; }

        public int userID;
        public virtual User user { get; set; }
        public int itemId { get; set; }
        public virtual Item item{ get; set; }

    }
}
