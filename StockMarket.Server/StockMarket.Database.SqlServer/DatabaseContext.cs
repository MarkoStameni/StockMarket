using StockMarket.Database.SqlServer.Models;
using Microsoft.EntityFrameworkCore;

namespace StockMarket.Database.SqlServer
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyUser>().HasKey(cu => new { cu.UserId, cu.CompanyId});

            modelBuilder.Entity<CompanyUser>()
                 .HasOne<Company>(sc => sc.Company)
                 .WithMany(s => s.CompanyUsers)
                 .HasForeignKey(sc => sc.CompanyId);

            modelBuilder.Entity<CompanyUser>()
                .HasOne<User>(sc => sc.User)
                .WithMany(s => s.CompanyUsers)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<Company>().HasData(new Company
            {
                Id = 1,
                Name = "AAPL",
                OpenPrice = 100,
                ClosePrice = null,
                HighPrice = null,
                LowPrice = null,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Share = 150
            });

            modelBuilder.Entity<Company>().HasData(new Company
            {
                Id = 2,
                Name = "META",
                OpenPrice = 234,
                ClosePrice = null,
                HighPrice = null,
                LowPrice = null,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Share = 186
            });

            modelBuilder.Entity<Company>().HasData(new Company
            {
                Id = 3,
                Name = "MSFT",
                OpenPrice = 88,
                ClosePrice = null,
                HighPrice = null,
                LowPrice = null,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Share = 109
            });

            modelBuilder.Entity<Company>().HasData(new Company
            {
                Id = 4,
                Name = "GOOGL",
                OpenPrice = 321,
                ClosePrice = null,
                HighPrice = null,
                LowPrice = null,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Share = 199
            });

            modelBuilder.Entity<Company>().HasData(new Company
            {
                Id = 5,
                Name = "AMZN",
                OpenPrice = 118,
                ClosePrice = null,
                HighPrice = null,
                LowPrice = null,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Share = 155
            });

            modelBuilder.Entity<BuyingSellingShare>().HasData(new BuyingSellingShare
            {
                Id = 1,
                CompanyId = 1,
                Price = 102,
                DateCreated = DateTime.Now.AddMinutes(2),
                DateUpdated = DateTime.Now,
            });

            modelBuilder.Entity<BuyingSellingShare>().HasData(new BuyingSellingShare
            {
                Id = 2,
                CompanyId = 1,
                Price = 107,
                DateCreated = DateTime.Now.AddMinutes(3),
                DateUpdated = DateTime.Now,
            });

            modelBuilder.Entity<BuyingSellingShare>().HasData(new BuyingSellingShare
            {
                Id = 3,
                CompanyId = 1,
                Price = 103.5M,
                DateCreated = DateTime.Now.AddMinutes(8),
                DateUpdated = DateTime.Now,
            });
        }
        
        public DbSet<BuyingSellingShare> BuyingSelingShares { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CompanyUser> CompanyUsers { get; set; }
    }
}
