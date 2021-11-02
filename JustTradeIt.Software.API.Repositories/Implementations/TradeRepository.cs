using System;
using System.Collections.Generic;
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
            List<Item> list2 = new List<Item>();
            Trade t=null;
            for (int i=0;i<itemsToTrade.Count();i++)
            {
                if(int.Parse(itemsToTrade[i].Owner.Identifier)!=sender.Id && int.Parse(itemsToTrade[i].Owner.Identifier) != receiver.Id)
                {
                    validateSenderAndReceiver = false;
                }
                else
                {
                    int identifier = int.Parse(itemsToTrade[i].Identifier);
                    Item itemToTrade=this._context.Item.Where(i => i.Id == identifier).First();
                    if (itemToTrade.Owner == sender)
                        list.Add(itemToTrade);
                    else
                        list2.Add(itemToTrade);
                }
            }
            if(validateSenderAndReceiver)
            {
                //ItemCondition itemCondition = this._context.ItemCondition.Where(i => i.ConditionCode == TradeStatus.Pending.ToString()).First();

                t = new Trade("id", DateTime.Now, DateTime.Now, sender.FullName, TradeStatus.Pending);
                t.Sender = sender;
                t.Receiver = receiver;
                t.SendingtradeItems = list;
                t.ReceivingtradeItems = list2;
                this._context.Trade.Add(t);
                this._context.SaveChanges();
            }
            return t.Id.ToString();
        }

        public TradeDetailsDto GetTradeByIdentifier(string identifier)
        {
            Trade t = this._context.Trade.Include(t=>t.Sender).Include(t=>t.Receiver).Include(t=>t.ReceivingtradeItems).Include(t => t.SendingtradeItems).Where(t => t.Id == int.Parse(identifier)).First();
            return this.mapper.Map<TradeDetailsDto>(t);
        }

        public IEnumerable<TradeDto> GetTradeRequests(string email, bool onlyIncludeActive)
        {
            User user = (User)this._context.User.Where(u => u.Email == email); 
            List<Trade> l=new List<Trade>();
            if (onlyIncludeActive)
            {
                l = user.userTrades.Where(t => t.TradeStatus != Models.Enums.TradeStatus.Accepted).ToList();
            }
            List<TradeDto> list = new List<TradeDto>();
            for(int i=0;i<l.Count();i++)
            {
                list.Add(this.mapper.Map<TradeDto>(l[i]));
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