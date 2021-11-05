using JustTradeIt.Software.API.Models;
using JustTradeIt.Software.API.Models.DTOs;
using JustTradeIt.Software.API.Models.Helpers;
using JustTradeIt.Software.API.Models.InputModels;
using JustTradeIt.Software.API.Models.Models;
using JustTradeIt.Software.API.Repositories.Interfaces;
using JustTradeIt.Software.API.Services.Implementations;
using JustTradeIt.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace JustTradeIt.Software.API.Controllers
{
    [Route("api/items")]
    [Authorize]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private ItemService _ItemService;

        private IItemRepository _ItemRepository;
        public ItemController(ItemService itemService, IItemRepository itemRepository)
        {
            this._ItemService = itemService;
            this._ItemRepository=itemRepository;
        }

        [HttpGet("{pageSize}/{pageNumber}/{ascendingOrder}")]
        public Envelope<ItemDto> getAvailableItems(int pageSize, int pageNumber, bool ascendingOrder)
        {
            return this._ItemService.GetItems(pageSize, pageNumber, ascendingOrder);
        }
        [HttpGet("{identifier}")]
        public ItemDetailsDto getItemByIdentifier(string identifier)
        {
            return this._ItemService.GetItemByIdentifier(identifier);
        }

        [HttpPost]
        public ItemDto createNewItem(ItemInputModel itemInputModel)
        {
            UserDto user = (UserDto)HttpContext.Items["User"];
            return this._ItemService.AddNewItem(user.Email, itemInputModel);
        }

        [HttpDelete("{itemIdentifier}")]
        public void removeItemByIdentifier(string itemIdentifier)
        {
            UserDto user = (UserDto)HttpContext.Items["User"];
            this._ItemService.RemoveItem(user.Email, itemIdentifier);
        }
    }
}