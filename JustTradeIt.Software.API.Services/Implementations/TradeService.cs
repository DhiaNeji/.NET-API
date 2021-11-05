using System;
using System.Collections.Generic;
using JustTradeIt.Software.API.Models.DTOs;
using JustTradeIt.Software.API.Models.Enums;
using JustTradeIt.Software.API.Models.InputModels;
using JustTradeIt.Software.API.Repositories.Interfaces;
using JustTradeIt.Software.API.Services.Interfaces;
using RabbitMQ.Client;

namespace JustTradeIt.Software.API.Services.Implementations
{
    public class TradeService : ITradeService
    {
        public ITradeRepository ITradeRepository;
        public QueueService queueService;

        public IUserRepository iTUserRepository;
        public TradeService(ITradeRepository ITradeRepository,IUserRepository userRepository,QueueService queueService)
        {
            this.ITradeRepository = ITradeRepository;
            this.iTUserRepository = userRepository;
            this.queueService = queueService;
        }
        public string CreateTradeRequest(string email, TradeInputModel tradeRequest)
        {
            this.SendEmailOfNewTrade(this.iTUserRepository.GetUserInformation(tradeRequest.ReceiverIdentifier).Email);
            return this.ITradeRepository.CreateTradeRequest(email, tradeRequest);
        }

        public void SendEmailOfNewTrade(string email)
        {
            byte[] body = System.Text.Encoding.Default.GetBytes(email);
            this.queueService.Dispose();
            this.queueService.PublishMessage("directexchange_key", body);
        }

        public void SendEmailOfUpdateTrade(string email)
        {
            byte[] body = System.Text.Encoding.Default.GetBytes(email);
            this.queueService.Dispose();
            this.queueService.PublishMessage("updaterequestexchange_key", body);
        }
        public TradeDetailsDto GetTradeByIdentifer(string tradeIdentifier)
        {
            return this.ITradeRepository.GetTradeByIdentifier(tradeIdentifier);
        }

        public IEnumerable<TradeDto> GetTradeRequests(string email, bool onlyIncludeActive)
        {
            return this.ITradeRepository.GetTradeRequests(email, onlyIncludeActive);
        }

        public IEnumerable<TradeDto> GetTrades(string email)
        {
            return this.ITradeRepository.GetTrades(email);
        }

        public void UpdateTradeRequest(string identifier, string email, TradeStatus status)
        {
             this.ITradeRepository.UpdateTradeRequest(identifier, email,status);
             this.SendEmailOfUpdateTrade(this.iTUserRepository.getUserToNotifyByTrade(email,int.Parse(identifier)).Email);
        }
    }
}