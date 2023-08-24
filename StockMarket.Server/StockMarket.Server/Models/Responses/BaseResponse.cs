namespace StockMarket.Server.Models.Response
{
    public class BaseResponse
    {
        public string Message { get; set; } = string.Empty;
        public int? Id { get; set; }
    }
}
