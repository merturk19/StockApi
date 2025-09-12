using Microsoft.EntityFrameworkCore;

namespace StockApi.Models
{
    public class StockItem
    {
        public int Id { get; set; }

        public string? name { get; set; }
    }
}
