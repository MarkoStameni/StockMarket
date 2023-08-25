using AutoMapper;
using Serilog;
using StockMarket.Database.SqlServer.Models;
using StockMarket.Server.Models.Responses;
using StockMarket.Server.Services.Interfaces;

namespace StockMarket.Server.Helpers
{
    public class TypeTactics
    {
        private readonly ICompanyService _companyService;
        private readonly IUserService _userService;
        private readonly ICompanyUserService _companyUserService;
        private readonly IPriceFluctuationsService _priceFluctuationsShare;
        private readonly IMapper _mapper;

        public TypeTactics(
            ICompanyService companyService, 
            IUserService userService,
            ICompanyUserService companyUserService,
            IPriceFluctuationsService priceFluctuationsShare,
            IMapper mapper)
        {
            _companyService = companyService;
            _userService = userService;
            _companyUserService = companyUserService;
            _priceFluctuationsShare = priceFluctuationsShare;
            _mapper = mapper;
        }

        public async Task CheckTactics(int userId, int companyId)
        {
            var sumPrice = 0.00M;
            var userResponse = await _userService.GetAsync(userId);

            switch (userResponse!.Tactics!.Id)
            { 
                case 1:
                    var company = await _companyService.GetLastFiveAsync(companyId);
                    var lastPrice = company!.PriceFluctuations!.First().Price;

                    foreach (var item in company.PriceFluctuations!.Skip(1))
                    {
                        sumPrice += item.Price;
                    }

                    if (lastPrice > sumPrice / company.PriceFluctuations!.Count())
                    {
                        var availableFunds = userResponse.BalanceFunds * userResponse.RiskCoefficient;
                        var canBuy = (int)(availableFunds / lastPrice);

                        if (company.Share > canBuy)
                        {
                            if (canBuy != 0)
                            {
                                Log.Warning("You bought as many shares as you have available funds");
                                await _companyUserService.UpdateAsync(userId, companyId, canBuy);
                                await UpdateNewBalanceFunds(userResponse, lastPrice, canBuy, "buy");
                                await _companyService.UpdateAsync(companyId, canBuy, "buy");
                                await InsertNewPrice(companyId, canBuy, lastPrice, "buy");
                            }
                        }
                        else
                        {
                            if(company.Share > 0)
                            {
                                Log.Warning("You bought all the remaining shares");
                                await _companyUserService.UpdateAsync(userId, companyId, company.Share);
                                await UpdateNewBalanceFunds(userResponse, lastPrice, company.Share, "buy");
                                await _companyService.UpdateAsync(companyId, company.Share, "buy");
                                await InsertNewPrice(companyId, company.Share, lastPrice, "buy");
                            }
                        }
                    }
                    else
                    {
                        var companyUser = await _companyUserService.GetAsync(userId, companyId);
                        if (companyUser != null)
                        {
                            if (companyUser.Shares > 0)
                            {
                                await _companyUserService.UpdateAsync(userId, companyId, 0);
                                await UpdateNewBalanceFunds(userResponse, lastPrice, companyUser.Shares, "sell");
                                await _companyService.UpdateAsync(companyId, companyUser.Shares, "sell");
                                await InsertNewPrice(companyId, companyUser.Shares, lastPrice, "sell");
                            }
                            else
                            {
                                Log.Warning("You do not have any company shares");
                            }
                        }
                    }

                    break;
                case 2:
                    
                    break;
                case 3:
                    
                    break;
                case 4:
                    
                    break;
                case 5:
                    
                    break;
            }
        }

        public async Task UpdateNewBalanceFunds(UserResponse userResponse, decimal lastPrice, decimal canBuy, string buySell)
        {
            if (buySell == "buy")
            {
                var newBalanceFunds = userResponse.BalanceFunds - (canBuy * lastPrice);
                var user = _mapper.Map<User>(userResponse);
                user.BalanceFunds = newBalanceFunds;
                await _userService.UpdateAsync(user);
            }
            else
            {
                var newBalanceFunds = canBuy * lastPrice;
                userResponse.BalanceFunds += newBalanceFunds;
                var user = _mapper.Map<User>(userResponse);
                await _userService.UpdateAsync(user);
            }
        }
 
        public async Task InsertNewPrice(int companyId, int shares, decimal lastPrice, string buySell)
        {
            var newPrice = 0.00M;

            switch (companyId)
            {
                case 1:
                    newPrice = buySell == "buy" ? shares / 10 + lastPrice : lastPrice - shares / 10; 

                    break;
                case 2:
                    newPrice = buySell == "buy" ? lastPrice + (lastPrice * 0.02M + shares * 0.15M) : lastPrice - (lastPrice * 0.02M + shares * 0.15M);

                    break;
                case 3:
                    //newPrice = buySell == "buy" ? lastPrice + Math.Sqrt(lastPrice) * shares : lastPrice - Math.Sqrt(lastPrice) * shares;

                    break;
                case 4:
                    //newPrice = buySell == "buy" ? lastPrice + Math.Sqrt(lastPrice) * shares * 0.5M : lastPrice - Math.Sqrt(lastPrice) * shares * 0.5M;

                    break;
                case 5:

                    newPrice = buySell == "buy" ? lastPrice + shares * shares * 0.01M : lastPrice - shares * shares * 0.01M;
                    break;
            }

            await _priceFluctuationsShare.UpdateAsync(companyId, newPrice);
        }
    }
}
