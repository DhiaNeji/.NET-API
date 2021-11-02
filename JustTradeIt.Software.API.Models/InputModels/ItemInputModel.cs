using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTradeIt.Software.API.Models.InputModels
{
   public enum Condition
    {
        GOOD, MINT, USED, BAD,
DAMAGED
    }
  public   class ItemInputModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public Condition ConditionCode { get; set; }
        [Required]
        public IEnumerable<string>  ItemImages { get; set; }

    }
}
