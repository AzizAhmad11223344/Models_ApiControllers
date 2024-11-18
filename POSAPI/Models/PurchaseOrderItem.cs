namespace POSAPI.Models
{
    public class PurchaseOrderItem: BaseEntity
    {
        
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }


        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int PurchaseOrderId { get; set; }
        public PurchaseOrder? PurchaseOrder { get; set; }
    }
}
