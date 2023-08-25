
using StockMarket.Database.SqlServer.Models;

namespace SstockMarket.Server.Models.Responses
{
    public class CompanyResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal OpenPrice { get; set; }
        public decimal? ClosePrice { get; set; }
        public decimal? HighPrice { get; set; }
        public decimal? LowPrice { get; set; }
        public decimal? LastPrice { get; set; }
        public int Share { get; set; }
        public decimal? IncreaseDecrease { get; set; }
        public string? IncreaseDecreaseText { get; set; }
        public List<PriceFluctuations>? PriceFluctuations { get; set; }
    }
}
