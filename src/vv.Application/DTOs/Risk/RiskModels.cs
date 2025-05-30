using System;
using System.Collections.Generic;

namespace vv.Application.DTOs.Risk
{
    public class RiskReportDto
    {
        public DateTime GeneratedAt { get; set; }
        public required string PoolId { get; set; }
        public Dictionary<string, decimal> RiskExposures { get; set; } = new(); // Risk type → exposure
        public Dictionary<string, decimal> CounterpartyExposure { get; set; } = new(); // Entity → exposure %
        public Dictionary<string, decimal> AssetConcentration { get; set; } = new(); // Asset → concentration %
        public Dictionary<string, decimal> GeographicExposure { get; set; } = new(); // Region → exposure %
        public List<RiskAlertDto> ActiveAlerts { get; set; } = new();
        public decimal VaR95 { get; set; } // 95% Value at Risk
        public decimal VaR99 { get; set; } // 99% Value at Risk
        public decimal ExpectedShortfall { get; set; }
        public decimal StressTestWorstCase { get; set; } // % loss in worst case scenario
        public List<ScenarioAnalysisDto> ScenarioAnalyses { get; set; } = new();
        public decimal LiquidityRiskScore { get; set; } // 1-10
        public decimal MarketRiskScore { get; set; } // 1-10
        public decimal CreditRiskScore { get; set; } // 1-10
        public decimal OperationalRiskScore { get; set; } // 1-10
        public decimal ComplianceRiskScore { get; set; } // 1-10
        public decimal AggregateRiskScore { get; set; } // 1-10
    }

    public class RiskAlertDto
    {
        public required string AlertId { get; set; }
        public required string Severity { get; set; } // High, Medium, Low
        public required string Category { get; set; }
        public required string Description { get; set; }
        public DateTime Detected { get; set; }
        public bool IsResolved { get; set; }
        public required string AssignedTo { get; set; }
        public List<string> AffectedAssets { get; set; } = new();
        public decimal PotentialImpactUsd { get; set; }
        public required string RecommendedAction { get; set; }
        public int EscalationLevel { get; set; } // 1-3
    }

    public class ScenarioAnalysisDto
    {
        public required string ScenarioId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public Dictionary<string, decimal> AssetPriceChanges { get; set; } = new();
        public decimal PoolImpactPercent { get; set; }
        public decimal VaultImpactValue { get; set; }
        public List<string> MitigationStrategies { get; set; } = new();
        public required string Likelihood { get; set; } // Low, Medium, High
        public DateTime LastRun { get; set; }
    }

    public class RiskThresholdDto
    {
        public required string ThresholdId { get; set; }
        public required string Name { get; set; }
        public required string RiskCategory { get; set; } // Market, Credit, Liquidity, etc.
        public required string MetricName { get; set; }
        public required string Condition { get; set; } // >, <, =, >=, <=
        public decimal ThresholdValue { get; set; }
        public required string Severity { get; set; } // Warning, Critical, Emergency
        public bool IsActive { get; set; }
        public required string NotificationGroup { get; set; }
        public List<string> Recipients { get; set; } = new();
        public int CooldownMinutes { get; set; } // Minimum time between notifications
        public DateTime? LastTriggeredAt { get; set; }
        public int TriggerCount { get; set; }
    }

    public class CorrelationMatrixDto
    {
        public DateTime GeneratedAt { get; set; }
        public List<string> Assets { get; set; } = new();
        public List<List<decimal>> CorrelationValues { get; set; } = new();
        public int TimeframeInDays { get; set; }
        public List<HighCorrelationPairDto> SignificantCorrelations { get; set; } = new();
    }

    public class HighCorrelationPairDto
    {
        public required string Asset1 { get; set; }
        public required string Asset2 { get; set; }
        public decimal CorrelationCoefficient { get; set; }
        public required string Interpretation { get; set; } // "Strong Positive", "Moderate Negative", etc.
        public bool IsRiskFactor { get; set; }
    }


}