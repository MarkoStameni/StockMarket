using StockMarket.Database.SqlServer.Models;
using StockMarket.Server.Models.Responses;
using StockMarket.Server.Requests;

namespace StockMarket.Server.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse?> GetAsync(int id);
        Task<List<UserResponse>> GetListAsync();
        Task<int?> InsertAsync(User user);
        Task<User?> UpdateAsync(User user);
        Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model, string ipAddress);
        Task<AuthenticateResponse?> RefreshToken(string token, string ipAddress);
    }
}
