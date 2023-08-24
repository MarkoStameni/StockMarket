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

            return Ok(new ApiResult<List<CompanyResponse>>(companies));
        }
    }
}
