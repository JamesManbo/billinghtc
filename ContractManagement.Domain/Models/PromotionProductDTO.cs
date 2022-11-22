namespace ContractManagement.Domain.Models
{
    public class PromotionProductDTO
    {
        public int Id { get; set; }
        public int PromotionDetailId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public bool IsOurProduct { get; set; }
        public bool IsChange { get; set; }
    }
}