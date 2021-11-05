using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using JustTradeIt.Software.API.Models.DTOs;
using JustTradeIt.Software.API.Models.Enums;
using JustTradeIt.Software.API.Models.InputModels;
using JustTradeIt.Software.API.Models.Models;
using JustTradeIt.Software.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JustTradeIt.Software.API.Repositories.Implementations
{
    public class TradeRepository : ITradeRepository
    {
        private JustTradeItContext _context;

        private IMapper mapper;
        public TradeRepository(JustTradeItContext context,IMapper mapper)
        {
            this._context = context;
            this.mapper = mapper;
        }
        public string CreateTradeRequest(string email, TradeInputModel trade)
        {
            List<ItemDto> itemsToTrade = trade.ItemsInTrade.ToList();
            User sender = this._context.User.Where(u => u.Email == email).First();
            User receiver = this._context.User.Where(u => u.Id == int.Parse(trade.ReceiverIdentifier)).First();
            bool itemSenderExist = false;
            bool itemReceiverExist = false;
            bool itemStranger = true;
            List<Item> list = new List<Item>();
            List<TradeItem> list2 = new List<TradeItem>();
            Trade t=null;
            t = new Trade("id", DateTime.Now, DateTime.Now, sender.FullName, TradeStatus.Pending, receiver, sender);

            for (int i=0;i<itemsToTrade.Count();i++)
            {
                if (int.Parse(itemsToTrade[i].Owner.Identifier)==sender.Id )
                {
                    string itemToTradeId = itemsToTrade[i].Identifier;
                    Item it = this._context.Item.Where(i => i.Id == int.Parse(itemToTradeId)).First();
                    TradeItem tr = new TradeItem(t, it, sender);
                    it.setRelatedTradeItems(tr);
                    this._context.Item.Update(it);
                    itemSenderExist = true;
                }
                else
                {
                    if(int.Parse(itemsToTrade[i].Owner.Identifier) == receiver.Id)
                    {
                        string itemToTradeId = itemsToTrade[i].Identifier;
                        Item it = this._context.Item.Where(i => i.Id == int.Parse(itemToTradeId)).First();
                        TradeItem tr = new TradeItem(t, it, receiver);
                        it.setRelatedTradeItems(tr);
                        this._context.Item.Update(it);
                        itemReceiverExist = true;
                    }
                    else
                    {
                        itemStranger = false;
                    }
                }
            }
            if(itemReceiverExist && itemReceiverExist &&itemStranger)
            {
                this._context.SaveChanges();
            }
            else
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Please Verify The trading Items", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.BadRequest
                };
                throw new HttpResponseException(response);
            }
            return t.Id.ToString();
        }

        public TradeDetailsDto GetTradeByIdentifier(string identifier)
        {
            Trade t = this._context.Trade.Include(t=>t.Sender).Include(t=>t.Receiver).Include(t=>t.RelatedtradeItems).Include(t=>t.RelatedtradeItems).Where(t => t.Id == int.Parse(identifier)).First();
            List<TradeItem> trr = this._context.TradeItem.Include(tr=>tr.item).Where(tr => tr.TradeId == t.Id).ToList();
            ICollection<TradeItem> tr = t.RelatedtradeItems;
            ICollection<ItemDto> sentItems = new Collection<ItemDto>();
            ICollection<ItemDto> receivedItems = new Collection<ItemDto>();
            List<TradeItem> l=tr.ToList();
            for(int i=0;i<l.Count;i++)
            {
                if (t.Sender.Id == l[i].item.Owner.Id)
                    sentItems.Add(this.mapper.Map<ItemDto>(l[i].item));
                else
                    receivedItems.Add(this.mapper.Map<ItemDto>(l[i].item));
            }
            TradeDetailsDto tradeDetailDto = new TradeDetailsDto(receivedItems,sentItems,this.mapper.Map<UserDto>(t.Receiver),this.mapper.Map<UserDto>(t.Sender), t.IssueDate,t.Id.ToString(),t.IssueDate,t.ModifiedDate,t.ModifiedBy,t.TradeStatus);
            return tradeDetailDto;
        }

        public IEnumerable<TradeDto> GetTradeRequests(string email, bool onlyIncludeActive)
        {
            User user = this._context.User.Where(u => u.Email.Equals(email)).First();
            List<TradeItem> l = this._context.TradeItem.Include(t=>t.trade).Where(u => u.user.Id == user.Id).ToList();
            if (onlyIncludeActive)
            {
                l = l.Where(t => t.trade.TradeStatus != Models.Enums.TradeStatus.Accepted).ToList();
            }
            List<TradeDto> list = new List<TradeDto>();
            for(int i=0;i<l.Count();i++)
            {
                list.Add(this.mapper.Map<TradeDto>(l[i].trade));
            }
            return list;
        }

        public IEnumerable<TradeDto> GetTrades(string email)
        {
            User user = this._context.User.Where(u => u.Email == email).First();
            Trade t = this._context.Trade.Where(t => t.Sender == user).First();
            List<Trade> trades = this._context.Trade.Where(t => t.Sender == user).Where(t => t.TradeStatus == Models.Enums.TradeStatus.Accepted).ToList();
            List<TradeDto> list = new List<TradeDto>();
            for (int i = 0; i < trades.Count(); i++)
            {
                list.Add(this.mapper.Map<TradeDto>(trades[i]));
            }
            return list;
        }

        public IEnumerable<TradeDto> GetUserTrades(string userIdentifier)
        {
            User user = this._context.User.Where(u => u.Id == int.Parse(userIdentifier)).First();
            List<Trade> l = this._context.Trade.Where(t => t.Sender == user || t.Receiver == user).Where(t => t.TradeStatus == Models.Enums.TradeStatus.Accepted).ToList();
            List<TradeDto> list = new List<TradeDto>();
            for(int i=0;i<l.Count();i++)
            {
                list.Add(this.mapper.Map<TradeDto>(l[i]));
            }
            return list;
        }

        public String UpdateTradeRequest(string identifier, string email, Models.Enums.TradeStatus newStatus)
        {
            Trade trade= this._context.Trade.Include(t=>t.Sender).Include(t=>t.RelatedtradeItems).Include(t => t.Receiver).Where(u => u.Id == int.Parse(identifier)).First();
            User user = this._context.User.Where(u => u.Email == email).First();
            if (trade.TradeStatus==Models.Enums.TradeStatus.Pending)
            {
                if (trade.Sender == user)
                {
                    if (newStatus == Models.Enums.TradeStatus.Cancelled)
                    {
                        trade.TradeStatus = newStatus;
                        trade.ModifiedDate = DateTime.Now;
                        trade.ModifiedBy = user.FullName;
                        trade.RelatedtradeItems.Clear();
                    }
                    else
                    {
                        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent("As a sender, you can only cancel a trade request", System.Text.Encoding.UTF8, "text/plain"),
                            StatusCode = HttpStatusCode.BadRequest
                        };
                        throw new HttpResponseException(response);
                    }
                }
                if (trade.Receiver == user)
                {
                    if (newStatus == Models.Enums.TradeStatus.Declined)
                    {
                        trade.TradeStatus = newStatus;
                        trade.ModifiedDate = DateTime.Now;
                        trade.ModifiedBy = user.FullName;
                        trade.RelatedtradeItems.Clear();
                    }
                    else
                    {
                        if(newStatus == Models.Enums.TradeStatus.Accepted)
                        {
                            trade.TradeStatus = newStatus;
                            trade.ModifiedDate = DateTime.Now;
                            trade.ModifiedBy = user.FullName;
                            List<TradeItem> trr = this._context.TradeItem.Include(tr => tr.item).Where(tr => tr.TradeId == trade.Id).ToList();
                            ICollection<ItemDto> sentItems = new Collection<ItemDto>();
                            ICollection<ItemDto> receivedItems = new Collection<ItemDto>();
                            ICollection<TradeItem> tr = trade.RelatedtradeItems;
                            List<TradeItem> l = tr.ToList();
                            for (int i = 0; i < l.Count; i++)
                            {
                                if (trade.Sender.Id == l[i].item.Owner.Id)
                                    l[i].item.Owner = trade.Receiver;
                                else
                                    l[i].item.Owner = trade.Sender;
                                this._context.Item.Update(l[i].item);
                            }
                            trade.RelatedtradeItems.Clear();
                        }
                        else
                        {
                            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content = new StringContent("As a receive, you can only accept or decline a Trade", System.Text.Encoding.UTF8, "text/plain"),
                                StatusCode = HttpStatusCode.BadRequest
                            };
                            throw new HttpResponseException(response);
                        }
                   
                    }
                }
            }
            else
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("You cannot perform this action", System.Text.Encoding.UTF8, "text/plain"),
                    StatusCode = HttpStatusCode.BadRequest
                };
                throw new HttpResponseException(response);
            }
            this._context.SaveChanges();
            return trade.TradeStatus.ToString();
        }
    }
}