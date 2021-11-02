using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTradeIt.Software.API.Models.DTOs
{
   public class ItemDto
    {
     public string Identifier { get; set; }
     public string Title { get; set; }
     public string ShortDescription { get; set; }
     public UserDto Owner { get; set; }

    }
}
