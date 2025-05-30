# Downside Risk Measures

> Advanced techniques for measuring portfolio downside risk

---

## Overview

Downside risk measures focus on the negative deviations in investment returns, which aligns more closely with investor preferences than traditional variance-based measures. This document details various downside risk measurement approaches implemented in VeritasVault.

## Key Principles

Downside risk measures are built on these foundational principles:

1. **Asymmetric Risk Perception**: Investors view losses differently than gains
2. **Threshold-Based Evaluation**: Risk is evaluated relative to a minimum acceptable return
3. **Tail Risk Focus**: Special attention to extreme negative outcomes
4. **Distribution-Aware**: Not constrained by normality assumptions

## Core Downside Risk Measures

### Downside Deviation

Measures the volatility of returns below a minimum acceptable return (MAR):

```
Downside Deviation = √[ ∑ min(r_i - MAR, 0)² / n ]
```

Where:
- r_i = return in period i
- MAR = minimum acceptable return (often 0 or risk-free rate)
- n = number of observations

**Implementation**:
```python
def downside_deviation(returns, mar=0):
    """
    Calculate downside deviation.
    
    Parameters:
    returns (array-like): Array of returns
    mar (float): Minimum acceptable return
    
    Returns:
    float: Downside deviation
    """
    # Calculate downside differences
    downside_diff = np.minimum(returns - mar, 0)
    
    # Square the differences and take mean
    squared_downside = np.square(downside_diff)
    mean_squared_downside = np.mean(squared_downside)
    
    # Take square root
    return np.sqrt(mean_squared_downside)
```

### Sortino Ratio

Risk-adjusted performance measure using downside deviation:

```
Sortino Ratio = (r_p - r_f) / DD
```

Where:
- r_p = portfolio return
- r_f = risk-free rate
- DD = downside deviation

**Implementation**:
```python
def sortino_ratio(returns, risk_free_rate=0, mar=0):
    """
    Calculate Sortino ratio.
    
    Parameters:
    returns (array-like): Array of returns
    risk_free_rate (float): Risk-free rate
    mar (float): Minimum acceptable return for downside deviation
    
    Returns:
    float: Sortino ratio
    """
    # Calculate mean excess return
    excess_return = np.mean(returns) - risk_free_rate
    
    # Calculate downside deviation
    dd = downside_deviation(returns, mar)
    
    # Return Sortino ratio
    return excess_return / dd if dd > 0 else np.nan
```

### Lower Partial Moments

Generalized framework for downside risk:

```
LPM(n, τ) = E[max(τ - R, 0)ⁿ]
```

Where:
- n = order of the moment
- τ = threshold return
- R = random variable representing returns

**Orders**:
- n = 0: Shortfall probability
- n = 1: Expected shortfall
- n = 2: Downside variance (squared semi-deviation)

**Implementation**:
```python
def lower_partial_moment(returns, threshold=0, order=2):
    """
    Calculate lower partial moment of specified order.
    
    Parameters:
    returns (array-like): Array of returns
    threshold (float): Threshold return
    order (int): Order of moment
    
    Returns:
    float: Lower partial moment
    """
    # Calculate shortfall
    shortfall = np.maximum(threshold - returns, 0)
    
    # Calculate lower partial moment
    lpm = np.mean(shortfall ** order)
    
    return lpm
```

### Drawdown Measures

Captures the decline from a historical peak:

```
Drawdown_t = (V_t / max_{0≤j≤t} V_j) - 1
```

Where:
- V_t = portfolio value at time t

**Key Drawdown Metrics**:

1. **Maximum Drawdown (MDD)**:
   ```
   MDD = min_{0≤t≤T} Drawdown_t
   ```

2. **Average Drawdown**:
   ```
   AvgDD = mean(Drawdown_t)
   ```

3. **Conditional Drawdown at Risk (CDaR)**:
   Average of worst drawdowns at a specified confidence level

**Implementation**:
```python
def calculate_drawdowns(returns):
    """
    Calculate drawdown series from returns.
    
    Parameters:
    returns (array-like): Array of returns
    
    Returns:
    array: Drawdown series
    """
    # Calculate cumulative returns
    cum_returns = (1 + returns).cumprod()
    
    # Calculate running maximum
    running_max = np.maximum.accumulate(cum_returns)
    
    # Calculate drawdowns
    drawdowns = (cum_returns / running_max) - 1
    
    return drawdowns

def maximum_drawdown(returns):
    """
    Calculate maximum drawdown.
    
    Parameters:
    returns (array-like): Array of returns
    
    Returns:
    float: Maximum drawdown
    """
    drawdowns = calculate_drawdowns(returns)
    return np.min(drawdowns)

def conditional_drawdown_at_risk(returns, alpha=0.05):
    """
    Calculate Conditional Drawdown at Risk (CDaR).
    
    Parameters:
    returns (array-like): Array of returns
    alpha (float): Confidence level (e.g., 0.05 for 95%)
    
    Returns:
    float: CDaR
    """
    drawdowns = calculate_drawdowns(returns)
    sorted_drawdowns = np.sort(drawdowns)
    index = int(np.ceil(alpha * len(drawdowns))) - 1
    worst_drawdowns = sorted_drawdowns[:index+1]
    return np.mean(worst_drawdowns)
```

### Downside Correlation and Beta

Measures co-movement during negative market periods:

1. **Downside Correlation**:
   ```
   Downside Correlation = Corr(R_A, R_B | R_B < τ)
   ```

2. **Downside Beta**:
   ```
   Downside Beta = Cov(R_A, R_B | R_B < τ) / Var(R_B | R_B < τ)
   ```

Where:
- R_A = asset or portfolio returns
- R_B = benchmark returns
- τ = threshold (often 0 or mean return)

**Implementation**:
```python
def downside_correlation(returns_a, returns_b, threshold=0):
    """
    Calculate downside correlation.
    
    Parameters:
    returns_a (array-like): Returns of asset/portfolio A
    returns_b (array-like): Returns of benchmark B
    threshold (float): Threshold return
    
    Returns:
    float: Downside correlation
    """
    # Filter for downside periods
    mask = returns_b < threshold
    downside_a = returns_a[mask]
    downside_b = returns_b[mask]
    
    # Calculate correlation
    if len(downside_a) > 1:
        return np.corrcoef(downside_a, downside_b)[0, 1]
    else:
        return np.nan

def downside_beta(returns_a, returns_b, threshold=0):
    """
    Calculate downside beta.
    
    Parameters:
    returns_a (array-like): Returns of asset/portfolio A
    returns_b (array-like): Returns of benchmark B
    threshold (float): Threshold return
    
    Returns:
    float: Downside beta
    """
    # Filter for downside periods
    mask = returns_b < threshold
    downside_a = returns_a[mask]
    downside_b = returns_b[mask]
    
    # Calculate downside beta
    if len(downside_a) > 1:
        cov = np.cov(downside_a, downside_b)[0, 1]
        var = np.var(downside_b, ddof=1)
        return cov / var if var > 0 else np.nan
    else:
        return np.nan
```

## Advanced Downside Risk Measures

### Conditional Value at Risk (CVaR)

Also known as Expected Shortfall, measures the expected loss exceeding VaR:

```
CVaR_α = E[R | R ≤ VaR_α]
```

Where:
- α = confidence level (e.g., 95%)
- VaR_α = Value at Risk at confidence level α

**Implementation**:
```python
def conditional_value_at_risk(returns, alpha=0.05, method='historical'):
    """
    Calculate Conditional Value at Risk (CVaR).
    
    Parameters:
    returns (array-like): Array of returns
    alpha (float): Confidence level (e.g., 0.05 for 95%)
    method (str): Calculation method ('historical' or 'parametric')
    
    Returns:
    float: CVaR
    """
    if method == 'historical':
        # Sort returns
        sorted_returns = np.sort(returns)
        
        # Find cutoff index
        cutoff_index = int(np.ceil(alpha * len(returns)))
        
        # Calculate CVaR as mean of worst returns
        worst_returns = sorted_returns[:cutoff_index]
        return np.mean(worst_returns)
    
    elif method == 'parametric':
        # Calculate mean and standard deviation
        mu = np.mean(returns)
        sigma = np.std(returns, ddof=1)
        
        # Calculate VaR
        var = mu + sigma * stats.norm.ppf(alpha)
        
        # Calculate CVaR
        return mu - sigma * stats.norm.pdf(stats.norm.ppf(alpha)) / alpha
    
    else:
        raise ValueError("Method must be 'historical' or 'parametric'")
```

For more detailed information on CVaR, see [Conditional Value at Risk](./tail-risk/conditional-value-at-risk.md).

### Entropic Value at Risk (EVaR)

Risk measure based on relative entropy:

```
EVaR_α(X) = inf_{z>0} {z⁻¹ log(M_X(z)/α)}
```

Where:
- M_X(z) = moment generating function of X
- α = confidence level

**Characteristics**:
- Coherent risk measure
- Accounts for tail risk beyond CVaR
- Less sensitive to extreme outliers than CVaR

### Tail Risk Parity

Portfolio construction approach allocating risk equally to tail events:

```
TRP: min_w ∑_i,j (w_i × TC_i - w_j × TC_j)²
```

Where:
- w_i = weight of asset i
- TC_i = tail risk contribution of asset i

## Practical Applications

### Risk Budgeting with Downside Risk

Allocating portfolio risk budget using downside measures:

1. **Downside Risk Decomposition**:
   ```
   DR_p = ∑_i w_i × MRC_i
   ```
   Where MRC_i is the marginal risk contribution of asset i

2. **Equal Downside Risk Contribution (EDRC)**:
   Portfolio where each asset contributes equally to downside risk

### Risk-Adjusted Performance Evaluation

Using downside risk for performance assessment:

| Measure | Formula | Description |
|---------|---------|-------------|
| Sortino Ratio | (r_p - r_f) / DD | Return per unit of downside risk |
| Omega Ratio | E[max(R-τ,0)] / E[max(τ-R,0)] | Ratio of gains to losses |
| Calmar Ratio | (r_p - r_f) / MDD | Return per unit of maximum drawdown |
| Pain Ratio | (r_p - r_f) / Pain Index | Return per unit of average drawdown |

### Downside Risk Forecasting

Approaches for predicting future downside risk:

1. **Conditional Volatility Models**:
   * GARCH models focusing on downside movements
   * Asymmetric GARCH capturing leverage effects

2. **Extreme Value Theory (EVT)**:
   * Peaks-over-threshold for modeling tail behavior
   * Block maxima approach for extreme events

3. **Copula-Based Methods**:
   * Capturing non-linear dependencies in extreme events
   * Tail-dependent copulas for downside co-movement

## VeritasVault Implementation

VeritasVault provides comprehensive tools for downside risk analysis:

1. **Downside Risk Engine**: Core calculation of downside measures
2. **Drawdown Analysis**: Detailed drawdown computation and visualization
3. **Tail Risk Modeling**: Advanced modeling of tail events
4. **Scenario Stress Testing**: Testing portfolio behavior in adverse scenarios
5. **Downside Risk Attribution**: Identifying contributors to downside risk
6. **Downside Risk Optimization**: Portfolio construction with downside constraints

## Related Documentation

* [Risk Measures Overview](./risk-measures-overview.md)
* [Value at Risk](./tail-risk/value-at-risk.md)
* [Conditional Value at Risk](./tail-risk/conditional-value-at-risk.md)
* [Extreme Value Theory](./tail-risk/extreme-value-theory.md)
* [Stress Testing](./scenario-analysis/stress-testing.md)