using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using StockMarket.Database.SqlServer;
using StockMarket.Server.Helpers;
using StockMarket.Server.Services.Interfaces;
using StockMarket.Server.Services;
using StockMarket.Server;
using System.Text.Json.Serialization;
using StockMarket.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddDbContext<DatabaseContext>((options) => DatabaseInitializer.ConfigureDatabase(builder.Configuration, options), contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Singleton);
builder.Services.AddDbContextFactory<DatabaseContext>((options) => DatabaseInitializer.ConfigureDatabase(builder.Configuration, options));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
        options => builder.Configuration.Bind("JwtSettings", options));

var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperProfile());
});

var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddTransient<IJwtUtils, JwtUtils>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IBuyingSellingShareService, BuyingSellingShareService>();
builder.Services.AddScoped<ICompanyUserService, CompanyUserService>();
builder.Services.AddScoped<TypeTactics>();

var _logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext().CreateLogger();
builder.Logging.AddSerilog(_logger);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors(x => x
      .SetIsOriginAllowed(origin => true)
      .AllowAnyMethod()
      .AllowAnyHeader()
      .AllowCredentials());
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<DataHub>("/dataHub");
});

app.Logger.LogInformation("Starting the app");
await app.RunAsync();
