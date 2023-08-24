
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockMarket.Database.SqlServer.Models
{
    public class Company : BaseModel
    {
        public string Name { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal OpenPrice { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ClosePrice { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? HighPrice { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? LowPrice { get; set; }
        public int Share { get; set; }
        public List<CompanyUser> CompanyUsers { get; } = new();
        public List<BuyingSellingShare> BuyingSelingShares { get; set; } = default!;
    }
}
