
using StockMarket.Database.SqlServer.Models;

namespace StockMarket.Server.Models.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal BalanceFunds { get; set; }
        public decimal? NumberShares { get; set; }
        public decimal RiskCoefficient { get; set; }
        public Tactics? Tactics { get; set; }
    }
}
