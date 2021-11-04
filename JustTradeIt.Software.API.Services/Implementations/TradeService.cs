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

        public TradeService(ITradeRepository ITradeRepository)
        {
            this.ITradeRepository = ITradeRepository;
        }
        public string CreateTradeRequest(string email, TradeInputModel tradeRequest)
        {
            //this.SendEmailOfNewTrade("dhia666@gmail.com");
            return this.ITradeRepository.CreateTradeRequest(email, tradeRequest);
        }

        public void SendEmailOfNewTrade(string email)
        {
            var connectionFactory = new ConnectionFactory()
            {
                UserName = "guest",
                Password = "guest",
                HostName = "localhost"
            };
            var connection = connectionFactory.CreateConnection();
            var model = connection.CreateModel();
            var properties = model.CreateBasicProperties();
            properties.Persistent = false;
            byte[] messagebuffer = System.Text.Encoding.Default.GetBytes(email);
            model.BasicPublish("JustTradeItExchange", "directexchange_key", properties, messagebuffer);
        }

        public void SendEmailOfUpdateTrade(string email)
        {
            var connectionFactory = new ConnectionFactory()
            {
                UserName = "guest",
                Password = "guest",
                HostName = "localhost"
            };
            var connection = connectionFactory.CreateConnection();
            var model = connection.CreateModel();
            var properties = model.CreateBasicProperties();
            properties.Persistent = false;
            byte[] messagebuffer = System.Text.Encoding.Default.GetBytes(email);
            model.BasicPublish("JustTradeItExchange", "updaterequestexchange_key", properties, messagebuffer);
        }
        public TradeDetailsDto GetTradeByIdentifer(string tradeIdentifier)
        {
            return this.ITradeRepository.GetTradeByIdentifier(tradeIdentifier);
        }

        public IEnumerable<TradeDto> GetTradeRequests(string email, bool onlyIncludeActive = true)
        {
            return this.ITradeRepository.GetTradeRequests(email, onlyIncludeActive);
        }

        public IEnumerable<TradeDto> GetTrades(string email)
        {
            return this.ITradeRepository.GetTrades(email);
        }

        public void UpdateTradeRequest(string identifier, string email, TradeStatus status)
        {
            //this.ITradeRepository.UpdateTradeRequest(identifier, email,status);
            this.SendEmailOfUpdateTrade("dhia666@gmail.com");
        }
    }
}