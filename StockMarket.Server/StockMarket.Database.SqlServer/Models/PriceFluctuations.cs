using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StockMarket.Database.SqlServer.Models
{
    public class PriceFluctuations : BaseModel
    {
        public int CompanyId { get; set; }
        [JsonIgnore]
        public Company? Company { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
    }
}
