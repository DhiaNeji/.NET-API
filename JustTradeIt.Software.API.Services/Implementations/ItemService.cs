using JustTradeIt.Software.API.Models;
using JustTradeIt.Software.API.Models.DTOs;
using JustTradeIt.Software.API.Models.InputModels;
using JustTradeIt.Software.API.Models.Models;
using JustTradeIt.Software.API.Repositories;
using JustTradeIt.Software.API.Repositories.Implementations;
using JustTradeIt.Software.API.Repositories.Interfaces;
using JustTradeIt.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace JustTradeIt.Software.API.Services.Implementations
{
    public class ItemService : IItemService
    {
        private IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            this._itemRepository = itemRepository;
        }
        public ItemDto AddNewItem(string email, ItemInputModel item)
        {
            return this._itemRepository.AddNewItem(email,item);
        }

        public ItemDetailsDto GetItemByIdentifier(string identifier)
        {
            return this._itemRepository.GetItemByIdentifier(identifier);
        }

        public Envelope<ItemDto> GetItems(int pageSize, int pageNumber, bool ascendingSortOrder)
        {
            return this._itemRepository.GetAllItems(pageSize, pageNumber, ascendingSortOrder);
        }

        public void RemoveItem(string email, string itemIdentifier)
        {
            this._itemRepository.RemoveItem("Test", itemIdentifier);
        }

    }
}