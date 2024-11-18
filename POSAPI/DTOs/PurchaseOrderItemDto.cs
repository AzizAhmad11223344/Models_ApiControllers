namespace POSAPI.DTOs
{
    public class PurchaseOrderItemDto
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public int PurchaseOrderId { get; set; }
       public int ProductId { get; set; }
    }
}
