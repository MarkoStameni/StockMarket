using Microsoft.AspNetCore.SignalR;
using StockMarket.Server.Helpers;
using StockMarket.Server.Services.Interfaces;

namespace StockMarket.Server.Hubs
{
    public class DataHub : Hub
    {
        private readonly ICompanyService _companyService;
        private readonly TypeTactics _typeTactics;
        public DataHub(ICompanyService companyService, TypeTactics typeTactics) 
        {
            _companyService = companyService;
            _typeTactics = typeTactics;
        }

        public  async Task GetHistoryTrade(UserCompany userCompany)
       {
            var response = await _companyService.GetAsync(userCompany.CompanyId);
            await Clients.All.SendAsync("ReceiveData", response);
        }

        public async Task StartTrade(UserCompany userCompany)
        {
            await _typeTactics.CheckTactics(userCompany.UserId, userCompany.CompanyId);
            var response = await _companyService.GetAsync(userCompany.CompanyId);
            await Clients.All.SendAsync("ReceiveData", response);
        }
    }
}

public class UserCompany
{
    public int UserId { get; set; }
    public int CompanyId { get; set; }
}

