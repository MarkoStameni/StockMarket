namespace SstockMarket.Server.Models.Responses
{
    public class PriceFluctuationsResponse
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal Price { get; set; }
    }
}
