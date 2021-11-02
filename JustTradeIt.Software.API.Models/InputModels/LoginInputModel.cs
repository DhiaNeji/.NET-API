using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTradeIt.Software.API.Models.InputModels
{
  public  class LoginInputModel
    {
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Should be a valid email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3,ErrorMessage = "Minimum 3 characters required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
