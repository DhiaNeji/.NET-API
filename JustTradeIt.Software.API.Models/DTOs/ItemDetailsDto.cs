using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTradeIt.Software.API.Models.DTOs
{
    public class ItemDetailsDto
    {
     public string Identifier { get; set; }
     public string Title { get; set; }
     public string Description { get; set; }
     public IEnumerable<ImageDto>  Images { get; set; }
     public int NumberOfActiveTradeRequests { get; set; }
     public string Condition { get; set; }
     public UserDto Owner { get; set; }

    }
}
