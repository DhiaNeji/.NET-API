using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JustTradeIt.Software.API.Models.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public string PublicIdentifier { get; set; }
        public string Tiltle { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }

        public virtual ItemCondition ItemCondition { get; set; }
        public virtual User Owner { get; set; }

        public ICollection<ItemImage> ItemImages { get; set; }

        [NotMapped]
        public IList<Trade> relatedTrades { get; set; }

        public Item(string publicIdentifier, string tiltle, string description, 
            string shortDescription)
        {
            PublicIdentifier = publicIdentifier;
            Tiltle = tiltle;
            Description = description;
            ShortDescription = shortDescription;
        }

        public Item(int id, string publicIdentifier, string tiltle, string description, 
            string shortDescription, ItemCondition itemCondition, User owner, ICollection<ItemImage> itemImages)
        {
            Id = id;
            PublicIdentifier = publicIdentifier;
            Tiltle = tiltle;
            Description = description;
            ShortDescription = shortDescription;
            ItemCondition = itemCondition;
            Owner = owner;
            ItemImages = itemImages;
        }
    }
}