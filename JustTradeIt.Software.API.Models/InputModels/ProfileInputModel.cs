using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTradeIt.Software.API.Models.InputModels
{
   public  class ProfileInputModel
    {
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Minimum length of 3")]
        public string FullName {get; set;}

        public IFormFile ProfileImage { get; set; }
    }
}
