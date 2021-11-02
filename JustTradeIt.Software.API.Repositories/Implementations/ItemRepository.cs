using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using JustTradeIt.Software.API.Models;
using JustTradeIt.Software.API.Models.DTOs;
using JustTradeIt.Software.API.Models.InputModels;
using JustTradeIt.Software.API.Models.Models;
using JustTradeIt.Software.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JustTradeIt.Software.API.Repositories.Implementations
{
    public class ItemRepository : IItemRepository
    {
        JustTradeItContext _context;

        private IMapper mapper;
        public ItemRepository(JustTradeItContext context, IMapper mapper)  
        {
            this._context = context;
            this.mapper = mapper;
        }
        public ItemDto AddNewItem(ItemInputModel item)
        {
            //Get the authenticated User
            User user = this._context.User.Where(u => u.Id == 1).First();
            System.Diagnostics.Debug.WriteLine(item.ConditionCode.ToString());
            ItemCondition itemCondition = this._context.ItemCondition.Where(i => i.Description.Equals(item.ConditionCode.ToString())).First();
            List<String> l = item.ItemImages.ToList();
            List<ItemImage> list = new List<ItemImage>();
            for(int i=0;i<l.Count();i++)
            {
                ItemImage img = new ItemImage(l[i]);
            }
            Item newItem = new Item("1", item.Title, item.Description, item.ShortDescription);
            newItem.Owner = user;
            newItem.ItemCondition = itemCondition;
            newItem.ItemImages = list;
            this._context.Item.Add(newItem);
            this._context.SaveChanges();
            return this.mapper.Map<ItemDto>(newItem);
        }

        public Envelope<ItemDto> GetAllItems(int pageSize, int pageNumber, bool ascendingSortOrder)
        {
            int skip = (pageNumber - 1) * pageSize;
            int total = this._context.Item.Count();
            List<Item> l;
            if (ascendingSortOrder)
                l = this._context.Item.Include(i=>i.Owner).OrderBy(c => c.Id).Skip(skip).Take(pageSize).ToList().ToList();
            else
                l = this._context.Item.Include(i => i.Owner).OrderByDescending(c => c.Id).Skip(skip).Take(pageSize).ToList().ToList();
            decimal maxPages = (decimal)((float)total /(float) pageSize);
            maxPages = Math.Ceiling(maxPages);
            List<ItemDto> list = new List<ItemDto>();
            for(int i=0;i<l.Count();i++)
            {
                list.Add(this.mapper.Map<ItemDto>(l[i]));
            }
            return new Envelope<ItemDto>(pageNumber,list.Count(),Decimal.ToInt32(maxPages),list);
        }

        public ItemDetailsDto GetItemByIdentifier(string identifier)
        {
            Item item = this._context.Item.Include(i=>i.Owner).Include(i=>i.ItemCondition).Where(i => i.Id == int.Parse(identifier)).First(); ;
            return this.mapper.Map<ItemDetailsDto>(item);
        }

        public void RemoveItem(string email, string identifier)
        {
            //To be implemented also and associate it with the authentified user
            Item item = this._context.Item.Where(i => i.Id == int.Parse(identifier)).First();
            this._context.Item.Remove(item);
            this._context.SaveChanges();
        }
        
    }
}