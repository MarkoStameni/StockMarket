using StockMarket.Database.SqlServer.Models;

namespace StockMarket.Server.Services.Interfaces
{
    public interface ICompanyUserService
    {
        Task UpdateAsync(int userId, int companyId, int shares);
        Task<CompanyUser?> GetAsync(int userId, int companyId);
    }
}
