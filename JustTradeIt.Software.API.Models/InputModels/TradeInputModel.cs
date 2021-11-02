using JustTradeIt.Software.API.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTradeIt.Software.API.Models.InputModels
{
   public  class TradeInputModel
    {
        [Required]
        public string ReceiverIdentifier { get; set; }

        [Required]
        public IEnumerable<ItemDto> ItemsInTrade { get; set; }
    }
}
