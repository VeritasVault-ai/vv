using System;
using System.Collections.Generic;

namespace vv.Application.DTOs.Portfolio.BlackLitterman
{
    public class BlackLittermanInputDto
    {
        public string ModelId { get; set; }
        public string PortfolioId { get; set; }
        public Dictionary<string, decimal> MarketCapitalizationWeights { get; set; } = new();
        public Dictionary<string, decimal> EquilibriumReturns { get; set; } = new();
        public List<List<decimal>> CovarianceMatrix { get; set; } = new();
        public List<string> Assets { get; set; } = new();
        public List<InvestorViewDto> Views { get; set; } = new();
        public decimal ConfidenceInPrior { get; set; } // Tau parameter, typically 0.01-0.05
        public decimal RiskAversionCoefficient { get; set; } // Lambda parameter
        public List<BlackLittermanConstraintDto> Constraints { get; set; } = new();
        public Dictionary<string, decimal> CurrentHoldings { get; set; } = new();
        public decimal RiskFreeRate { get; set; }
    }

    public class InvestorViewDto
    {
        public string ViewId { get; set; }
        public string Description { get; set; }
        public ViewType Type { get; set; } // Absolute, Relative
        public List<ViewAssetWeightDto> AssetWeights { get; set; } = new();
        public decimal ExpectedReturn { get; set; }
        public decimal Confidence { get; set; } // 0-1, higher means more confident
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string Rationale { get; set; }
        public List<string> SupportingEvidence { get; set; } = new();
        public DateTime? ExpiryDate { get; set; }
    }

    public class ViewAssetWeightDto
    {
        public string Asset { get; set; }
        public decimal Weight { get; set; }
    }

    public enum ViewType
    {
        Absolute, // View on a single asset
        Relative  // View comparing assets (e.g., A will outperform B)
    }

    public class BlackLittermanConstraintDto
    {
        public string ConstraintId { get; set; }
        public string Type { get; set; }
        public Dictionary<string, decimal> AssetWeights { get; set; } = new();
        public decimal LowerBound { get; set; }
        public decimal UpperBound { get; set; }
        public string Description { get; set; }
    }

    public class BlackLittermanResultDto
    {
        public string ResultId { get; set; }
        public string ModelId { get; set; }
        public DateTime GeneratedAt { get; set; }
        public Dictionary<string, decimal> PosteriorExpectedReturns { get; set; } = new();
        public List<List<decimal>> PosteriorCovariance { get; set; } = new();
        public Dictionary<string, decimal> OptimalWeights { get; set; } = new();
        public Dictionary<string, decimal> ImpliedConfidenceLevel { get; set; } = new();
        public decimal ImpliedRiskAversion { get; set; }
        public PortfolioMetricsDto ResultingPortfolioMetrics { get; set; }
        public List<ViewImpactDto> ViewsImpact { get; set; } = new();
        public Dictionary<string, decimal> TradeRecommendations { get; set; } = new();
        public decimal TiltMagnitude { get; set; } // How much the views changed the portfolio
    }

    public class ViewImpactDto
    {
        public string ViewId { get; set; }
        public decimal ImpactScore { get; set; } // -1 to 1
        public Dictionary<string, decimal> WeightChanges { get; set; } = new();
        public decimal ReturnContribution { get; set; }
        public decimal RiskContribution { get; set; }
    }
}