using AutoMapper;
using StockMarket.Database.SqlServer;
using StockMarket.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SstockMarket.Server.Models.Responses;
using StockMarket.Database.SqlServer.Models;

namespace StockMarket.Server.Services
{
    public class PriceFluctuationsService : IPriceFluctuationsService
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;
        private readonly IMapper _mapper;

        public PriceFluctuationsService(IDbContextFactory<DatabaseContext> dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public async Task<List<PriceFluctuationsResponse>> GetLastFiveAsync(int companyId)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var priceFluctuations = await dbContext.PriceFluctuations
                .Where(x => x.CompanyId == companyId)
                .OrderByDescending(x => x.DateCreated)
                .Take(6)
                .ToListAsync();

            var response = _mapper.Map<List<PriceFluctuationsResponse>>(priceFluctuations);
            return response;
        }

        public async Task UpdateAsync(int companyId, decimal newPrice)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var priceFluctuations = new PriceFluctuations
            {
                DateUpdated = DateTime.Now,
                DateCreated = DateTime.Now,
                CompanyId = companyId,
                Price = newPrice
            };

            dbContext.PriceFluctuations.Add(priceFluctuations);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<PriceFluctuationsResponse>> GetLAsync(int companyId)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var priceFluctuations = await dbContext.PriceFluctuations
                .Where(x => x.CompanyId == companyId)
                .OrderByDescending(x => x.DateCreated)
                .Take(6)
                .ToListAsync();

            var response = _mapper.Map<List<PriceFluctuationsResponse>>(priceFluctuations);
            return response;
        }
    }
}
