using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTradeIt.Software.API.Models.Models
{
  public  class ItemCondition
    {
        [Key]
        public int Id { get; set; }

        public string ConditionCode { get; set; }

        public string Description { get; set; }

        public ItemCondition(int id, string conditionCode, string description)
        {
            Id = id;
            ConditionCode = conditionCode;
            Description = description;
        }
    }
}
