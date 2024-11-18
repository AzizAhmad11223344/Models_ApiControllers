using POSAPI.Models;

namespace POSAPI.DTOs
{
    public class ProductDto
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public int CategoryId { get; set; }
    }
}
