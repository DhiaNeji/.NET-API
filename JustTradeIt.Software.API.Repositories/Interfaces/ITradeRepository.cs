using JustTradeIt.Software.API.Models.DTOs;
using JustTradeIt.Software.API.Models.InputModels;
using JustTradeIt.Software.API.Models.Models;
using System.Collections.Generic;

namespace JustTradeIt.Software.API.Repositories.Interfaces
{
    public interface ITradeRepository
    {
        IEnumerable<TradeDto> GetTrades(string email);
        TradeDetailsDto GetTradeByIdentifier(string identifier);
        IEnumerable<TradeDto> GetTradeRequests(string email, bool onlyIncludeActive);
        string CreateTradeRequest(string email, TradeInputModel trade);
        string UpdateTradeRequest(string email, string identifier, Models.Enums.TradeStatus newStatus);
        IEnumerable<TradeDto> GetUserTrades(string userIdentifier);
    }
}