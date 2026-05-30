using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InvestmentAssistant.Api.Services;

namespace InvestmentAssistant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;

    public StockController(IStockService stockService)
    {
        _stockService = stockService;
    }

    /// <summary>
    /// Pobiera dane cenowe dla akcji
    /// </summary>
    [HttpGet("{symbol}")]
    public async Task<IActionResult> GetStock(string symbol)
    {
        var stock = await _stockService.GetStockDataAsync(symbol);

        if (stock is null)
        {
            return NotFound(new { message = $"Nie mogę pobrać danych dla {symbol}" });
        }

        return Ok(stock);
    }
}