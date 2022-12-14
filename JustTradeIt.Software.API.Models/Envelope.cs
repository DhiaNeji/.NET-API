using System.Collections.Generic;

namespace JustTradeIt.Software.API.Models
{
    public class Envelope<T> where T : class
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int MaxPages { get; set; }
        public IEnumerable<T> Items { get; set; }

        public Envelope(int pageNumber, int pageSize, int maxPages, IEnumerable<T> items)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            MaxPages = maxPages;
            Items = items;
        }
    }
}