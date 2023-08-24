using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StockMarket.Database.SqlServer.Models
{
    public class User : BaseModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal BalanceFunds { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal RiskCoefficient { get; set; }
        public string Salt { get; set; } = string.Empty;
        public List<CompanyUser> CompanyUsers { get; } = new();
        public int? TacticsId { get; set; } = new();
        public Tactics? Tactics { get; set; } = new();
        [JsonIgnore]
        public string PasswordHash { get; set; } = string.Empty;

        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; set; } = default!;
    }
}
