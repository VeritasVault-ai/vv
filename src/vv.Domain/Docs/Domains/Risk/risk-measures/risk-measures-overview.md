# Risk Measures Overview

> Comprehensive guide to investment risk measurement techniques

---

## Overview

This document provides an overview of the risk measurement methodologies used in VeritasVault for portfolio and investment risk assessment. It covers the theoretical foundation, implementation approaches, and practical applications of various risk measures.

## Core Risk Measurement Concepts

### Risk Dimensions

Risk in investment portfolios can be measured across multiple dimensions:

1. **Volatility Risk**: Variability of returns around the mean
2. **Downside Risk**: Potential for losses below a threshold
3. **Tail Risk**: Extreme negative outcomes
4. **Relative Risk**: Performance relative to a benchmark
5. **Factor Risk**: Exposure to systematic risk factors
6. **Liquidity Risk**: Ability to transact without significant price impact
7. **Concentration Risk**: Exposure to individual positions or sectors

### Risk Measurement Time Horizons

Risk measurement varies across different time horizons:

* **Short-Term**: Daily to weekly risk (trading and operational focus)
* **Medium-Term**: Monthly to quarterly risk (tactical asset allocation)
* **Long-Term**: Annual or multi-year risk (strategic asset allocation)

## Traditional Risk Measures

### Variance and Standard Deviation

The most common measure of total risk:

```
Variance = σ² = E[(R - E[R])²]
Standard Deviation = σ = √σ²
```

**Characteristics**:
* Measures dispersion around the mean
* Treats upside and downside deviations equally
* Assumes normal distribution of returns

**Implementation**:
```python
def calculate_variance(returns):
    """Calculate variance of returns."""
    return np.var(returns, ddof=1)

def calculate_std_dev(returns):
    """Calculate standard deviation of returns."""
    return np.std(returns, ddof=1)
```

### Beta

Measures systematic risk relative to a benchmark:

```
Beta = β = Cov(R, R_m) / Var(R_m)
```

Where:
* R = asset or portfolio returns
* R_m = market (benchmark) returns

**Characteristics**:
* Measures sensitivity to market movements
* β > 1: More volatile than market
* β < 1: Less volatile than market
* β = 0: Uncorrelated with market

**Implementation**:
```python
def calculate_beta(returns, market_returns):
    """Calculate beta of returns relative to market returns."""
    covariance = np.cov(returns, market_returns)[0, 1]
    market_variance = np.var(market_returns, ddof=1)
    return covariance / market_variance
```

### Tracking Error

Measures the variability of return differences between portfolio and benchmark:

```
Tracking Error = TE = σ(R_p - R_b)
```

Where:
* R_p = portfolio returns
* R_b = benchmark returns

**Characteristics**:
* Measures active risk
* Higher values indicate greater deviation from benchmark
* Used for active management evaluation

**Implementation**:
```python
def calculate_tracking_error(portfolio_returns, benchmark_returns):
    """Calculate tracking error."""
    excess_returns = portfolio_returns - benchmark_returns
    return np.std(excess_returns, ddof=1)
```

## Downside Risk Measures

### Semivariance and Semideviation

Measures dispersion of returns below the mean or a target:

```
Semivariance = E[min(R - T, 0)²]
Semideviation = √Semivariance
```

Where T is the target return (often mean return or 0)

**Characteristics**:
* Only considers downside deviations
* More aligned with investor preferences
* Does not assume symmetrical returns

**Implementation**:
```python
def calculate_semivariance(returns, target=None):
    """Calculate semivariance of returns below target."""
    if target is None:
        target = np.mean(returns)
    
    downside_returns = np.minimum(returns - target, 0)
    return np.mean(downside_returns**2)

def calculate_semideviation(returns, target=None):
    """Calculate semideviation of returns below target."""
    return np.sqrt(calculate_semivariance(returns, target))
```

For more detailed information on downside risk measures, see [Downside Risk Measures](./downside-risk-measures.md).

## Value at Risk (VaR)

Estimates the maximum loss within a confidence interval:

```
VaR_α = F⁻¹(α)
```

Where:
* α = confidence level (e.g., 95%)
* F⁻¹ = inverse cumulative distribution function of returns

**Characteristics**:
* Easy to interpret (e.g., "95% confident loss won't exceed X")
* Widely used in risk management and regulation
* Multiple calculation approaches (parametric, historical, Monte Carlo)

**Implementation**:
```python
def calculate_parametric_var(returns, confidence=0.95, horizon=1):
    """Calculate parametric Value at Risk."""
    mu = np.mean(returns)
    sigma = np.std(returns, ddof=1)
    
    # For daily returns, assuming normal distribution
    var = -1 * (mu * horizon + sigma * np.sqrt(horizon) * 
                stats.norm.ppf(1 - confidence))
    
    return var

def calculate_historical_var(returns, confidence=0.95):
    """Calculate historical Value at Risk."""
    return -1 * np.percentile(returns, (1 - confidence) * 100)
```

For more detailed information on VaR and related measures, see [Value at Risk](./tail-risk/value-at-risk.md).

## Conditional Value at Risk (CVaR)

Also known as Expected Shortfall (ES), measures the average loss beyond VaR:

```
CVaR_α = E[R | R ≤ VaR_α]
```

**Characteristics**:
* Accounts for tail risk beyond VaR
* Coherent risk measure (unlike VaR)
* More sensitive to extreme events

**Implementation**:
```python
def calculate_historical_cvar(returns, confidence=0.95):
    """Calculate historical Conditional Value at Risk."""
    var = calculate_historical_var(returns, confidence)
    return -1 * np.mean(returns[returns <= -var])
```

For more detailed information on CVaR, see [Conditional Value at Risk](./tail-risk/conditional-value-at-risk.md).

## Factor-Based Risk Measures

### Factor Contribution to Risk

Decomposes portfolio risk into factor contributions:

```
Risk_p = ∑_i ∑_j w_i w_j σ_ij = ∑_k β_k² σ_k² + σ_ε²
```

Where:
* w_i, w_j = weights of assets i and j
* σ_ij = covariance between assets i and j
* β_k = portfolio exposure to factor k
* σ_k² = variance of factor k
* σ_ε² = specific risk

**Characteristics**:
* Identifies systematic sources of risk
* Allows for risk decomposition by factor
* Useful for risk budgeting

**Implementation**:
```python
def calculate_factor_risk_contribution(weights, factor_exposures, factor_cov, specific_risk):
    """Calculate factor contribution to portfolio risk."""
    # Portfolio factor exposures
    portfolio_exposures = weights @ factor_exposures
    
    # Factor contribution to risk
    factor_contribution = portfolio_exposures @ factor_cov @ portfolio_exposures
    
    # Specific risk contribution
    specific_contribution = weights @ np.diag(specific_risk) @ weights
    
    # Total risk
    total_risk = factor_contribution + specific_contribution
    
    return {
        'factor_contribution': factor_contribution,
        'specific_contribution': specific_contribution,
        'total_risk': total_risk
    }
```

For more information on factor-based risk, see [Risk Factor Parity](./risk-factor-parity.md).

## Risk Measure Selection

Guidelines for selecting appropriate risk measures:

| Risk Measure | When to Use | Limitations |
|--------------|-------------|-------------|
| Standard Deviation | Symmetrical return distributions | Penalizes upside equally |
| Semideviation | Asymmetrical distributions | More complex calculation |
| VaR | Regulatory requirements, simple interpretation | Not coherent, ignores tail risk |
| CVaR | Tail risk concerns | More data requirements |
| Beta | Relative to market risk | Market may not be relevant benchmark |
| Tracking Error | Active management | Doesn't measure absolute risk |
| Factor Risk | Understanding risk sources | Model risk in factor specification |

## Risk Measurement Implementation

Best practices for implementing risk measures:

1. **Data Quality**:
   * Ensure sufficient history
   * Address outliers appropriately
   * Handle missing data consistently

2. **Estimation Techniques**:
   * Use appropriate estimators for covariance
   * Consider shrinkage methods for stability
   * Apply robust statistical methods

3. **Time Scaling**:
   * Scale risk measures appropriately across time horizons
   * Consider autocorrelation in returns
   * Adjust for changing volatility regimes

4. **Interpretation**:
   * Provide context for risk measures
   * Compare to relevant benchmarks
   * Consider multiple risk dimensions

## VeritasVault Risk Framework

VeritasVault implements a comprehensive risk measurement framework:

1. **Risk Engine**: Core calculation of risk measures
2. **Risk Decomposition**: Breakdown of risk by factors, sectors, etc.
3. **Risk Attribution**: Analysis of risk contributors
4. **Scenario Analysis**: Stress testing under various scenarios
5. **Risk Visualization**: Intuitive display of risk information
6. **Risk Monitoring**: Ongoing tracking of risk measures

## Related Documents

* [Downside Risk Measures](./downside-risk-measures.md)
* [Risk Factor Parity](./risk-factor-parity.md)
* [Tail Risk Overview](./tail-risk/tail-risk-overview.md)
* [Value at Risk](./tail-risk/value-at-risk.md)
* [Conditional Value at Risk](./tail-risk/conditional-value-at-risk.md)
* [Stress Testing](./scenario-analysis/stress-testing.md)