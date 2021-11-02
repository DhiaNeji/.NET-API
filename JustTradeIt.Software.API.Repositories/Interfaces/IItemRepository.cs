using JustTradeIt.Software.API.Models;
using JustTradeIt.Software.API.Models.DTOs;
using JustTradeIt.Software.API.Models.InputModels;
using JustTradeIt.Software.API.Models.Models;
using System;
using System.Collections.Generic;

namespace JustTradeIt.Software.API.Repositories.Interfaces
{
    public interface IItemRepository 
    {
        Envelope<ItemDto> GetAllItems(int pageSize, int pageNumber, bool ascendingSortOrder);
        ItemDetailsDto GetItemByIdentifier(string identifier);
        ItemDto AddNewItem(ItemInputModel item);
        void RemoveItem(string email, string identifier);

    }
}