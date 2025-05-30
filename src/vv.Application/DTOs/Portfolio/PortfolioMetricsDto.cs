using System;
using System.Collections.Generic;

namespace vv.Application.DTOs.Portfolio
{
    public class PortfolioMetricsDto
    {
        public string PortfolioId { get; set; } = string.Empty;

        public decimal TotalValue { get; set; }

        public decimal Volatility { get; set; }

        public decimal SharpeRatio { get; set; }

        public decimal MaxDrawdown { get; set; }

        public decimal ValueAtRisk { get; set; }

        public decimal ExpectedReturn { get; set; }

        public Dictionary<string, decimal> AssetAllocation { get; set; } = new Dictionary<string, decimal>();

        public DateTime CalculatedAt { get; set; }
    }
}
