
namespace StockMarket.Server.Requests.User
{
    public class AddUserRequest
    {
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal BalanceFunds { get; set; }
        public decimal RiskCoefficient { get; set; }
        public int TackticId { get; set; }
    }
}
