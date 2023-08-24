namespace StockMarket.Identity.Server.Requests
{
    public abstract class PaginatedRequestModel<T>
    {
        protected abstract string DefaultOrderColumn { get; }
        protected abstract string DefaultOrderDirection { get; }

        public int Page { get; set; } = 1;
        public int RecordCount { get; set; } = 10;
        public T? Filter { get; set; }
        public string? OrderColumn { get; set; }
        public string? OrderDirection { get; set; }

        public string GetOrderColumn()
        {
            return !string.IsNullOrEmpty(OrderColumn) ? OrderColumn : DefaultOrderColumn;
        }

        public string GetOrderDirection()
        {
            return !string.IsNullOrEmpty(OrderDirection) ? OrderDirection : DefaultOrderDirection;
        }
    }
}
