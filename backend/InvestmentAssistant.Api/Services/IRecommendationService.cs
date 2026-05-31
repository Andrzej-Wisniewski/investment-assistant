using InvestmentAssistant.Api.Models.Responses;

namespace InvestmentAssistant.Api.Services;

public interface IRecommendationService
{
    Task<RecommendationResponse?> GetRecommendationAsync(string symbol);
}