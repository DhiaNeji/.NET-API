using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTradeIt.Software.API.Models.InputModels
{
  public  class RegisterInputModel
    {
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Should be a valid email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Minimum length of 3")]
        public string FullName { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Minimum 8 characters required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Minimum 8 characters required")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [NotMapped]
        public string PasswordConfirmation { get; set; }
    }
}
