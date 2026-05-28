using InvestmentAssistant.Api.Services;
using InvestmentAssistant.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using InvestmentAssistant.Api.Infrastructure.Database; 

var builder = WebApplication.CreateBuilder(args);


// === Konfiguracja bazy danych ===
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// === Rejestracja usług ===
builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddOpenApi();

var app = builder.Build();

// === Konfiguracja middleware ===
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
