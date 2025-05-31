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

# Equity Factor Attribution

> Detailed attribution of equity portfolio performance to factor exposures

---

## Overview

Equity factor attribution analyzes the performance of equity portfolios by decomposing returns into contributions from systematic risk factors and security-specific returns. This document details the methodologies, implementation approaches, and applications specific to equity portfolios.

## Key Principles

Equity factor attribution is built on these core principles:

1. **Equity-Specific Factors**: Focuses on factors relevant to equity markets
2. **Style Analysis**: Quantifies exposures to established equity style factors
3. **Active Management Assessment**: Evaluates the sources of active returns
4. **Risk-Return Linkage**: Connects risk exposures to performance outcomes
5. **Skill Identification**: Distinguishes factor returns from security selection

## Equity Factor Models

### Common Equity Factors

The most widely used equity factors:

1. **Market (Beta)**: Sensitivity to overall market movements
2. **Size**: Exposure to market capitalization (small vs. large)
3. **Value**: Exposure to value metrics (P/B, P/E, etc.)
4. **Momentum**: Exposure to price trends
5. **Quality**: Exposure to profitability and earnings stability
6. **Low Volatility**: Exposure to historical volatility
7. **Dividend Yield**: Exposure to dividend-paying stocks
8. **Growth**: Exposure to earnings growth expectations

### Specialized Equity Factors

Additional factors for deeper analysis:

1. **Industry/Sector**: Exposure to industry groups
2. **Country/Region**: Geographical exposures
3. **Liquidity**: Exposure to trading liquidity
4. **Investment**: Capital expenditure patterns
5. **Profitability**: Operating profit margins
6. **Leverage**: Debt utilization
7. **Earnings Revision**: Analyst estimate changes
8. **ESG**: Environmental, social, governance metrics

## Implementation Framework

### Factor Model Specification

```python
def specify_equity_factor_model(returns_data, include_factors=None):
    """
    Specify an equity factor model based on available data.
    
    Parameters:
    returns_data (DataFrame): Historical returns data
    include_factors (list): Specific factors to include
    
    Returns:
    dict: Factor model specification
    """
    # Default factors
    all_factors = ['market', 'size', 'value', 'momentum', 'quality', 'volatility']
    
    # Use specified factors or defaults
    factors_to_use = include_factors if include_factors else all_factors
    
    # Create factor definitions
    factor_definitions = {}
    for factor in factors_to_use:
        if factor == 'market':
            factor_definitions[factor] = {'type': 'market_index', 'source': 'market_returns'}
        elif factor == 'size':
            factor_definitions[factor] = {'type': 'size_sort', 'metric': 'market_cap', 'groups': 2}
        elif factor == 'value':
            factor_definitions[factor] = {'type': 'characteristic_sort', 'metric': 'book_to_price', 'groups': 5}
        elif factor == 'momentum':
            factor_definitions[factor] = {'type': 'characteristic_sort', 'metric': 'returns_12m_1m', 'groups': 5}
        elif factor == 'quality':
            factor_definitions[factor] = {'type': 'characteristic_sort', 'metric': 'roe', 'groups': 5}
        elif factor == 'volatility':
            factor_definitions[factor] = {'type': 'characteristic_sort', 'metric': 'volatility_12m', 'groups': 5}
    
    return {
        'model_type': 'equity_factor_model',
        'factors': factor_definitions,
        'regression_method': 'ols',
        'estimation_window': 36  # months
    }
```

### Factor Returns Calculation

For detailed methodologies on calculating factor returns, please refer to:

* [Factor Return Construction](./factor-return-construction.md)
* [Cross-Sectional Methods](./cross-sectional-methods.md)
* [Time-Series Methods](./time-series-methods.md)

### Equity Attribution Analysis

```python
def equity_factor_attribution(portfolio_returns, portfolio_characteristics, factor_returns):
    """
    Perform factor-based attribution for an equity portfolio.
    
    Parameters:
    portfolio_returns (Series): Portfolio returns time series
    portfolio_characteristics (DataFrame): Portfolio characteristics over time
    factor_returns (DataFrame): Factor returns time series
    
    Returns:
    DataFrame: Attribution results
    """
    # Initialize results
    results = {}
    
    # Calculate factor exposures using portfolio characteristics
    exposures = calculate_portfolio_exposures(portfolio_characteristics)
    
    # Calculate factor contributions
    factor_contributions = {}
    for factor in factor_returns.columns:
        if factor in exposures:
            factor_contributions[factor] = exposures[factor] * factor_returns[factor]
    
    # Calculate total factor contribution
    results['factor_contribution'] = pd.DataFrame(factor_contributions)
    results['total_factor_contribution'] = results['factor_contribution'].sum(axis=1)
    
    # Calculate security selection (residual)
    results['security_selection'] = portfolio_returns - results['total_factor_contribution']
    
    # Calculate total attribution
    results['total_attributed'] = results['total_factor_contribution'] + results['security_selection']
    
    # Calculate attribution statistics
    results['attribution_summary'] = calculate_attribution_statistics(results)
    
    return results
```

## Advanced Attribution Techniques

### Time-Varying Exposures

For portfolios with changing factor exposures over time:

```python
def time_varying_attribution(portfolio_returns, time_varying_exposures, factor_returns):
    """
    Perform attribution with time-varying factor exposures.
    
    Parameters:
    portfolio_returns (Series): Portfolio returns time series
    time_varying_exposures (DataFrame): Factor exposures over time
    factor_returns (DataFrame): Factor returns time series
    
    Returns:
    DataFrame: Attribution results with time-varying exposures
    """
    # Initialize results
    results = pd.DataFrame(index=portfolio_returns.index)
    
    # Calculate factor contributions at each time point
    for date in portfolio_returns.index:
        if date in time_varying_exposures.index and date in factor_returns.index:
            # Get exposures and factor returns for this date
            exposures = time_varying_exposures.loc[date]
            factors = factor_returns.loc[date]
            
            # Calculate factor contributions
            for factor in factors.index:
                if factor in exposures:
                    column_name = f"{factor}_contribution"
                    results.loc[date, column_name] = exposures[factor] * factors[factor]
    
    # Calculate total factor contribution
    factor_columns = [col for col in results.columns if col.endswith('_contribution')]
    results['total_factor_contribution'] = results[factor_columns].sum(axis=1)
    
    # Calculate security selection
    results['security_selection'] = portfolio_returns - results['total_factor_contribution']
    
    return results
```

For more detailed techniques on handling time-varying exposures, see:

* [Dynamic Factor Exposures](./dynamic-factor-exposures.md)
* [Conditional Factor Models](./conditional-factor-models.md)

### Benchmark-Relative Attribution

```python
def benchmark_relative_attribution(portfolio_returns, benchmark_returns, 
                                  portfolio_exposures, benchmark_exposures,
                                  factor_returns):
    """
    Perform benchmark-relative factor attribution.
    
    Parameters:
    portfolio_returns (Series): Portfolio returns
    benchmark_returns (Series): Benchmark returns
    portfolio_exposures (Series): Portfolio factor exposures
    benchmark_exposures (Series): Benchmark factor exposures
    factor_returns (DataFrame): Factor returns
    
    Returns:
    dict: Benchmark-relative attribution results
    """
    # Calculate active returns
    active_returns = portfolio_returns - benchmark_returns
    
    # Calculate active exposures
    active_exposures = {}
    for factor in portfolio_exposures.index:
        if factor in benchmark_exposures:
            active_exposures[factor] = portfolio_exposures[factor] - benchmark_exposures[factor]
    
    # Calculate factor contributions to active return
    active_factor_contributions = {}
    for factor, active_exposure in active_exposures.items():
        active_factor_contributions[factor] = active_exposure * factor_returns[factor]
    
    # Calculate total factor contribution to active return
    total_active_factor = sum(active_factor_contributions.values())
    
    # Calculate security selection (residual)
    security_selection = active_returns - total_active_factor
    
    return {
        'active_return': active_returns,
        'active_exposures': active_exposures,
        'active_factor_contributions': active_factor_contributions,
        'total_active_factor': total_active_factor,
        'security_selection': security_selection
    }
```

## Practical Applications

### Performance Evaluation

Using factor attribution to evaluate portfolio performance:

1. **Style Consistency**: Assessing consistency of factor exposures over time
2. **Factor Timing**: Evaluating the impact of changing factor exposures
3. **Security Selection**: Measuring the value added beyond factor exposures
4. **Risk-Adjusted Performance**: Evaluating performance in context of factor risks

### Portfolio Construction

Applying factor attribution insights to portfolio construction:

1. **Factor Targeting**: Deliberately targeting specific factor exposures
2. **Exposure Neutralization**: Neutralizing unwanted factor exposures
3. **Alpha Maximization**: Focusing on securities with high specific returns
4. **Risk Control**: Managing factor-based risk exposures

## Implementation Considerations

### Data Requirements

Essential data for equity factor attribution:

1. **Returns Data**: Historical returns for portfolio, benchmark, and factors
2. **Holdings Data**: Detailed portfolio and benchmark holdings over time
3. **Security Characteristics**: Fundamental and market data for securities
4. **Factor Definitions**: Clear and consistent factor specifications
5. **Classification Data**: Sector, industry, and country classifications

### Common Challenges

Typical challenges in equity factor attribution:

1. **Factor Multicollinearity**: Correlation between factors
2. **Time-Varying Behavior**: Changing factor behavior over time
3. **Outlier Effects**: Impact of extreme returns on factor estimates
4. **Data Quality**: Consistency and completeness of security data
5. **Turnover Effects**: Impact of trading activity on attribution

## VeritasVault Implementation

VeritasVault provides comprehensive equity factor attribution tools:

1. **Equity Factor Models**: Pre-built models for major markets
2. **Custom Factor Builder**: Tools for custom factor definition
3. **Time-Varying Analysis**: Support for dynamic factor exposures
4. **Benchmark-Relative Analysis**: Detailed active return decomposition
5. **Integration with Holdings**: Connection with detailed holdings data

## Related Documentation

* [Factor Attribution Overview](./factor-attribution-overview.md)
* [Performance Attribution Overview](./performance-attribution-overview.md)
* [Equity Factor Models](../equity-factors/equity-factor-models.md)
* [Multi-Factor Models](../multi-factor/multi-factor-models.md)