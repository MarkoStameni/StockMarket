using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StockMarket.Database.SqlServer;
using StockMarket.Server.Models.Responses;
using StockMarket.Server.Services.Interfaces;

namespace StockMarket.Server.Services
{
    public class TradeServicee : ITradeService
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;
        private readonly IMapper _mapper;

        public TradeServicee(IDbContextFactory<DatabaseContext> dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }
    }
}
