---
document_type: guide
classification: internal
status: draft
version: 0.1.0
last_updated: '2025-05-31'
applies_to:
- Core
reviewers:
- '@tech-lead'
priority: p2
next_review: '2026-05-31'
---

# Single-Factor Models

> Portfolio construction targeting exposure to individual risk factors

---

## Overview

Single-factor models create portfolios with targeted exposure to one specific factor premium. These models provide the purest expression of factor investing and allow for precise control over specific risk exposures.

## Key Principles

Single-factor portfolios are constructed on these fundamental principles:

* **Factor Isolation**: Maximize exposure to the target factor while minimizing unintended exposures
* **Factor Purity**: Ensure the portfolio genuinely captures the intended risk premium
* **Factor Efficiency**: Capture the factor premium with minimal implementation costs
* **Factor Consistency**: Maintain stable exposure to the factor over time

## Implementation Methodologies

### Factor Scoring Approach

The most common implementation methodology follows these steps:

1. **Factor Definition**: Precisely define the factor and its measurement
2. **Asset Scoring**: Score each asset in the investment universe based on the factor
3. **Selection**: Select assets with the highest (or lowest) factor scores
4. **Weighting**: Weight selected assets based on chosen methodology:
   * Equal weighting
   * Factor score weighting
   * Factor score with risk adjustment
   * Optimization-based weighting

### Common Weighting Schemes

| Weighting Approach | Description | Advantages | Challenges |
|-------------------|-------------|------------|------------|
| Equal Weight | Equal allocation to all selected securities | Simple, transparent | No consideration for factor intensity or risk |
| Factor Score Weight | Weight proportional to factor score | Strong factor alignment | May lead to concentration risk |
| Score-Volatility Adjusted | Weight proportional to score/volatility | Balances factor exposure with risk | Requires accurate volatility forecasts |
| Optimization-Based | Maximize factor exposure subject to constraints | Comprehensive risk control | Complexity, estimation risk |

## Popular Single-Factor Models

### Value Factor

* **Definition**: Companies trading at a discount relative to fundamentals
* **Common Metrics**: P/B, P/E, EV/EBITDA, P/CF ratios
* **Implementation**: Select lowest P/B stocks, equal or inverse ratio weighting
* **Risk Profile**: May include distressed companies, sector biases
* **Performance Characteristics**: Cyclical, tends to outperform during economic recovery

### Momentum Factor

* **Definition**: Securities with positive price trends
* **Common Metrics**: 12-1 month price return (excluding most recent month)
* **Implementation**: Select highest momentum stocks, equal or momentum-weighted
* **Risk Profile**: Higher turnover, susceptible to momentum crashes
* **Performance Characteristics**: Strong in trending markets, poor during reversals

### Quality Factor

* **Definition**: Companies with stable earnings, strong balance sheets
* **Common Metrics**: ROE, earnings stability, debt/equity, accruals ratio
* **Implementation**: Composite quality score, select highest scoring stocks
* **Risk Profile**: Lower volatility, defensive characteristics
* **Performance Characteristics**: Outperforms in economic slowdowns, market stress

### Size Factor

* **Definition**: Smaller capitalization companies
* **Common Metrics**: Market capitalization
* **Implementation**: Select smaller companies, often equal-weighted
* **Risk Profile**: Higher volatility, liquidity constraints
* **Performance Characteristics**: Long-term outperformance with significant volatility

### Low Volatility Factor

* **Definition**: Securities with lower price fluctuations
* **Common Metrics**: Historical volatility, beta
* **Implementation**: Select lowest volatility stocks, often with risk-weighted scheme
* **Risk Profile**: Interest rate sensitivity, sector concentration
* **Performance Characteristics**: Downside protection, lower upside capture

## Considerations and Challenges

Single-factor models face several important challenges:

* **Factor Cyclicality**: Factors undergo periods of outperformance and underperformance
* **Factor Definition**: Different definitions may lead to materially different portfolios
* **Unintended Exposures**: Single-factor focus may introduce sector/country biases
* **Transaction Costs**: Some factors (like momentum) require higher turnover
* **Capacity Constraints**: Some factors may have limited capacity before returns diminish

## Risk Management

Specific risk management considerations for single-factor portfolios:

* **Factor Decay Monitoring**: Regular assessment of factor effectiveness
* **Crowding Analysis**: Evaluation of factor popularity and crowding
* **Exposure Constraints**: Limits on sector, industry, country exposures
* **Rebalancing Protocols**: Systematic rebalancing to maintain factor exposure
* **Volatility Management**: Factor-specific volatility forecasting and management

## VeritasVault Implementation

VeritasVault provides comprehensive tools for single-factor portfolio construction:

* **Factor Library**: Pre-defined factor definitions with historical data
* **Factor Analysis**: Tools to analyze factor characteristics and performance
* **Custom Factor Construction**: Ability to create and test custom factor definitions
* **Factor Simulation**: Back-testing capabilities for factor performance
* **Factor Portfolio Construction**: Automated construction with constraint handling

## Integration Points

Single-factor models integrate with other components of the VeritasVault platform:

* **Multi-Factor Models**: Component models for multi-factor approaches
* **Factor Attribution**: Performance analysis through factor lens
* **Risk Modeling**: Factor-based risk decomposition
* **Optimization Engine**: Constraint implementation and optimization

For specific implementation details on particular single-factor strategies, refer to the factor-specific documentation.