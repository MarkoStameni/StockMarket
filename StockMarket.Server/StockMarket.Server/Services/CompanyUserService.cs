using Microsoft.EntityFrameworkCore;
using StockMarket.Database.SqlServer;
using StockMarket.Database.SqlServer.Models;
using StockMarket.Server.Services.Interfaces;

namespace StockMarket.Server.Services
{
    public class CompanyUserService : ICompanyUserService
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

        public CompanyUserService(IDbContextFactory<DatabaseContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<CompanyUser?> GetAsync(int userId, int companyId)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var user = await dbContext.Users
                .Include(x => x.CompanyUsers)
                .SingleOrDefaultAsync(x => x.Id == userId);

            if (user != null)
            {
                var userCompanyRelation = user?.CompanyUsers.FirstOrDefault(x => x.CompanyId == companyId);
                return userCompanyRelation;
            }

            return null;
        }

        public async Task UpdateAsync(int userId, int companyId, int shares)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var companyUser = await GetAsync(userId, companyId);

            if (companyUser != null)
            {
                companyUser.DateUpdated = DateTime.Now;
                companyUser.Shares = shares;
                dbContext.CompanyUsers.Update(companyUser);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                var newUserCompany = new CompanyUser
                {
                    DateCreated = DateTime.Now,
                    UserId = userId,
                    CompanyId = companyId,
                    Shares = shares
                };

                dbContext.CompanyUsers.Add(newUserCompany);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
