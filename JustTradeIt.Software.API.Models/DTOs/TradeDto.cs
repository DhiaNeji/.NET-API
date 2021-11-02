using JustTradeIt.Software.API.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTradeIt.Software.API.Models.DTOs
{
  
   public  class TradeDto
    {
     public string Identifier { get; set; }
     public DateTime IssuedDate { get; set; }
     public DateTime ModifiedDate { get;set; }
     public string ModifiedBy { get; set; }
     public    TradeStatus Status { get; set; }


    }
}

