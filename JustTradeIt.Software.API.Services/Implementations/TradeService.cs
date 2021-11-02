using System;
using System.Collections.Generic;
using JustTradeIt.Software.API.Models.DTOs;
using JustTradeIt.Software.API.Models.Enums;
using JustTradeIt.Software.API.Models.InputModels;
using JustTradeIt.Software.API.Repositories.Interfaces;
using JustTradeIt.Software.API.Services.Interfaces;

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
            return this.ITradeRepository.CreateTradeRequest(email, tradeRequest);
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
            this.ITradeRepository.UpdateTradeRequest(identifier, email,status);
        }
    }
}