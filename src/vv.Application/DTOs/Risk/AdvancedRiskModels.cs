using System;
using System.Collections.Generic;

namespace vv.Application.DTOs.Risk.Advanced
{
    public class StressTestDto
    {
        public required string TestId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string CreatedBy { get; set; }
        public List<StressScenarioDto> Scenarios { get; set; } = new();
        public List<string> PortfolioIds { get; set; } = new();
        public List<StressTestResultDto> Results { get; set; } = new();
        public bool IsHistorical { get; set; }
        public bool IsSystematic { get; set; }
        public required string Severity { get; set; } // Mild, Moderate, Severe
        public decimal ConfidenceLevel { get; set; } // 0-1
    }

    public class StressScenarioDto
    {
        public required string ScenarioId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public Dictionary<string, decimal> AssetShocks { get; set; } = new(); // Asset → % change
        public Dictionary<string, decimal> FactorShocks { get; set; } = new(); // Factor → % change
        public Dictionary<string, decimal> MarketVariableShocks { get; set; } = new(); // Variable → % change
        public required string HistoricalPeriod { get; set; } // If based on historical period
        public DateTime? ReferenceStartDate { get; set; }
        public DateTime? ReferenceEndDate { get; set; }
        public decimal Probability { get; set; } // Estimated probability of occurrence
    }

    public class StressTestResultDto
    {
        public required string ResultId { get; set; }
        public required string TestId { get; set; }
        public required string PortfolioId { get; set; }
        public DateTime RunAt { get; set; }
        public Dictionary<string, decimal> PortfolioValueChanges { get; set; } = new(); // Scenario → % change
        public Dictionary<string, decimal> AssetContributions { get; set; } = new(); // Asset → contribution
        public decimal MaxLoss { get; set; }
        public required string WorstScenario { get; set; }
        public decimal AvgLoss { get; set; }
        public List<RiskBreachDto> RiskBreaches { get; set; } = new();
        public Dictionary<string, List<decimal>> LossDistribution { get; set; } = new(); // Histogram data
        public decimal LiquidityImpact { get; set; } // Additional loss due to liquidity constraints
    }

    public class RiskBreachDto
    {
        public required string BreachId { get; set; }
        public required string RiskMetric { get; set; } // VaR, MaxDrawdown, etc.
        public decimal ThresholdValue { get; set; }
        public decimal ActualValue { get; set; }
        public decimal BreachMagnitude { get; set; } // % beyond threshold
        public required string Severity { get; set; } // Low, Medium, High, Critical
        public required string ScenarioId { get; set; }
        public List<string> AffectedAssets { get; set; } = new();
        public List<string> RecommendedActions { get; set; } = new();
    }

    public class ConditionalValueAtRiskDto
    {
        public required string CalculationId { get; set; }
        public required string PortfolioId { get; set; }
        public DateTime CalculatedAt { get; set; }
        public decimal ConfidenceLevel { get; set; } // Typically 0.95, 0.99
        public int TimeHorizonDays { get; set; }
        public decimal CVaRValue { get; set; }
        public decimal CVaRPercentage { get; set; }
        public Dictionary<string, decimal> ComponentCVaR { get; set; } = new(); // Asset → contribution
        public required string MethodologyUsed { get; set; } // Historical, Monte Carlo, Parametric
        public int SampleSize { get; set; }
        public Dictionary<string, decimal> TailRiskMetrics { get; set; } = new(); // Additional metrics
        public List<decimal> WorstLosses { get; set; } = new(); // The losses beyond VaR
    }

    public class FundamentalFactorModelDto
    {
        public required string ModelId { get; set; }
        public required string Name { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string> Factors { get; set; } = new();
        public Dictionary<string, List<decimal>> FactorExposures { get; set; } = new(); // Asset → exposures
        public List<List<decimal>> FactorCovariance { get; set; } = new();
        public Dictionary<string, decimal> FactorReturns { get; set; } = new();
        public Dictionary<string, decimal> SpecificRisks { get; set; } = new(); // Asset → specific risk
        public Dictionary<string, decimal> R2Values { get; set; } = new(); // Asset → R^2
        public int EstimationWindow { get; set; } // In days
        public required string FactorType { get; set; } // Macro, Style, Industry
    }

    public class LiquidityRiskDto
    {
        public required string AnalysisId { get; set; }
        public required string PortfolioId { get; set; }
        public DateTime AnalyzedAt { get; set; }
        public Dictionary<string, decimal> LiquidationTimeByAsset { get; set; } = new(); // Asset → days
        public Dictionary<string, decimal> LiquidationCostByAsset { get; set; } = new(); // Asset → % cost
        public decimal PortfolioLiquidationTime95 { get; set; } // Days to liquidate 95%
        public decimal PortfolioLiquidationCost { get; set; } // % cost to liquidate
        public Dictionary<string, decimal> BidAskSpreadsByAsset { get; set; } = new();
        public Dictionary<string, decimal> MarketDepthByAsset { get; set; } = new();
        public decimal ConcentrationRisk { get; set; } // Measure of concentration in illiquid assets
        public List<LiquidityTierDto> LiquidityTiers { get; set; } = new();
        public Dictionary<string, decimal> RedemptionCoverage { get; set; } = new(); // Days → % coverage
        public decimal StressedLiquidityCost { get; set; } // Cost under stressed conditions
    }

    public class LiquidityTierDto
    {
        public required string TierId { get; set; }
        public required string Name { get; set; } // Ultra Liquid, Liquid, Semi-liquid, Illiquid
        public int MaxLiquidationDays { get; set; }
        public decimal PortfolioPercentage { get; set; }
        public List<string> Assets { get; set; } = new();
        public decimal AverageImpactCost { get; set; }
    }
}