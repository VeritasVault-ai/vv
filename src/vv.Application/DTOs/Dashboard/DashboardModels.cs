using System;
using System.Collections.Generic;

namespace vv.Application.DTOs.Dashboard
{
    public class CorporateDashboardSummaryDto
    {
        public string ClientId { get; set; }
        public string CompanyName { get; set; }
        public decimal TotalAssetValueUsd { get; set; }
        public decimal TotalProfitLossUsd { get; set; }
        public decimal ProfitLossPercentage { get; set; }
        public decimal WeightedAverageYield { get; set; }
        public List<DashboardVaultDto> Vaults { get; set; } = new();
        public List<DashboardAlertDto> RecentAlerts { get; set; } = new();
        public Dictionary<string, decimal> AssetAllocation { get; set; } = new();
        public decimal CurrentRiskScore { get; set; } // 1-100
        public List<DashboardUpcomingEventDto> UpcomingEvents { get; set; } = new();
        public decimal WithdrawalAvailable { get; set; }
        public decimal PendingDeposits { get; set; }
        public decimal PendingWithdrawals { get; set; }
        public Dictionary<string, decimal> TreasuryMetrics { get; set; } = new();
        public DateTime LastLogin { get; set; }
        public List<UserActivityDto> RecentActivity { get; set; } = new();
    }

    public class DashboardVaultDto
    {
        public string VaultId { get; set; }
        public string Name { get; set; }
        public string RiskCategory { get; set; }
        public decimal AllocationPercentage { get; set; }
        public decimal CurrentValueUsd { get; set; }
        public decimal ProfitLossUsd { get; set; }
        public decimal ProfitLossPercentage { get; set; }
        public decimal CurrentYield { get; set; }
        public decimal UtilizationRate { get; set; }
        public string Status { get; set; }
        public Dictionary<string, decimal> TopHoldings { get; set; } = new(); // Top 5 assets
        public List<PerformanceSnapshotDto> Performance { get; set; } = new(); // Daily for last 30 days
    }

    public class DashboardAlertDto
    {
        public string AlertId { get; set; }
        public string Type { get; set; } // Risk, Performance, Compliance, System
        public string Severity { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
        public bool RequiresAction { get; set; }
        public string ActionUrl { get; set; }
    }

    public class DashboardUpcomingEventDto
    {
        public string EventId { get; set; }
        public string Type { get; set; } // Rebalance, Report, Regulatory Filing, etc.
        public string Title { get; set; }
        public DateTime ScheduledFor { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public bool RequiresApproval { get; set; }
    }

    public class UserActivityDto
    {
        public string ActivityId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; } // Login, Withdrawal Request, Setting Change, etc.
        public DateTime Timestamp { get; set; }
        public string IpAddress { get; set; }
        public string Device { get; set; }
        public string Location { get; set; }
        public bool IsSuccessful { get; set; }
    }

    public class PerformanceSnapshotDto
    {
        public DateTime Date { get; set; }
        public decimal ValueUsd { get; set; }
        public decimal DailyChangePercent { get; set; }
        public decimal YieldToDate { get; set; }
    }

    public class AdminDashboardSummaryDto
    {
        public int TotalCorporateClients { get; set; }
        public int ActiveClients { get; set; }
        public int PendingOnboardings { get; set; }
        public decimal TotalAssetsUnderManagementUsd { get; set; }
        public decimal TotalRevenueCurrentMonthUsd { get; set; }
        public decimal TotalFeesCollectedYtdUsd { get; set; }
        public List<DashboardPoolSummaryDto> TopPerformingPools { get; set; } = new();
        public List<DashboardAlertDto> SystemAlerts { get; set; } = new();
        public Dictionary<string, int> ClientsByRegion { get; set; } = new();
        public Dictionary<string, decimal> AssetDistribution { get; set; } = new();
        public List<DashboardMetricDto> KeyMetricsTrend { get; set; } = new(); // AUM, Revenue, Client Count
    }

    public class DashboardPoolSummaryDto
    {
        public string PoolId { get; set; }
        public string Name { get; set; }
        public string RiskCategory { get; set; }
        public decimal TotalValueUsd { get; set; }
        public decimal ReturnCurrentMonth { get; set; }
        public decimal ReturnYtd { get; set; }
        public int ClientCount { get; set; }
        public decimal UtilizationRate { get; set; }
    }

    public class DashboardMetricDto
    {
        public DateTime Date { get; set; }
        public string MetricName { get; set; }
        public decimal Value { get; set; }
        public decimal ChangeFromPrevious { get; set; }
    }
}