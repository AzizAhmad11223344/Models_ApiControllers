namespace POSAPI.Models
{
    public class SaleOrder: BaseEntity
    {
      
        public string Name { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string? Remarks { get; set; }
        public double Amouunt { get; set; }
        public double SubTotal { get; set; }
        public double Discount { get; set; }
        public double Tax { get; set; }
        public double Total { get; set; }
    

        public ICollection<SaleOrderItem>? SaleOrderItems { get; set; }
    }
}
