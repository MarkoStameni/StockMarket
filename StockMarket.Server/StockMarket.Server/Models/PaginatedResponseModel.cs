namespace StockMarket.Server.Models
{
    public class PaginatedResponseModel<T>
    {
        public T? Items { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
    }
}
