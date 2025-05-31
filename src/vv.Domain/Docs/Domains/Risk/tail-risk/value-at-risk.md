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

# Value-at-Risk (VaR)

> Estimating potential loss at a specific confidence level

---

## Overview

Value-at-Risk (VaR) is a risk measure that quantifies the maximum potential loss a portfolio may experience over a specified time horizon at a given confidence level. It has become one of the most widely used risk metrics in financial institutions, asset management, and regulatory frameworks.

## Key Principles

Value-at-Risk is built on these fundamental principles:

* **Probabilistic Framework**: Expresses risk in probabilistic terms
* **Time Horizon**: Specified period over which risk is measured
* **Confidence Level**: Statistical confidence in the risk estimate
* **Single Number**: Consolidates complex risk exposures into one figure
* **Relative Simplicity**: More straightforward than many alternative tail measures

## Mathematical Definition

### Formal Definition

Value-at-Risk at confidence level α over time horizon T is defined as:

* **Formal Definition**: VaR(α) is the smallest value x such that P(Loss > x) ≤ 1-α
* **Alternative Definition**: VaR(α) is the (1-α)-quantile of the loss distribution
* **Typical Parameters**:
  * α = 95%, 99%, or 99.5% confidence level
  * T = 1 day, 1 week, or 1 month time horizon

### Statistical Interpretation

* **Probability Statement**: With probability α, the loss will not exceed VaR
* **Exceedance Frequency**: Loss should exceed VaR approximately (1-α)% of the time
* **Not Expected Loss**: VaR is not the expected loss when threshold is exceeded

## Implementation Methodologies

### Parametric (Variance-Covariance) Method

* **Assumption**: Returns follow a specific distribution (typically normal)
* **Formula**: VaR = μ + σ × z(α)
  * Where z(α) is the α-quantile of the standard normal distribution
* **Process**:
  1. Estimate mean (μ) and standard deviation (σ) of returns
  2. Determine z-score for desired confidence level
  3. Calculate VaR
* **Advantages**: Simple implementation, minimal data requirements
* **Limitations**: Assumes normal distribution, underestimates tail risk

### Historical Simulation Method

* **Assumption**: Historical returns represent future return distribution
* **Process**:
  1. Collect historical return series of appropriate length
  2. Sort returns from worst to best
  3. Identify the return at the (1-α) percentile
  4. Calculate VaR based on this return
* **Advantages**: No distributional assumptions, captures actual market behavior
* **Limitations**: Highly dependent on historical sample, limited by available history

### Monte Carlo Simulation Method

* **Process**:
  1. Specify stochastic process for risk factors
  2. Generate large number of random scenarios
  3. Calculate portfolio value under each scenario
  4. Determine VaR from resulting distribution
* **Advantages**: Flexible modeling, can incorporate complex securities
* **Limitations**: Computationally intensive, model specification risk

## VaR for Different Asset Classes

### Equity Portfolios

* **Key Considerations**: Fat tails, skewness, time-varying volatility
* **Common Approaches**: Historical simulation, GARCH models
* **Limitations**: Correlation changes during stress periods

### Fixed Income Portfolios

* **Key Considerations**: Duration, convexity, credit spreads
* **Common Approaches**: Principal component analysis, regime-switching models
* **Limitations**: Non-linear price responses to yield changes

### Options and Derivatives

* **Key Considerations**: Non-linearity, time decay, volatility exposure
* **Common Approaches**: Full revaluation in Monte Carlo, Greek-based approximations
* **Limitations**: Higher-order risks not captured by linear approximations

### Multi-Asset Portfolios

* **Key Considerations**: Cross-asset correlations, diversification effects
* **Common Approaches**: Copula methods, factor models
* **Limitations**: Correlation instability in stress periods

## Extensions and Variations

### Conditional VaR (CVaR)

* **Definition**: Expected loss given that loss exceeds VaR
* **Advantages**: Accounts for severity beyond VaR threshold, coherent risk measure
* **Implementation**: See [Conditional Value-at-Risk](./conditional-value-at-risk.md)

### Component VaR

* **Definition**: Decomposition of total VaR into individual position contributions
* **Formula**: CVaR(i) = w(i) × β(i) × VaR(p)
  * Where β(i) is sensitivity of asset i to portfolio moves
* **Advantage**: Shows risk contribution of each position
* **Use**: Risk budgeting, position sizing

### Incremental VaR

* **Definition**: Change in VaR from adding or removing a position
* **Calculation**: VaR(new) - VaR(original)
* **Use**: Evaluating impact of potential trades

### Marginal VaR

* **Definition**: Sensitivity of VaR to small changes in position size
* **Calculation**: Partial derivative of VaR with respect to position weight
* **Use**: Portfolio optimization, risk allocation

## Practical Implementation

### Backtesting VaR Models

* **Purpose**: Verify accuracy of VaR estimates
* **Process**:
  1. Compare actual losses with VaR predictions
  2. Count number of VaR breaches
  3. Apply statistical tests (e.g., Kupiec test, Christoffersen test)
* **Regulatory Requirement**: Basel framework requires backtesting

### Selecting Parameters

* **Confidence Level**:
  * Higher levels (99%, 99.5%) for regulatory purposes
  * Lower levels (95%) for day-to-day risk management
* **Time Horizon**:
  * Shorter periods (1-day) for trading desks
  * Longer periods (10-day, 1-month) for regulatory and strategic views
* **Historical Window**:
  * Trade-off between relevance and statistical significance
  * Typical ranges from 1 to 3 years of daily data

### Adjustments and Considerations

* **Volatility Updating**: Techniques to account for changing volatility (e.g., EWMA, GARCH)
* **Scaling Methods**: Converting between time horizons (√T rule and alternatives)
* **Extreme Value Theory**: Enhancing tail modeling (see [Extreme Value Theory](./extreme-value-theory.md))
* **Stress Testing**: Complementing VaR with stress scenarios (see [Stress Testing Approaches](../scenario-analysis/stress-testing.md))

## Advantages of VaR

* **Intuitive Interpretation**: Clear probabilistic statement about potential loss
* **Universal Application**: Applicable across asset classes and portfolios
* **Standardization**: Widely accepted industry standard
* **Aggregation**: Can be aggregated across different positions and portfolios
* **Regulatory Acceptance**: Core component of regulatory capital frameworks

## Limitations and Criticisms

* **Tail Blindness**: Provides no information about severity of losses beyond VaR threshold
* **Non-Coherence**: Lacks mathematical property of subadditivity in general case
* **Model Risk**: High sensitivity to model assumptions
* **Procyclicality**: Can underestimate risk during calm periods
* **False Precision**: Appearance of precision may mask underlying uncertainties
* **Behavioral Impact**: May create false sense of security or encourage gaming

## VeritasVault Implementation

VeritasVault provides comprehensive tools for VaR calculation:

* **Multiple Methodologies**: Parametric, historical, and Monte Carlo approaches
* **Custom Confidence Levels**: Flexible confidence level specification
* **Time Horizon Adjustment**: Automated scaling across time horizons
* **Decomposition Tools**: Component and marginal VaR analytics
* **Backtesting Framework**: Automated VaR backtesting and exception analysis
* **Visual Analytics**: Interactive visualization of VaR and contributing factors

## Best Practices

* **Multiple Methods**: Use multiple VaR methodologies for comparison
* **Complementary Measures**: Combine VaR with other risk measures
* **Regular Backtesting**: Continuously validate VaR model accuracy
* **Stress Testing**: Complement with stress scenarios and worst-case analysis
* **Transparent Communication**: Clearly communicate VaR limitations to stakeholders
* **Regular Review**: Periodically review and update VaR methodology

For specific implementation details and code examples, please refer to the VaR calculation API documentation.