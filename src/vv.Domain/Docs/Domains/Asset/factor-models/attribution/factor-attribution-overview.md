# Factor Attribution Overview

> Framework for attributing performance to systematic risk factors

---

## Overview

Factor attribution analyzes portfolio performance through the lens of systematic risk factors. This approach decomposes returns into factor exposures and specific returns, providing insight into the systematic drivers of portfolio performance.

## Key Principles

Factor attribution is built on these core principles:

1. **Factor-Based Decomposition**: Explains returns through systematic factors
2. **Exposure Analysis**: Quantifies portfolio exposure to various factors
3. **Risk-Return Assessment**: Links factor exposures to both risk and return
4. **Style Characterization**: Identifies investment style through factor exposures
5. **Skill Evaluation**: Distinguishes between factor returns and security selection

## Factor Attribution Framework

### General Factor Model

The general form of a factor model:

```
r_i = α_i + ∑(β_i,j × F_j) + ε_i
```

Where:
- r_i = return of asset i
- α_i = asset-specific return (alpha)
- β_i,j = exposure of asset i to factor j
- F_j = return of factor j
- ε_i = residual return (specific return)

### Portfolio Factor Attribution

At the portfolio level:

```
r_p = α_p + ∑(β_p,j × F_j) + ε_p
```

Where:
- r_p = portfolio return
- α_p = portfolio alpha
- β_p,j = portfolio exposure to factor j
- ε_p = portfolio specific return

## Common Factor Models

Several factor models are commonly used in attribution:

### Capital Asset Pricing Model (CAPM)

Single-factor model using market factor:

```
r_i - r_f = α_i + β_i(r_m - r_f) + ε_i
```

Where:
- r_f = risk-free rate
- r_m = market return
- β_i = market beta

### Fama-French Models

Multi-factor models incorporating size and value:

**Three-Factor Model**:
```
r_i - r_f = α_i + β_i,M(r_m - r_f) + β_i,SMB × SMB + β_i,HML × HML + ε_i
```

Where:
- SMB = Small Minus Big (size factor)
- HML = High Minus Low (value factor)

**Five-Factor Model**:
Adds profitability and investment factors:
```
r_i - r_f = α_i + β_i,M(r_m - r_f) + β_i,SMB × SMB + β_i,HML × HML 
           + β_i,RMW × RMW + β_i,CMA × CMA + ε_i
```

Where:
- RMW = Robust Minus Weak (profitability factor)
- CMA = Conservative Minus Aggressive (investment factor)

### Custom Factor Models

For specialized applications, custom factor models can be developed:

1. **Macroeconomic Factor Models**: Using economic variables
2. **Statistical Factor Models**: Derived from principal component analysis
3. **Fundamental Factor Models**: Based on company fundamentals
4. **Industry-Specific Models**: Tailored to specific sectors

## Implementation Methodology

### Factor Exposure Estimation

```python
def estimate_factor_exposures(returns, factor_returns):
    """
    Estimate factor exposures using regression.
    
    Parameters:
    returns (DataFrame): Asset or portfolio returns
    factor_returns (DataFrame): Factor returns
    
    Returns:
    Series: Factor exposures (betas) and alpha
    """
    # Add constant for alpha
    X = sm.add_constant(factor_returns)
    
    # Fit regression
    model = sm.OLS(returns, X).fit()
    
    # Extract results
    alpha = model.params[0]
    betas = model.params[1:]
    r_squared = model.rsquared
    
    return {
        'alpha': alpha,
        'exposures': betas,
        'r_squared': r_squared
    }
```

### Return Decomposition

```python
def decompose_returns(returns, factor_returns, exposures, alpha):
    """
    Decompose returns into factor and specific components.
    
    Parameters:
    returns (Series): Actual returns
    factor_returns (DataFrame): Factor returns
    exposures (Series): Factor exposures
    alpha (float): Estimated alpha
    
    Returns:
    dict: Return components
    """
    # Calculate factor contribution
    factor_contribution = {}
    for factor in exposures.index:
        factor_contribution[factor] = exposures[factor] * factor_returns[factor]
    
    # Calculate total factor contribution
    total_factor = sum(factor_contribution.values())
    
    # Calculate specific return
    specific_return = returns - alpha - total_factor
    
    return {
        'alpha': alpha,
        'factor_contribution': factor_contribution,
        'total_factor': total_factor,
        'specific_return': specific_return,
        'total_return': returns
    }
```

## Factor Model Construction

For detailed information on building and implementing factor models, these specialized documents provide in-depth guidance:

* [Fundamental Factor Models](./fundamental-factor-models.md)
* [Statistical Factor Models](./statistical-factor-models.md)
* [Macroeconomic Factor Models](./macroeconomic-factor-models.md)

## Applications

### Active Risk Decomposition

Factor models can decompose active risk:

```
Active Risk² = ∑(Active Exposure_j² × Factor Variance_j) 
               + ∑∑(Active Exposure_i × Active Exposure_j × Factor Covariance_i,j)
               + Specific Risk²
```

Where:
- Active Exposure_j = Portfolio exposure to factor j minus benchmark exposure

### Performance Attribution

Performance can be attributed to factors:

```
Active Return = ∑(Active Exposure_j × Factor Return_j) + Security Selection
```

### Style Analysis

Factor exposures reveal investment style:

1. **Growth vs. Value**: Exposure to value factors
2. **Size Bias**: Exposure to size factors
3. **Momentum Exposure**: Exposure to momentum factors
4. **Quality Tilt**: Exposure to quality factors

## Practical Considerations

### Factor Selection

Criteria for selecting factors:

1. **Economic Rationale**: Factors should have economic justification
2. **Persistence**: Factors should be persistent over time
3. **Pervasiveness**: Factors should apply across markets
4. **Robustness**: Factors should be robust to different definitions
5. **Investability**: Factors should be implementable

### Estimation Issues

Common challenges in factor modeling:

1. **Multicollinearity**: Correlation between factors
2. **Time-Varying Exposures**: Changing factor exposures over time
3. **Regime Changes**: Different factor behavior in different regimes
4. **Outliers**: Impact of extreme return observations
5. **Limited Data**: Statistical significance with limited history

## VeritasVault Implementation

VeritasVault provides comprehensive factor attribution capabilities:

1. **Factor Model Library**: Pre-built factor models
2. **Custom Model Builder**: Tools for custom factor model creation
3. **Exposure Analysis**: Factor exposure calculation and visualization
4. **Attribution Engine**: Factor-based performance attribution
5. **Risk Decomposition**: Factor-based risk analysis

## Related Documentation

* [Performance Attribution Overview](./performance-attribution-overview.md)
* [Returns-Based Attribution](./returns-based-attribution.md)
* [Holdings-Based Attribution](./holdings-based-attribution.md)
* [Equity Factor Attribution](./equity-factor-attribution.md)
* [Multi-Factor Models](../multi-factor/multi-factor-models.md)