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

# Holdings-Based Attribution

> Detailed performance attribution using portfolio and benchmark holdings

---

## Overview

Holdings-based attribution analyzes portfolio performance using detailed portfolio and benchmark holdings information. This approach provides a granular understanding of how specific allocation and selection decisions contributed to overall performance.

## Key Principles

Holdings-based attribution is built on these core principles:

1. **Holdings-Driven Analysis**: Uses actual security weights and returns
2. **Decision-Based Decomposition**: Separates effects of different investment decisions
3. **Hierarchical Analysis**: Can be performed at multiple levels (sector, country, security)
4. **Point-in-Time Accuracy**: Accounts for portfolio changes over time
5. **Direct Decision Evaluation**: Directly connects investment decisions to outcomes

## Brinson Attribution Models

### Brinson-Hood-Beebower (BHB) Model

The classic three-factor model decomposing returns into:

1. **Allocation Effect**: Impact of over/underweighting segments vs benchmark
   ```
   Allocation Effect = ∑ (w_p,i - w_b,i) × r_b,i
   ```

2. **Selection Effect**: Impact of security selection within segments
   ```
   Selection Effect = ∑ w_p,i × (r_p,i - r_b,i)
   ```

3. **Interaction Effect**: Combined impact of allocation and selection
   ```
   Interaction Effect = ∑ (w_p,i - w_b,i) × (r_p,i - r_b,i)
   ```

Where:
- w_p,i = portfolio weight in segment i
- w_b,i = benchmark weight in segment i
- r_p,i = portfolio return in segment i
- r_b,i = benchmark return in segment i

### Brinson-Fachler (BF) Model

A modification that attributes interaction effect to selection:

1. **Allocation Effect**: 
   ```
   Allocation Effect = ∑ (w_p,i - w_b,i) × (r_b,i - r_b)
   ```

2. **Selection Effect** (including interaction): 
   ```
   Selection Effect = ∑ w_p,i × (r_p,i - r_b,i)
   ```

Where r_b is the total benchmark return.

## Implementation Framework

### Single-Period Attribution

```python
def calculate_brinson_attribution(portfolio_weights, benchmark_weights, 
                                 portfolio_returns, benchmark_returns, model='BHB'):
    """
    Calculate Brinson attribution effects.
    
    Parameters:
    portfolio_weights (dict): Portfolio weights by segment
    benchmark_weights (dict): Benchmark weights by segment
    portfolio_returns (dict): Portfolio returns by segment
    benchmark_returns (dict): Benchmark returns by segment
    model (str): 'BHB' or 'BF' model
    
    Returns:
    dict: Attribution effects
    """
    segments = set(portfolio_weights.keys()) | set(benchmark_weights.keys())
    
    # Initialize effects
    allocation_effect = 0
    selection_effect = 0
    interaction_effect = 0
    
    # Total benchmark return (for BF model)
    if model == 'BF':
        total_benchmark_return = sum([benchmark_weights.get(segment, 0) * 
                                    benchmark_returns.get(segment, 0)
                                    for segment in segments])
    
    # Calculate effects by segment
    for segment in segments:
        pw = portfolio_weights.get(segment, 0)
        bw = benchmark_weights.get(segment, 0)
        pr = portfolio_returns.get(segment, 0)
        br = benchmark_returns.get(segment, 0)
        
        if model == 'BHB':
            # Brinson-Hood-Beebower
            allocation_effect += (pw - bw) * br
            selection_effect += pw * (pr - br)
            interaction_effect += (pw - bw) * (pr - br)
        elif model == 'BF':
            # Brinson-Fachler
            allocation_effect += (pw - bw) * (br - total_benchmark_return)
            selection_effect += pw * (pr - br)
    
    # Compile results
    results = {
        'allocation_effect': allocation_effect,
        'selection_effect': selection_effect
    }
    
    if model == 'BHB':
        results['interaction_effect'] = interaction_effect
        results['total_effect'] = allocation_effect + selection_effect + interaction_effect
    else:
        results['total_effect'] = allocation_effect + selection_effect
    
    return results
```

### Multi-Period Attribution

For accurate multi-period attribution, specialized techniques are needed to link effects over time. Due to their complexity, these are covered in a dedicated document:

* [Multi-Period Attribution Techniques](./multi-period-attribution.md)

## Advanced Holdings-Based Methods

### Transaction-Based Attribution

Incorporates actual transactions to capture timing effects:

1. **Buy/Sell Timing**: Effect of purchase and sale timing
2. **Trading Costs**: Impact of transaction costs
3. **Cash Drag**: Impact of cash holdings versus full investment

### Fixed Income Attribution

Specialized models for bond portfolios:

1. **Yield Curve Attribution**: Effect of yield curve movements
2. **Spread Attribution**: Effect of spread changes
3. **Income Attribution**: Effect of yield income

For detailed information on these specialized methods, see:

* [Fixed Income Attribution](./fixed-income-attribution.md)
* [Transaction-Based Attribution](./transaction-attribution.md)

## Practical Considerations

### Data Requirements

Critical data needed for accurate attribution:

1. **Complete Holdings**: Full portfolio and benchmark holdings
2. **Point-in-Time Data**: Holdings as of analysis dates
3. **Accurate Pricing**: Consistent pricing for all securities
4. **Proper Classification**: Correct segment assignment
5. **Corporate Actions**: Adjusted for dividends, splits, etc.

### Implementation Challenges

Common challenges and solutions:

1. **Data Quality**: Implement data validation and cleansing
2. **Security Mapping**: Ensure consistent security identifiers
3. **Benchmark Construction**: Create appropriate custom benchmarks if needed
4. **Classification Consistency**: Maintain consistent classification scheme
5. **Performance Calculation**: Ensure accurate return calculations

## VeritasVault Implementation

VeritasVault provides comprehensive holdings-based attribution capabilities:

1. **Holdings Analysis Engine**: Core calculation of attribution effects
2. **Multi-Model Support**: Implementation of various attribution models
3. **Hierarchical Attribution**: Analysis at different classification levels
4. **Transaction Integration**: Incorporation of transaction data
5. **Multi-Period Analysis**: Linking of effects across time periods

## Related Documentation

* [Performance Attribution Overview](./performance-attribution-overview.md)
* [Returns-Based Attribution](./returns-based-attribution.md)
* [Factor Attribution Overview](./factor-attribution-overview.md)
* [Equity Factor Attribution](./equity-factor-attribution.md)