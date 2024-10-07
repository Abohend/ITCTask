namespace EntityLayer.Models
{
    public class Discount
    {
        public int Id { get; set; }
        public int DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
