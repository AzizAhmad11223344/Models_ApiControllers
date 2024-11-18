namespace POSAPI.Models
{
    public class SaleOrderItem: BaseEntity
    {
     
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
  

        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int SaleOrderId { get; set; }
        public SaleOrder? SaleOrder { get; set; }
    }
}

