using SstockMarket.Server.Models.Responses;
using StockMarket.Database.SqlServer.Models;

namespace StockMarket.Server.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyResponse?> GetAsync(int companyId);
        Task<List<CompanyResponse>> GetListAsync();
        Task<CompanyResponse?> GetLastFiveAsync(int companyId);
        Task UpdateAsync(int companyId, int shares, string buySell);
    }
}
