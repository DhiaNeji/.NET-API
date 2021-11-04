using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            bool validateSenderAndReceiver = true;
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
                    this._context.SaveChanges();
                    System.Diagnostics.Debug.WriteLine("done");

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
                        this._context.SaveChanges();
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("on ELSE statement");
                    }
                }
            }
            //if(validateSenderAndReceiver)
            //{
            //ItemCondition itemCondition = this._context.ItemCondition.Where(i => i.ConditionCode == TradeStatus.Pending.ToString()).First();

            //t = new Trade("id", DateTime.Now, DateTime.Now, sender.FullName, TradeStatus.Pending);
            //t.Sender = sender;
            //t.Receiver = receiver;
            //t.RelatedtradeItems = list;
            //  t.ReceivingtradeItems = list2;
            //this._context.Trade.Add(t);
            //this._context.SaveChanges();
            //}
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
            System.Diagnostics.Debug.WriteLine(t.TradeStatus);
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
            Trade trade= this._context.Trade.Include(t=>t.Sender).Include(t => t.Receiver).Where(u => u.Id == int.Parse(identifier)).First();
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
                    }
                    else
                    {
                        throw new Exception("You cannot perform this action");
                    }
                }
                if (trade.Receiver == user)
                {
                    if (newStatus == Models.Enums.TradeStatus.Declined || newStatus == Models.Enums.TradeStatus.Accepted)
                    { 
                        trade.TradeStatus = newStatus;
                        trade.ModifiedDate = DateTime.Now;
                        trade.ModifiedBy = user.FullName;
                    }
                    else
                    {
                        throw new Exception("You cannot perform this action");
                    }
                }
            }
            else
            {
                throw new Exception("Status is not on pending");
            }
            this._context.SaveChanges();
            return trade.TradeStatus.ToString(); //To be performed when DTO is done
        }
    }
}