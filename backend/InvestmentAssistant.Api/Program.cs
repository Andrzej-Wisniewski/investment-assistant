using InvestmentAssistant.Api.Services;
using InvestmentAssistant.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// === Rejestracja usług ===
builder.Services.AddControllers();

// Rejestracja usług i repozytoriów
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
