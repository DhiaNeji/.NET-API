using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
        public ItemDto AddNewItem(string email,ItemInputModel item)
        {
            User user = this._context.User.Where(u => u.Id == 1).First();
            ItemCondition itemCondition = this._context.ItemCondition.Where(i => i.Description.Equals(item.ConditionCode.ToString())).First();
            List<String> l = item.ItemImages.ToList();
            List<ItemImage> list = new List<ItemImage>();
            
            Item newItem = new Item("identifier", item.Title, item.Description, item.ShortDescription);
            newItem.Owner = user;
            newItem.ItemCondition = itemCondition;
            for (int i = 0; i < l.Count; i++)
            {
                ItemImage img = new ItemImage(l[i]);
                this._context.ItemImage.Add(img);
                img.ImageUrl = l[i];
                img.Item = newItem;
                this._context.ItemImage.Add(img);
            }
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
            Item item = this._context.Item.Include(i=>i.Owner).Include(i=>i.ItemCondition).Include(i=>i.relatedTradeItems).Include(i=>i.ItemImages).Where(i => i.Id == int.Parse(identifier)).First(); ;
            ItemDetailsDto dto = this.mapper.Map<ItemDetailsDto>(item);
            dto.NumberOfActiveTradeRequests = item.relatedTradeItems.Count;
            return dto;
        }

        public void RemoveItem(string email, string identifier)
        {
            Item item = this._context.Item.Where(i => i.Id == int.Parse(identifier)).First();
            User user = this._context.User.Where(u => u.Email.Equals(email)).First();
            if(item.Owner.Id!=user.Id)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("This Item is not relevant to you", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.BadRequest
                };
                throw new HttpResponseException(response);
            }
            List<TradeItem> l = this._context.TradeItem.Include(tr=>tr.trade).Where(tr => tr.item.Id == item.Id).Where(tr => tr.trade.TradeStatus == Models.Enums.TradeStatus.Pending).ToList();
            if(l.Count>0)
            {
                for(int i=0;i<l.Count;i++)
                {
                    Trade trade = this._context.Trade.Include(tr=>tr.RelatedtradeItems).Where(tr => tr.Id == l[i].trade.Id).First();
                    trade.TradeStatus = Models.Enums.TradeStatus.Cancelled;
                    trade.ModifiedDate = DateTime.Now;
                    trade.ModifiedBy = user.FullName;
                    trade.RelatedtradeItems.Clear();
                    this._context.Trade.Update(trade);
                }
            }
            this._context.Item.Remove(item);
            this._context.SaveChanges();
        }
        
    }
}