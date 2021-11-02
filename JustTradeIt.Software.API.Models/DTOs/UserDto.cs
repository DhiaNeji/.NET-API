using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JustTradeIt.Software.API.Models.DTOs
{
 public  class UserDto
    {
    public string Identifier { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string ProfileImageUrl { get; set; }
   [JsonIgnore]
   public int TokenId { get; set; }

    }
}
