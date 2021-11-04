using JustTradeIt.Software.API.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTradeIt.Software.API.Models.DTOs
{

   public  class TradeDetailsDto
    {

      public IEnumerable<ItemDto> ReceivingItems { get; set; }
      public IEnumerable<ItemDto>  OfferingItems { get; set; }
      public UserDto Receiver { get; set; }
      public UserDto Sender { get; set; }
      public DateTime? ReceivedDate { get; set; }

      public string Identifier { get; set; }
      public DateTime IssuedDate { get; set; }
      public DateTime ModifiedDate { get; set; }
      public string ModifiedBy { get; set; }
      public TradeStatus Status { get; set; }

        public TradeDetailsDto(IEnumerable<ItemDto> receivingItems, IEnumerable<ItemDto> offeringItems, UserDto receiver, UserDto sender, DateTime? receivedDate, string identifier, DateTime issuedDate, DateTime modifiedDate, string modifiedBy, TradeStatus status)
        {
            ReceivingItems = receivingItems;
            OfferingItems = offeringItems;
            Receiver = receiver;
            Sender = sender;
            ReceivedDate = receivedDate;
            Identifier = identifier;
            IssuedDate = issuedDate;
            ModifiedDate = modifiedDate;
            ModifiedBy = modifiedBy;
            Status = status;
        }
    }
}
