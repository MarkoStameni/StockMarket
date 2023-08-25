using SstockMarket.Server.Models.Responses;

namespace StockMarket.Server.Services.Interfaces
{
    public interface IPriceFluctuationsService
    {
        Task<List<PriceFluctuationsResponse>> GetLastFiveAsync(int companyId);
        Task UpdateAsync(int companyId, decimal newPrice);
        Task<List<PriceFluctuationsResponse>> GetLAsync(int companyId);
    }
}
