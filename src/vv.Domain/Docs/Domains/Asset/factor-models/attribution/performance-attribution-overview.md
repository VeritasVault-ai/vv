---
document_type: domain-overview
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

```src/vv.Domain/Docs/Domains/Asset/performance-attribution-overview.md
# Performance Attribution Overview

> Frameworks for analyzing sources of portfolio performance

---

## Overview

Performance attribution is the process of analyzing a portfolio's performance to identify the sources of returns relative to a benchmark. This document provides a comprehensive overview of performance attribution methodologies implemented in VeritasVault.

## Core Attribution Concepts

### Purpose of Attribution

Performance attribution serves several key purposes:

1. **Performance Explanation**: Identify sources of outperformance or underperformance
2. **Skill Assessment**: Determine if performance is due to skill or luck
3. **Risk Alignment**: Ensure returns are generated from intended risk exposures
4. **Strategy Validation**: Validate that investment process is working as expected
5. **Communication**: Provide clear explanation to stakeholders

### Attribution Dimensions

Performance can be attributed along multiple dimensions:

1. **Asset Allocation**: Effects of allocating to different asset classes
2. **Security Selection**: Effects of selecting specific securities within asset classes
3. **Factor Exposures**: Effects of exposure to systematic risk factors
4. **Trading/Timing**: Effects of timing decisions in implementation
5. **Currency**: Effects of currency movements in global portfolios

### Attribution Time Periods

Attribution analysis can be performed across different time horizons:

* **Single-Period**: Attribution over a single measurement period
* **Multi-Period**: Linking attribution effects across multiple periods
* **Rolling-Period**: Attribution over rolling time windows
* **Inception-to-Date**: Cumulative attribution since portfolio inception

## Attribution Methodologies

### Returns-Based Attribution

Analysis based solely on portfolio and benchmark returns:

* **Strengths**: Requires minimal data, easy to implement
* **Weaknesses**: Limited granularity, may miss important effects
* **Best For**: High-level analysis, factor attribution

For detailed information, see [Returns-Based Attribution](./returns-based-attribution.md).

### Holdings-Based Attribution

Analysis based on detailed portfolio and benchmark holdings:

* **Strengths**: Precise analysis at security level, captures timing effects
* **Weaknesses**: Data-intensive, sensitive to data quality
* **Best For**: Detailed performance analysis, security selection evaluation

For detailed information, see [Holdings-Based Attribution](./holdings-based-attribution.md).

### Factor-Based Attribution

Analysis based on exposure to systematic risk factors:

* **Strengths**: Identifies systematic sources of returns, consistent framework
* **Weaknesses**: Model-dependent, factor selection impacts results
* **Best For**: Style analysis, risk-adjusted performance evaluation

For detailed information, see [Factor Attribution Overview](./factor-attribution-overview.md).

## Common Attribution Models

### Brinson Models

#### Brinson-Hood-Beebower (BHB) Model

The classic model decomposing returns into allocation and selection effects:

```
Total Active Return = Allocation Effect + Selection Effect + Interaction Effect
```

Where:
* **Allocation Effect**: Impact of over/underweighting sectors relative to benchmark
* **Selection Effect**: Impact of security selection within sectors
* **Interaction Effect**: Combined impact of allocation and selection decisions

#### Brinson-Fachler (BF) Model

A modification of BHB that attributes interaction effect to selection:

```
Total Active Return = Allocation Effect + Selection Effect
```

Where the selection effect includes the interaction effect.

### Factor Models

#### CAPM-Based Attribution

Simple single-factor attribution:

```
r_p - r_f = α + β(r_m - r_f) + ε
```

Where:
* α = security selection (alpha)
* β(r_m - r_f) = market exposure
* ε = residual returns

#### Multi-Factor Attribution

Attribution based on exposure to multiple factors:

```
r_p - r_f = α + ∑βᵢFᵢ + ε
```

Where:
* α = security selection (alpha)
* βᵢ = exposure to factor i
* Fᵢ = return of factor i
* ε = residual returns

For detailed information, see [Equity Factor Attribution](./equity-factor-attribution.md).

### Fixed Income Attribution

Specialized models for fixed income portfolios:

1. **Yield Curve Models**: Attribution based on yield curve movements
2. **Spread Models**: Attribution based on spread changes
3. **Duration/Convexity Models**: Attribution based on interest rate sensitivities

## Mathematical Framework

### Returns Decomposition

For a portfolio with assets in different segments (e.g., sectors, countries):

```
r_p - r_b = ∑ᵢ(wᵖᵢ - wᵇᵢ)r_bᵢ + ∑ᵢwᵖᵢ(r_pᵢ - r_bᵢ)
```

Where:
* r_p = portfolio return
* r_b = benchmark return
* wᵖᵢ = portfolio weight in segment i
* wᵇᵢ = benchmark weight in segment i
* r_pᵢ = portfolio return in segment i
* r_bᵢ = benchmark return in segment i

### Multi-Period Attribution

Several approaches exist for linking attribution effects across periods:

1. **Arithmetic Linking**: Simple addition of effects across periods
2. **Geometric Linking**: Compounding of effects across periods
3. **Logarithmic Linking**: Using logarithmic returns for linking
4. **Carino Linking**: Adjusting for the cross-product effect

## Implementation Considerations

### Data Requirements

Quality data is essential for accurate attribution:

1. **Holdings Data**: Complete and accurate portfolio and benchmark holdings
2. **Pricing Data**: Consistent pricing across portfolio and benchmark
3. **Corporate Action Data**: Properly adjusted for splits, dividends, etc.
4. **Classification Data**: Consistent classification for segments
5. **Factor Data**: Reliable factor definitions and returns

### Benchmark Selection

The choice of benchmark significantly impacts attribution results:

1. **Standard Indices**: Widely recognized but may not match investment universe
2. **Custom Benchmarks**: Better aligned but less recognized
3. **Style Benchmarks**: Capture style characteristics
4. **Multiple Benchmarks**: Different benchmarks for different portfolio components

### Currency Effects

For global portfolios, currency effects require special consideration:

1. **Currency Contribution**: Direct impact of currency movements
2. **Local vs. Base Returns**: Distinguishing local and currency-converted returns
3. **Currency Hedging Effects**: Impact of currency hedging decisions

## Attribution Analysis and Reporting

### Key Metrics

Important metrics for attribution analysis:

1. **Active Return**: Portfolio return minus benchmark return
2. **Information Ratio**: Active return divided by tracking error
3. **Hit Rate**: Percentage of segments with positive selection effect
4. **Consistency Metrics**: Consistency of effects across time periods
5. **Statistical Significance**: Statistical validation of attribution results

### Visualization Techniques

Effective ways to visualize attribution results:

1. **Attribution Heat Maps**: Color-coded attribution effects by segment
2. **Contribution Charts**: Waterfall charts showing contribution to active return
3. **Time Series Analysis**: Evolution of attribution effects over time
4. **Scatter Plots**: Relationship between allocation decisions and results
5. **Factor Exposure Maps**: Visualization of factor exposures

### Stakeholder Communication

Tailoring attribution reporting to different audiences:

1. **Investment Committees**: Focus on alignment with investment strategy
2. **Portfolio Managers**: Detailed analysis for decision improvement
3. **Clients**: Clear, intuitive explanation of performance drivers
4. **Regulators**: Compliance with disclosure requirements

## VeritasVault Implementation

VeritasVault provides comprehensive attribution capabilities:

1. **Attribution Engine**: Core calculation of attribution effects
2. **Multi-Model Support**: Implementation of various attribution methodologies
3. **Interactive Analytics**: Dynamic analysis of attribution results
4. **Attribution Reporting**: Customizable reporting for different stakeholders
5. **Attribution Database**: Historical storage of attribution results

## Related Documentation

* [Returns-Based Attribution](./returns-based-attribution.md)
* [Holdings-Based Attribution](./holdings-based-attribution.md)
* [Factor Attribution Overview](./factor-attribution-overview.md)
* [Equity Factor Attribution](./equity-factor-attribution.md)
* [Risk Measures Overview](../../../Risk/risk-measures/risk-measures-overview.md)