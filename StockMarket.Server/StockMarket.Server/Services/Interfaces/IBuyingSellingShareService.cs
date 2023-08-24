using SstockMarket.Server.Models.Responses;

namespace StockMarket.Server.Services.Interfaces
{
    public interface IBuyingSellingShareService
    {
        Task<List<BuyingSellingShareResponse>> GetLastFiveAsync(int companyId);
        Task UpdateAsync(int companyId, decimal newPrice);
        Task<List<BuyingSellingShareResponse>> GetLAsync(int companyId);
    }
}
