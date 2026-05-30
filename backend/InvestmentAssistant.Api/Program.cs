using InvestmentAssistant.Api.Services;
using InvestmentAssistant.Api.Repositories;
using InvestmentAssistant.Api.Infrastructure.Database;
using InvestmentAssistant.Api.Infrastructure.Cache;
using InvestmentAssistant.Api.Infrastructure.BackgroundServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// === Konfiguracja bazy danych ===
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// === Konfiguracja Redis ===
var redisConnectionString = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
var redis = ConnectionMultiplexer.Connect(redisConnectionString);
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
builder.Services.AddScoped<ICacheService, RedisCacheService>();

// === Konfiguracja JWT ===
var jwtSecret = builder.Configuration["Jwt:SecretKey"];
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret!));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true
        };
    });

// === Konfiguracja HttpClient ===
builder.Services.AddHttpClient("AlphaVantage", client =>
{
    client.Timeout = TimeSpan.FromSeconds(15);
});

// === Rejestracja usług ===
builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAlphaVantageService, AlphaVantageService>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IStockDataCollectorService, StockDataCollectorService>();

// === BackgroundService ===
builder.Services.AddHostedService<StockDataCollectorBackgroundService>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();