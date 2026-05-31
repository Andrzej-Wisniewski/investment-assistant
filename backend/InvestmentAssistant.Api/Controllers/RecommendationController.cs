using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InvestmentAssistant.Api.Services;

namespace InvestmentAssistant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RecommendationController : ControllerBase
{
    private readonly IRecommendationService _recommendationService;
    private readonly ILogger<RecommendationController> _logger;

    public RecommendationController(
        IRecommendationService recommendationService,
        ILogger<RecommendationController> logger)
    {
        _recommendationService = recommendationService;
        _logger = logger;
    }

    /// <summary>
    /// Pobiera rekomendację inwestycyjną na podstawie analizy technicznej
    /// </summary>
    [HttpGet("{symbol}")]
    public async Task<IActionResult> GetRecommendation(string symbol)
    {
        var recommendation = await _recommendationService.GetRecommendationAsync(symbol);

        if (recommendation is null)
        {
            return NotFound(new { message = $"Nie mogę wygenerować rekomendacji dla {symbol}" });
        }

        return Ok(recommendation);
    }
}