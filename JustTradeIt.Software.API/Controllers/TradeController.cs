using JustTradeIt.Software.API.Models.DTOs;
using JustTradeIt.Software.API.Models.Enums;
using JustTradeIt.Software.API.Models.InputModels;
using JustTradeIt.Software.API.Repositories.Interfaces;
using JustTradeIt.Software.API.Services.Implementations;
using JustTradeIt.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web.Helpers;

namespace JustTradeIt.Software.API.Controllers
{
    [Route("api/trades")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private TradeService _tradeService;

        private ITradeRepository _ITradeRepository;

        public TradeController(TradeService tradeService,ITradeRepository tradeRepository)
        {
            this._tradeService = tradeService;
            this._ITradeRepository = tradeRepository;
        }



        [HttpPost("{email}")]
        public string CreateNewTrade(string email,TradeInputModel trade)
        {
            return this._tradeService.CreateTradeRequest(email, trade);
        }

        [HttpGet("{identifier}")]
        public TradeDetailsDto getDetailedTrade(string identifier)
        {
            return this._tradeService.GetTradeByIdentifer(identifier);
        }

        [HttpPut("{identifier}/{email}")]
        public void getDetailedTrade(string identifier,string email, [FromBody]TradeStatus tradeStatus)
        {
            this._tradeService.UpdateTradeRequest(identifier,email,tradeStatus);
        }
        [HttpGet]
        public IEnumerable<TradeDto> getAuthenticatedUserTrades(bool onlyIncludeActive)
        {
            string email = "dhia666@gmail.com";
            return this._tradeService.GetTradeRequests(email, onlyIncludeActive);
        }
    }
}