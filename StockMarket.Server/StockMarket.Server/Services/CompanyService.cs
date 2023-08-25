using AutoMapper;
using StockMarket.Database.SqlServer;
using StockMarket.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SstockMarket.Server.Models.Responses;

namespace StockMarket.Server.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;
        private readonly IMapper _mapper;

        public CompanyService(IDbContextFactory<DatabaseContext> dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public async Task<CompanyResponse?> GetAsync(int companyId)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var company = await dbContext.Companys
                .Include(x => x.BuyingSelingShares)
                .SingleOrDefaultAsync(x => x.Id == companyId);

            if (company == null)
                return null;

            var response = _mapper.Map<CompanyResponse>(company);
            response.OpenPrice = company.BuyingSelingShares.First().Price;
            response.ClosePrice = company.BuyingSelingShares.Last().Price;
            response.HighPrice = company.BuyingSelingShares.Select(x => x.Price).Max();
            response.LowPrice = company.BuyingSelingShares.Select(x => x.Price).Min();

            return response;
        }

        public async Task<List<CompanyResponse>> GetListAsync()
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var companies = await dbContext.Companys
                .Include(x => x.BuyingSelingShares.OrderByDescending(bs => bs.DateCreated).Take(2))
                .ToListAsync();

            var response = _mapper.Map<List<CompanyResponse>>(companies);
            return response;
        }

        public async Task<CompanyResponse?> GetLastFiveAsync(int companyId)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var company = await dbContext.Companys
                .Include(x => x.BuyingSelingShares).OrderByDescending(x => x.DateCreated)
                .Take(6)
                .SingleOrDefaultAsync(x => x.Id == companyId);

            if (company == null)
                return null;

            var response = _mapper.Map<CompanyResponse>(company);
            return response;
        }

        public async Task UpdateAsync(int companyId, int shares, string buySell)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var company = await dbContext.Companys
                .SingleAsync(x => x.Id == companyId);

            if (buySell == "buy")
            {
                company.Share = company.Share - shares;
            }
            else
            {
                company.Share = company.Share + shares;
            }

            dbContext.Companys.Update(company);
            await dbContext.SaveChangesAsync();
        }
    }
}
