namespace StockMarket.Server.Models
{
    public static class ErrorCodes
    {
        //General
        public const string NotFound = "NOT_FOUND";
        public const string InvalidParameters = "INVALID_PARAMETERS";
        public const string MissingParameters = "MISSING_PARAMETERS";
        public const string AlreadyExists = "ALREADY_EXISTS";
        public const string InternalError = "INTERNAL_ERROR";

        // Auth
        public const string Unauthorized = "UNAUTHORIZED";
    }
}
