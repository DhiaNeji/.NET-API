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
        [Key]
        [Column(Order = 1)]
        public int TradeId { get; set; }
        [NotMapped]
        public Trade trade;
        [Key]
        [Column(Order = 2)]
        public int ItemId { get; set; }
        [NotMapped]
        public Item item;
        [Key]
        [Column(Order = 3)]
        public int UserId { get; set; }
        [NotMapped]
        public User user;

        public TradeItem(Trade trade, Item item, User user)
        {
            this.trade = trade;
            this.item = item;
            this.user = user;
        }
        public TradeItem()
        {

        }
        public TradeItem(int tradeId,int userId,int itemId)
        {
            this.ItemId = itemId;
            this.TradeId = tradeId;
            this.UserId = userId;
        }
    }
}
