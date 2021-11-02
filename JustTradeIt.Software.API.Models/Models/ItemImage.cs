using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTradeIt.Software.API.Models.Models
{
   public class ItemImage
    {   [Key]
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public virtual Item Item { get; set; } 

        public ItemImage(String imageUrl)
        {
            this.ImageUrl = ImageUrl;
        }
    }
}
