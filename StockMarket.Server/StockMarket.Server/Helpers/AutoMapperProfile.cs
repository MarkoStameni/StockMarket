using AutoMapper;
using SstockMarket.Server.Models.Responses;
using StockMarket.Database.SqlServer.Models;
using StockMarket.Server.Models.Responses;

namespace StockMarket.Server.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<User, UserResponse>().ReverseMap();
            CreateMap<Company, CompanyResponse>().ReverseMap();
            CreateMap<BuyingSellingShare, BuyingSellingShareResponse>().ReverseMap();
        }
    }
}
