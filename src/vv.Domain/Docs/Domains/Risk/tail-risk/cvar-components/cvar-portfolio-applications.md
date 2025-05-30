# CVaR: Portfolio Applications

> Using Conditional Value-at-Risk in portfolio construction and analysis

---

## CVaR Optimization

* **Objective**: Minimize CVaR for given expected return
* **Formula**: min CVaR(α) subject to E[R] ≥ target and other constraints
* **Implementation**: Linear programming formulation available
* **Advantages**: Produces portfolios with better tail behavior
* **Differences from Mean-Variance**: Less concentration, better diversification

## CVaR Contribution Analysis

* **Definition**: Decomposition of total CVaR into individual position contributions
* **Formula**: CVaR contribution of asset i = E[w(i) × r(i) | L ≥ VaR(α)] / CVaR
* **Applications**: Risk budgeting, position sizing
* **Advantages**: Identifies true tail risk contributors

## CVaR-Based Performance Evaluation

* **Measures**: Variants of Sharpe ratio using CVaR as risk measure
* **Formula**: STARR ratio = (r̄ - r_f) / CVaR
* **Applications**: Evaluating strategies with asymmetric return profiles
* **Advantages**: Better assessment of strategies with tail risk

## Implementation Example: CVaR Optimization

```python
import numpy as np
import cvxpy as cp

def cvar_portfolio_optimization(returns, target_return, confidence=0.95, risk_free_rate=0):
    """
    Optimize portfolio to minimize CVaR subject to target return
    
    Parameters:
    returns (matrix): Historical returns for each asset (rows=time, cols=assets)
    target_return (float): Target portfolio return
    confidence (float): Confidence level for CVaR
    risk_free_rate (float): Risk-free rate
    
    Returns:
    array: Optimal portfolio weights
    """
    n_samples, n_assets = returns.shape
    
    # Decision variables
    weights = cp.Variable(n_assets)
    aux_var = cp.Variable()  # VaR
    slack_vars = cp.Variable(n_samples)
    
    # Objective: Minimize CVaR
    alpha = 1 - confidence
    objective = cp.Minimize(aux_var + (1/(alpha*n_samples)) * cp.sum(slack_vars))
    
    # Calculate scenario losses
    scenario_losses = -returns @ weights
    
    # Constraints
    constraints = [
        # CVaR constraints
        slack_vars >= 0,
        slack_vars >= scenario_losses - aux_var,
        
        # Portfolio constraints
        cp.sum(weights) == 1,
        weights >= 0,  # No short selling
        
        # Return constraint
        weights @ np.mean(returns, axis=0) >= target_return
    ]
    
    # Solve the problem
    problem = cp.Problem(objective, constraints)
    problem.solve()
    
    return weights.value
```

## Related Documents

* [Conditional Value-at-Risk Overview](../conditional-value-at-risk.md)
* [CVaR Mathematical Definition](./cvar-mathematical-definition.md)
* [CVaR Practical Considerations](./cvar-practical-considerations.md)