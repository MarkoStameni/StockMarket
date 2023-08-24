using AutoMapper;
using StockMarket.Database.SqlServer;
using StockMarket.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SstockMarket.Server.Models.Responses;
using StockMarket.Database.SqlServer.Models;

namespace StockMarket.Server.Services
{
    public class BuyingSellingShareService : IBuyingSellingShareService
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;
        private readonly IMapper _mapper;

        public BuyingSellingShareService(IDbContextFactory<DatabaseContext> dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public async Task<List<BuyingSellingShareResponse>> GetLastFiveAsync(int companyId)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var buyingSelingShares = await dbContext.BuyingSelingShares
                .Where(x => x.CompanyId == companyId)
                .OrderByDescending(x => x.DateCreated)
                .Take(6)
                .ToListAsync();

            var response = _mapper.Map<List<BuyingSellingShareResponse>>(buyingSelingShares);
            return response;
        }

        public async Task UpdateAsync(int companyId, decimal newPrice)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var buyingSellingShare = new BuyingSellingShare
            {
                DateUpdated = DateTime.Now,
                DateCreated = DateTime.Now,
                CompanyId = companyId,
                Price = newPrice
            };

            dbContext.BuyingSelingShares.Add(buyingSellingShare);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<BuyingSellingShareResponse>> GetLAsync(int companyId)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var buyingSelingShares = await dbContext.BuyingSelingShares
                .Where(x => x.CompanyId == companyId)
                .OrderByDescending(x => x.DateCreated)
                .Take(6)
                .ToListAsync();

            var response = _mapper.Map<List<BuyingSellingShareResponse>>(buyingSelingShares);
            return response;
        }
    }
}
