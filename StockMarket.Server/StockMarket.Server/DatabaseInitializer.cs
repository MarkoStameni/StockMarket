using Microsoft.EntityFrameworkCore;

namespace StockMarket.Server
{
    public class DatabaseInitializer 
    {
        public static void ConfigureDatabase(IConfiguration config, DbContextOptionsBuilder options)
        {
            options.UseSqlServer(config.GetConnectionString("DefaultConnection"),
                options =>
                {
                    options.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                    options.EnableRetryOnFailure();
                }
            );
        }
    }
}
