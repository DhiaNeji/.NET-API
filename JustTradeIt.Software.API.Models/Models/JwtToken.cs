using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTradeIt.Software.API.Models.Models
{
 public class JwtToken
    {
        [Key]
        public int Id { get; set; }
        public Byte Blacklisted { get; set; }

        public string tokenValue { get; set; }
        public JwtToken( byte blacklisted,string tokenValue)
        {
            Blacklisted = blacklisted;
            this.tokenValue = tokenValue;
        }
    }
}
