using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTradeIt.Software.API.Models.Enums
{
   public  enum TradeStatus
    {
        Pending,
        Accepted,
        Declined,
        Cancelled,
        TimedOut
    }
}
