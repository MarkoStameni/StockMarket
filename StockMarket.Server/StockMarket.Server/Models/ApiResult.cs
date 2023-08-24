namespace StockMarket.Server.Models
{
    public class ApiResult
    {
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public ApiResult() { }
        public ApiResult(string? errorCode, string? errorMessage) 
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }

    public class ApiResult<T> : ApiResult
    {
        public T? Data { get; set; }
        public ApiResult() { }
        public ApiResult(T data)
        {
            Data = data;
        }

        public ApiResult(string? errorCode, string? errorMessage) : base(errorCode, errorMessage)
        {
        }
    }
}
