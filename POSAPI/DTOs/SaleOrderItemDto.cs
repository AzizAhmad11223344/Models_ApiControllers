namespace POSAPI.DTOs
{
    public class SaleOrderItemDto
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }


        public int ProductId { get; set; }
        public int SaleOrderId { get; set; }
    }
}
