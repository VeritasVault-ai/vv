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

# CVaR: Implementation Methodologies

> Different approaches for calculating Conditional Value-at-Risk

---

## Parametric Approach

* **Assumption**: Returns follow a specific distribution (typically normal)
* **Formula**: For normal distribution: CVaR = μ + σ × φ(Φ⁻¹(1-α))/(1-α)
  * Where φ is the standard normal PDF, Φ⁻¹ is the inverse standard normal CDF
* **Process**:
  1. Estimate distribution parameters
  2. Calculate VaR at confidence level α
  3. Compute expected value beyond VaR threshold
* **Advantages**: Analytical solution for many distributions
* **Limitations**: Highly sensitive to distribution assumptions

## Historical Simulation Method

* **Assumption**: Historical returns represent future return distribution
* **Process**:
  1. Collect historical return series of appropriate length
  2. Sort returns from worst to best
  3. Identify returns beyond the VaR threshold
  4. Calculate average of those extreme returns
* **Advantages**: No distributional assumptions, captures actual market behavior
* **Limitations**: Limited by available history, sensitive to outliers

## Monte Carlo Simulation Method

* **Process**:
  1. Specify stochastic process for risk factors
  2. Generate large number of random scenarios
  3. Calculate portfolio value under each scenario
  4. Identify scenarios beyond VaR threshold
  5. Compute average loss in those scenarios
* **Advantages**: Flexible modeling, handles complex securities and distributions
* **Limitations**: Computationally intensive, model specification risk

## Implementation Code Examples

```python
import numpy as np
from scipy import stats

def calculate_parametric_cvar(returns, confidence=0.95):
    """
    Calculate parametric CVaR assuming normal distribution
    """
    # Estimate parameters
    mu = np.mean(returns)
    sigma = np.std(returns, ddof=1)
    
    # Calculate alpha-quantile
    alpha_quantile = stats.norm.ppf(1 - confidence)
    
    # Calculate expected shortfall (CVaR)
    phi_alpha = stats.norm.pdf(alpha_quantile)
    cvar = mu + sigma * phi_alpha / (1 - confidence)
    
    return -cvar  # Return as positive value

def calculate_historical_cvar(returns, confidence=0.95):
    """
    Calculate historical CVaR
    """
    # Sort returns from worst to best
    sorted_returns = np.sort(returns)
    
    # Determine cutoff index
    n = len(returns)
    k = int(np.ceil(n * (1 - confidence)))
    
    # Calculate average of worst returns
    worst_returns = sorted_returns[:k]
    cvar = np.mean(worst_returns)
    
    return -cvar  # Return as positive value
```

## Related Documents

* [Conditional Value-at-Risk Overview](../conditional-value-at-risk.md)
* [CVaR Mathematical Definition](./cvar-mathematical-definition.md)
* [CVaR Advanced Estimation Techniques](./cvar-advanced-estimation.md)