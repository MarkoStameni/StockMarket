using StockMarket.Server.Attributes;
using StockMarket.Server.Models;
using StockMarket.Server.Models.Response;
using Microsoft.AspNetCore.Mvc;
using StockMarket.Server.Services.Interfaces;
using SstockMarket.Server.Models.Responses;
using Microsoft.AspNetCore.SignalR;
using StockMarket.Server.Helpers;

namespace StockMarket.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        [Route("GetCompany/{companyId}")]
        public async Task<IActionResult> GetCompany(int companyId)
        {
            var company = await _companyService.GetAsync(companyId);
            if (company == null)
                return UnprocessableEntity(new ApiResult(ErrorCodes.InvalidParameters, $"No company fount for ID: {companyId}"));

            return Ok(new ApiResult<CompanyResponse>(company));
        }

        [HttpGet]
        [Route("GetCompanies")]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _companyService.GetListAsync();

            if (companies.Count == 0)
                return Ok(new ApiResult<BaseResponse>(new BaseResponse { Message = "No companies found" }));

            foreach(var item in companies)
            {
                if (item.PriceFluctuations != null && item.PriceFluctuations.Count > 0)
                {
                    var newPrice = item.PriceFluctuations.First().Price;
                    var oldPrice = item.PriceFluctuations.Last().Price;
                    item.LastPrice = newPrice;
                    if (newPrice > oldPrice)
                    {
                        item.IncreaseDecrease = ((newPrice - oldPrice) / oldPrice) * 100;
                        item.IncreaseDecreaseText = "Increase";
                    }
                    else
                    {
                        item.IncreaseDecrease = ((oldPrice - newPrice) / oldPrice) * 100;
                        item.IncreaseDecreaseText = "Decrease";
                    }
                }
            }

            return Ok(new ApiResult<List<CompanyResponse>>(companies));
        }
    }
}
