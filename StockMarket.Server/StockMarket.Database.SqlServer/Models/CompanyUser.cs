using System.Text.Json.Serialization;

namespace StockMarket.Database.SqlServer.Models
{
    public class CompanyUser : BaseModel
    {
        public int Shares { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        [JsonIgnore]
        public User User { get; set; } = null!; 
        public Company Company { get; set; } = null!;
    }
}
