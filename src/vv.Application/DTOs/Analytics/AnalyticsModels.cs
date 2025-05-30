using System;
using System.Collections.Generic;

namespace vv.Application.DTOs.Analytics
{
    public class MarketInsightsDto
    {
        public DateTime GeneratedAt { get; set; }
        public List<MarketTrendDto> Trends { get; set; } = new();
        public List<AssetCorrelationDto> Correlations { get; set; } = new();
        public List<MarketEventDto> UpcomingEvents { get; set; } = new();
        public Dictionary<string, decimal> SectorPerformance { get; set; } = new();
        public List<MarketOpportunityDto> Opportunities { get; set; } = new();
        public List<MarketRiskDto> Risks { get; set; } = new();
        public string MarketSentiment { get; set; } // Bullish, Bearish, Neutral
        public decimal FearGreedIndex { get; set; } // 0-100
        public Dictionary<string, decimal> MacroIndicators { get; set; } = new(); // Inflation, Interest rates, etc.
    }

    public class MarketTrendDto
    {
        public string TrendId { get; set; }
        public string AssetClass { get; set; } // Crypto, Equity, Commodity, etc.
        public string Description { get; set; }
        public string Direction { get; set; } // Up, Down, Sideways
        public int Strength { get; set; } // 1-10
        public int Duration { get; set; } // In days
        public decimal MagnitudePercent { get; set; }
        public List<string> AffectedAssets { get; set; } = new();
        public string Analysis { get; set; }
    }

    public class AssetCorrelationDto
    {
        public string Asset1 { get; set; }
        public string Asset2 { get; set; }
        public decimal CorrelationCoefficient { get; set; } // -1 to 1
        public int TimeframeInDays { get; set; }
        public bool IsSignificant { get; set; }
        public string Interpretation { get; set; }
        public decimal ConfidenceLevel { get; set; } // 0-1
    }

    public class MarketOpportunityDto
    {
        public string OpportunityId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } // Arbitrage, Yield, Value, etc.
        public decimal PotentialReturnPercent { get; set; }
        public string TimeHorizon { get; set; } // Short, Medium, Long
        public decimal RiskLevel { get; set; } // 1-10
        public decimal ConfidenceScore { get; set; } // 1-10
        public List<string> RelatedAssets { get; set; } = new();
        public DateTime ExpiryDate { get; set; } // When opportunity is expected to disappear
    }

    public class MarketEventDto
    {
        public string EventId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; } // Hard Fork, Mainnet Launch, Regulatory, Conference
        public DateTime ScheduledDate { get; set; }
        public List<string> AffectedAssets { get; set; } = new();
        public string Description { get; set; }
        public string Source { get; set; }
        public decimal ExpectedImpact { get; set; } // -10 to 10
        public decimal Certainty { get; set; } // 0-1
        public List<string> RelatedLinks { get; set; } = new();
    }

    public class MarketRiskDto
    {
        public string RiskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; } // Regulatory, Technical, Market, etc.
        public decimal Probability { get; set; } // 0-1
        public decimal Impact { get; set; } // 1-10
        public decimal RiskScore { get; set; } // Probability * Impact
        public List<string> AffectedAssets { get; set; } = new();
        public List<string> MitigationStrategies { get; set; } = new();
        public string Timeframe { get; set; } // Immediate, Short-term, Long-term
    }

    public class PredictiveModelResultDto
    {
        public string ModelId { get; set; }
        public string ModelName { get; set; }
        public string AssetSymbol { get; set; }
        public DateTime GeneratedAt { get; set; }
        public List<PricePredictionDto> Predictions { get; set; } = new();
        public decimal ConfidenceScore { get; set; } // 0-1
        public decimal HistoricalAccuracy { get; set; } // 0-1
        public List<FactorContributionDto> KeyFactors { get; set; } = new();
        public Dictionary<string, decimal> ScenarioProbabilities { get; set; } = new(); // Scenario → probability
    }

    public class PricePredictionDto
    {
        public DateTime TargetDate { get; set; }
        public decimal PredictedPrice { get; set; }
        public decimal UpperBound { get; set; }
        public decimal LowerBound { get; set; }
        public decimal ConfidenceInterval { get; set; } // 0-1
    }

    public class FactorContributionDto
    {
        public string FactorName { get; set; }
        public decimal Contribution { get; set; } // -1 to 1
        public string Direction { get; set; } // Positive, Negative
        public decimal Significance { get; set; } // 0-1
        public string Explanation { get; set; }
    }
}