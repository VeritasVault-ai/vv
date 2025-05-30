# Performance Module

> Tools for measuring, analyzing, and attributing investment performance

---

## Overview

The Performance Module provides comprehensive capabilities for calculating, analyzing, and attributing investment performance. It enables accurate measurement of returns, comparison against benchmarks, and detailed understanding of the sources of performance across different dimensions.

## Core Responsibilities

### Return Calculation

* **Total Return Calculation**: Computing time-weighted and money-weighted returns
* **Multi-Period Returns**: Calculating returns over various time periods
* **Gross and Net Returns**: Computing returns before and after fees
* **Local and Base Currency Returns**: Handling currency translation
* **Annualized and Cumulative Returns**: Standardized return presentation

### Performance Analysis

* **Benchmark Comparison**: Comparing returns against relevant benchmarks
* **Peer Group Analysis**: Evaluating performance relative to peers
* **Rolling Return Analysis**: Analyzing returns over rolling periods
* **Drawdown Analysis**: Measuring and analyzing periods of decline
* **Return Distribution Analysis**: Statistical analysis of return patterns

### Performance Attribution

* **Allocation Attribution**: Measuring impact of allocation decisions
* **Selection Attribution**: Evaluating security selection decisions
* **Factor-Based Attribution**: Linking performance to factor exposures
* **Multi-Period Attribution**: Attributing performance across time periods
* **Ex-Ante vs. Ex-Post Attribution**: Comparing expected and realized attribution

### Risk-Adjusted Performance

* **Risk-Adjusted Metrics**: Calculating Sharpe, Information Ratio, etc.
* **Risk Contribution Analysis**: Linking risk contribution to performance
* **Alpha Measurement**: Isolating skill-based performance
* **Market Condition Analysis**: Performance across different market regimes
* **Scenario-Based Performance**: Evaluating performance under scenarios

## Technical Architecture

### Data Model

```
┌─────────────────┐       ┌─────────────────┐       ┌─────────────────┐
│   Performance   │       │  Return Series  │       │   Benchmark     │
│    Measure      │       │                 │       │                 │
│ - Measure ID    │◄─────►│ - Series ID     │◄─────►│ - Benchmark ID  │
│ - Type          │       │ - Portfolio ID  │       │ - Name          │
│ - Parameters    │       │ - Period Type   │       │ - Type          │
│ - Description   │       │ - Return Type   │       │ - Return Series │
│                 │       │ - Currency      │       │ - Composition   │
└────────┬────────┘       └─────────────────┘       └─────────────────┘
         │                                                    ▲
         │                                                    │
         ▼                                                    │
┌─────────────────┐       ┌─────────────────┐       ┌────────┴────────┐
│   Attribution   │       │  Performance    │       │  Risk-Adjusted  │
│    Analysis     │       │    Report       │       │   Performance   │
│ - Analysis ID   │◄─────►│ - Report ID     │◄─────►│ - Analysis ID   │
│ - Portfolio ID  │       │ - Portfolio ID  │       │ - Portfolio ID  │
│ - Model ID      │       │ - Time Period   │       │ - Risk Measures │
│ - Time Period   │       │ - Measures      │       │ - Adjustments   │
│ - Dimensions    │       │ - Comparisons   │       │ - Metrics       │
└─────────────────┘       └─────────────────┘       └─────────────────┘
```

### Component Architecture

* **Return Calculator**: Computes various types of returns
* **Benchmark Manager**: Handles benchmark data and comparisons
* **Attribution Engine**: Performs attribution analysis
* **Risk-Adjusted Metrics Engine**: Calculates risk-adjusted performance
* **Performance Repository**: Stores performance data and results
* **Performance Reporter**: Generates performance reports
* **Analysis Tools**: Provides interactive performance analysis

## API Interfaces

### Core API Operations

* **Return Calculation**: Calculate returns for portfolios and benchmarks
* **Performance Comparison**: Compare portfolio performance to benchmarks
* **Attribution Analysis**: Perform attribution analysis across dimensions
* **Risk-Adjusted Metrics**: Calculate risk-adjusted performance metrics
* **Performance Reporting**: Generate performance reports
* **Time-Series Analysis**: Analyze performance over time
* **Contribution Analysis**: Calculate contribution to performance

### API Examples

```json
// Example: Return Calculation Request
POST /performance/returns/calculate
{
  "portfolioId": "PORTFOLIO123",
  "returnType": "TimeWeighted",
  "currency": "USD",
  "grossOfFees": true,
  "periods": ["Daily", "Monthly", "Quarterly", "YTD", "1Y", "3Y", "5Y"],
  "startDate": "2018-01-01",
  "endDate": "2023-12-31"
}

// Example: Attribution Analysis Request
POST /performance/attribution/analyze
{
  "portfolioId": "PORTFOLIO123",
  "benchmarkId": "BENCHMARK001",
  "model": "BrinsonFachler",
  "dimensions": ["AssetClass", "Sector", "Geography"],
  "periods": ["1M", "3M", "1Y"],
  "endDate": "2023-12-31"
}
```

## Performance Measurement Framework

### Return Methodologies

* **Time-Weighted Return (TWR)**: Eliminates impact of cash flows
* **Money-Weighted Return (MWR)**: Accounts for timing of cash flows
* **Modified Dietz Method**: Approximation of TWR
* **Linked Returns**: Compounding period returns
* **Unit Value Method**: NAV-based return calculation

### Performance Comparison Methods

* **Excess Return**: Simple difference from benchmark
* **Relative Return**: Percentage difference from benchmark
* **Up/Down Capture**: Performance in rising vs. falling markets
* **Win/Loss Ratio**: Frequency of outperformance
* **Tracking Analysis**: Consistency of relative performance

### Peer Group Analysis

* **Percentile Rankings**: Relative position within peer group
* **Quartile Analysis**: Performance by quartile over time
* **Peer-Relative Metrics**: Custom metrics versus peers
* **Style-Adjusted Comparison**: Adjusting for style differences
* **Universe Construction**: Building appropriate peer groups

## Attribution Methodologies

### Asset Allocation Models

* **Brinson-Hood-Beebower**: Classic three-factor model
* **Brinson-Fachler**: Modified two-factor model
* **Multi-Factor Allocation**: Extended allocation models
* **Arithmetic vs. Geometric**: Different linking approaches
* **Smoothing Techniques**: Handling attribution noise

### Factor-Based Attribution

* **CAPM-Based**: Single-factor attribution
* **Multi-Factor Models**: Attribution to multiple risk factors
* **Returns-Based Style Analysis**: Attribution using returns
* **Holdings-Based Factor Models**: Attribution using characteristics
* **Custom Factor Models**: Specialized factor attribution

### Fixed Income Attribution

* **Yield Curve Attribution**: Effects of yield curve changes
* **Spread Attribution**: Effects of spread changes
* **Roll-Down Attribution**: Effects of yield curve roll-down
* **Carry Attribution**: Effects of yield income
* **Currency Attribution**: Effects of currency movements

## Performance Reporting

### Report Types

* **Executive Summary**: High-level performance overview
* **Detailed Performance**: Comprehensive performance metrics
* **Attribution Reports**: Detailed attribution analysis
* **Risk-Adjusted Analysis**: Risk-adjusted performance metrics
* **Composite Reports**: Aggregated performance across portfolios

### Reporting Standards

* **GIPS Compliance**: Global Investment Performance Standards
* **CFA Institute Standards**: Ethical and professional standards
* **Regulatory Requirements**: SEC, FINRA, etc.
* **Client-Specific Standards**: Customized reporting requirements
* **Internal Standards**: Organizational reporting policies

### Visualization Techniques

* **Performance Charts**: Visual representation of returns
* **Attribution Heat Maps**: Color-coded attribution effects
* **Risk-Return Plots**: Visualizing risk-adjusted performance
* **Rolling Analysis**: Performance over rolling periods
* **Interactive Dashboards**: Dynamic performance exploration

## VeritasVault Implementation

VeritasVault implements the Performance Module with these components:

* **Return Engine**: Advanced return calculation capabilities
* **Attribution System**: Comprehensive attribution functionality
* **Benchmark Database**: Extensive benchmark data
* **Analytics Suite**: Powerful performance analytics
* **Reporting Framework**: Flexible reporting capabilities
* **Integration Layer**: Connections to other system modules
* **Interactive Dashboard**: Visual performance analysis

## Related Documentation

* [Core Modules](../core-modules.md) - Overview of all core modules
* [Performance Attribution Overview](../../factor-models/attribution/performance-attribution-overview.md) - Detailed attribution methodologies
* [Returns-Based Attribution](../../factor-models/attribution/returns-based-attribution.md) - Returns-based attribution approaches
* [Holdings-Based Attribution](../../factor-models/attribution/holdings-based-attribution.md) - Holdings-based attribution techniques
* [Factor Attribution](../../factor-models/attribution/factor-attribution-overview.md) - Factor-based performance attribution