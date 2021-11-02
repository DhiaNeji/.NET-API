using JustTradeIt.Software.API.Models;
using JustTradeIt.Software.API.Models.DTOs;
using JustTradeIt.Software.API.Models.InputModels;
using JustTradeIt.Software.API.Models.Models;
using System.Collections.Generic;

namespace JustTradeIt.Software.API.Services.Interfaces
{
    public interface IItemService
    {
        Envelope<ItemDto> GetItems(int pageSize, int pageNumber, bool ascendingSortOrder);
        ItemDetailsDto GetItemByIdentifier(string identifier);
        ItemDto AddNewItem(string email, ItemInputModel item);
        void RemoveItem(string email, string itemIdentifier);

    }
}