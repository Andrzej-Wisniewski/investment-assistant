using Microsoft.AspNetCore.Mvc;
using InvestmentAssistant.Api.Services;

namespace InvestmentAssistant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly IStockDataCollectorService _collectorService;

    public HealthController(IStockDataCollectorService collectorService)
    {
        _collectorService = collectorService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow,
            service = "InvestmentAssistant.Api",
            backgroundServices = new
            {
                lastStockDataUpdate = _collectorService.LastSuccessfulUpdate
            }
        });
    }
}