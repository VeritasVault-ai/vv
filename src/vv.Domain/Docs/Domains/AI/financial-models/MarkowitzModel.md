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

# Markowitz Mean-Variance Optimization Model

> Classical portfolio optimization based on the efficient frontier

---

## 1. Overview

The Markowitz Mean-Variance Optimization model, based on Modern Portfolio Theory (MPT), provides a mathematical framework for constructing portfolios that maximize expected return for a given level of risk, or minimize risk for a given level of expected return. This implementation delivers a robust and efficient solution for portfolio optimization within the VeritasVault platform.

## 2. Key Concepts

### Efficient Frontier

* **Definition:** The set of optimal portfolios that offer the highest expected return for a defined level of risk
* **Calculation:** Derived by varying the risk aversion parameter across the optimization problem
* **Visualization:** Concave curve in risk-return space

### Risk-Return Tradeoff

* **Expected Return:** Weighted average of individual asset expected returns
* **Portfolio Risk:** Function of asset volatilities and their correlations
* **Diversification Benefit:** Risk reduction through imperfectly correlated assets

### Utility Function

* **Risk Aversion Parameter:** Controls the investor's preference between risk and return
* **Quadratic Utility:** Models investor preference with diminishing marginal utility
* **Optimal Portfolio:** Maximizes the utility function given investor preferences

## 3. Mathematical Framework

### Core Formulation

```python
# Pseudocode representation of key calculations

# 1. Portfolio Expected Return
E[R_p] = w^T * μ
    where:
    E[R_p] = Expected portfolio return
    w = Vector of portfolio weights
    μ = Vector of expected returns

# 2. Portfolio Variance
σ²_p = w^T * Σ * w
    where:
    σ²_p = Portfolio variance
    Σ = Covariance matrix of asset returns

# 3. Optimization Problem
Maximize: w^T * μ - (λ/2) * w^T * Σ * w
Subject to: sum(w) = 1, w_i ≥ 0 (and other constraints)
    where:
    λ = Risk aversion parameter
```

### Key Parameters

| Parameter | Description | Typical Values | Governance |
|-----------|-------------|----------------|------------|
| μ (mu) | Expected returns vector | Asset-specific | ReturnEstimator |
| Σ (sigma) | Covariance matrix | Asset-specific | CovarianceEstimator |
| λ (lambda) | Risk aversion | 1.0-10.0 | User configurable |

### Constraints Framework

* **Equality Constraints:** Sum of weights equals 1.0, target allocations
* **Inequality Constraints:** Position limits, sector exposure limits
* **Custom Constraints:** Tracking error, beta targets, factor exposures

## 4. Implementation Details

### Optimization Algorithm

* **Quadratic Programming:** For unconstrained or linearly constrained problems
* **Sequential Quadratic Programming:** For non-linear constraints
* **Interior Point Methods:** For large-scale optimization problems

### Numerical Considerations

* **Matrix Conditioning:** Techniques to ensure numerical stability
* **Constraint Handling:** Efficient encoding of portfolio constraints
* **Convergence Criteria:** Robust stopping conditions for iterative solvers

### Risk Adjustments

* **Shrinkage Estimators:** For robust covariance estimation
* **Resampling Techniques:** For sensitivity analysis
* **Stress Testing:** For portfolio behavior under extreme conditions

## 5. Integration with VeritasVault

### Core Infrastructure

* Utilizes TimeSeriesStore for historical return data
* Leverages ComputeOrchestrator for matrix operations
* Implements ConstraintManager for portfolio restrictions

### Process Flow

1. **Data Acquisition:**
   * Retrieve historical returns from TimeSeriesStore
   * Process and validate return data
   * Handle missing data and outliers

2. **Parameter Estimation:**
   * Calculate expected returns using configured method
   * Estimate covariance matrix with robust techniques
   * Apply any adjustment or shrinkage methods

3. **Constraint Setup:**
   * Apply universal constraints (sum to 1, etc.)
   * Import user-specific constraints
   * Validate constraint feasibility

4. **Optimization:**
   * Execute quadratic optimization
   * Generate efficient frontier points
   * Calculate optimal portfolio for given risk aversion

5. **Result Processing:**
   * Store optimization results
   * Calculate risk/return metrics
   * Generate visualization data

### Integration Points

* **User Interface:**
  * Risk preference selection
  * Constraint specification
  * Efficient frontier visualization

* **Other Financial Models:**
  * Can use Black-Litterman outputs as inputs
  * Supports comparison with other optimization methods
  * Can feed into risk management systems

## 6. Performance Considerations

### Computational Efficiency

* Matrix operations optimized for large asset universes
* Sparse matrix representation for efficient memory usage
* Warm-start capabilities for related optimization problems

### Scalability

* Supports parallel computation for frontier generation
* Efficient handling of large constraint sets
* Performance degrades gracefully with problem size

### Benchmarks

| Asset Count | Constraints | Typical Runtime | Memory Usage |
|-------------|-------------|----------------|--------------|
| 50 | Basic | < 0.5s | < 100MB |
| 100 | Standard | < 2s | < 250MB |
| 500 | Complex | < 10s | < 1GB |
| 1000+ | Advanced | < 60s | < 4GB |

## 7. Best Practices

### Input Preparation

* Use sufficient historical data (3-5 years recommended)
* Apply robust estimation methods for expected returns
* Consider factor models for improved covariance estimation
* Validate all inputs before optimization

### Constraint Design

* Start with basic constraints and add complexity as needed
* Ensure constraints are not overly restrictive
* Test constraint feasibility before full optimization
* Document rationale for each constraint

### Result Interpretation

* Examine sensitivity to input parameters
* Consider multiple points on the efficient frontier
* Evaluate practical implementation considerations
* Compare with benchmark and alternative allocations

## 8. References & Resources

### Internal Documentation

* [Portfolio Optimization Framework](./PortfolioOptimization.md)
* [Time Series Data Management](../time-series-management.md)
* [Covariance Estimation Techniques](../../AI/covariance-estimation.md)

### External References

* Markowitz, H. (1952). Portfolio Selection. The Journal of Finance, 7(1), 77-91.
* Markowitz, H. (1959). Portfolio Selection: Efficient Diversification of Investments. John Wiley & Sons.
* Merton, R. C. (1972). An Analytic Derivation of the Efficient Portfolio Frontier. Journal of Financial and Quantitative Analysis, 7(4), 1851-1872.
* Jagannathan, R., & Ma, T. (2003). Risk Reduction in Large Portfolios: Why Imposing the Wrong Constraints Helps. The Journal of Finance, 58(4), 1651-1683.

---

## 9. Document Control

* **Owner:** Financial Modeling Lead
* **Last Updated:** 2025-05-29
* **Status:** Draft