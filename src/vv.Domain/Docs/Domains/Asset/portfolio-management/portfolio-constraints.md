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

# Portfolio Constraints

> Implementation and management of constraints in portfolio optimization

---

## Overview

Portfolio constraints are restrictions applied during the portfolio construction process to ensure the resulting portfolios meet specific requirements. These constraints are essential for creating practical, implementable portfolios that adhere to investment guidelines, regulatory requirements, and risk management principles.

## Types of Constraints

### Hard vs. Soft Constraints

* **Hard Constraints**: Must be strictly satisfied (e.g., no short selling)
* **Soft Constraints**: Preferences that can be violated with penalties (e.g., sector neutrality)

### Common Portfolio Constraints

#### Investment Boundary Constraints

* **Long-Only Constraint**: All asset weights must be non-negative (w<sub>i</sub> ≥ 0)
* **Full Investment**: Sum of all weights equals 100% (Σw<sub>i</sub> = 1)
* **Box Constraints**: Minimum and maximum bounds on individual asset weights (L<sub>i</sub> ≤ w<sub>i</sub> ≤ U<sub>i</sub>)
* **Short Constraints**: Restrictions on short positions (e.g., 130/30 strategies)
* **Leverage Constraints**: Limits on total gross exposure (Σ|w<sub>i</sub>| ≤ L)

#### Group Constraints

* **Sector/Industry Constraints**: Limits on exposure to specific sectors or industries
* **Country/Region Constraints**: Limits on geographical exposures
* **Asset Class Constraints**: Restrictions on allocation across different asset classes
* **Style Constraints**: Limits on exposure to specific investment styles or factors
* **Issuer Constraints**: Limits on exposure to individual issuers or related entities

#### Risk-Based Constraints

* **Volatility Constraints**: Maximum portfolio volatility (σ<sub>p</sub> ≤ σ<sub>max</sub>)
* **Beta Constraints**: Restrictions on portfolio beta (β<sub>min</sub> ≤ β<sub>p</sub> ≤ β<sub>max</sub>)
* **Tracking Error Constraints**: Maximum deviation from benchmark (TE ≤ TE<sub>max</sub>)
* **Value-at-Risk Constraints**: Maximum allowable Value-at-Risk
* **Factor Exposure Constraints**: Limits on exposure to specific risk factors

#### Turnover and Trading Constraints

* **Maximum Turnover**: Limits on portfolio turnover per rebalancing period
* **Transaction Cost Constraints**: Incorporation of transaction costs in optimization
* **Minimum Trade Size**: Minimum trade size to avoid small, uneconomical trades
* **Market Impact Constraints**: Consideration of market impact for large trades
* **Liquidity Constraints**: Restrictions based on asset liquidity profiles

## Mathematical Representation

Portfolio constraints can be mathematically represented in different forms:

### Linear Equality Constraints

* **Formula**: Aw = b
* **Example**: Full investment constraint (1'w = 1)

### Linear Inequality Constraints

* **Formula**: Cw ≤ d
* **Example**: Sector exposure constraint (w<sub>sector</sub> ≤ 0.25)

### Quadratic Constraints

* **Formula**: w'Qw ≤ q
* **Example**: Volatility constraint (w'Σw ≤ σ<sub>max</sub><sup>2</sup>)

### Non-Linear Constraints

* **Formula**: f(w) ≤ c
* **Example**: Value-at-Risk constraint

## Implementation Approaches

### Constrained Optimization Methods

* **Quadratic Programming**: For mean-variance optimization with linear constraints
* **Second-Order Cone Programming**: For problems with quadratic constraints
* **Sequential Quadratic Programming**: For problems with non-linear constraints
* **Mixed Integer Programming**: For problems with integer constraints

### Penalty Methods

* **Penalty Functions**: Incorporate soft constraints through penalty terms
* **Lagrangian Relaxation**: Incorporate constraints through Lagrange multipliers
* **Augmented Lagrangian**: Enhanced version of Lagrangian relaxation

### Projection Methods

* **Constraint Projection**: Project unconstrained solution onto constraint set
* **Alternating Projection**: Iteratively project onto different constraint sets
* **Dykstra's Projection Algorithm**: Modified projection for non-orthogonal constraints

## Constraints in Different Portfolio Methodologies

### Mean-Variance Optimization

* **Key Constraints**: Full investment, long-only, asset bounds, group constraints
* **Implementation**: Typically quadratic programming with linear constraints
* **Challenge**: Constraints can significantly impact optimal allocation

### Risk-Based Portfolios

* **Key Constraints**: Long-only, asset bounds, sector/factor neutrality
* **Implementation**: Often requires specialized algorithms
* **Challenge**: Some constraints may conflict with risk-based objectives

### Factor Portfolios

* **Key Constraints**: Factor exposure, tracking error, sector neutrality
* **Implementation**: Factor-aware optimization frameworks
* **Challenge**: Balancing factor exposure against constraints

### Black-Litterman Portfolios

* **Key Constraints**: Similar to mean-variance plus view-specific constraints
* **Implementation**: Incorporates constraints in posterior optimization
* **Challenge**: Ensuring views and constraints are consistent

## Constraint Management

### Constraint Feasibility

* **Feasibility Analysis**: Verifying that constraints do not create an empty feasible set
* **Constraint Relaxation**: Procedures for relaxing constraints when necessary
* **Prioritization**: Hierarchy of constraints when all cannot be satisfied simultaneously
* **Infeasibility Detection**: Methods to identify infeasible constraint combinations

### Constraint Sensitivity

* **Sensitivity Analysis**: Understanding impact of constraints on portfolio outcomes
* **Shadow Prices**: Measuring marginal value of relaxing constraints
* **Binding Constraints**: Identifying which constraints are binding at optimum
* **What-If Analysis**: Exploring alternative constraint specifications

## Regulatory and Policy Constraints

### Regulatory Framework

* **Investment Company Act Restrictions**: Diversification requirements for funds
* **UCITS Regulations**: European regulatory framework constraints
* **Basel Constraints**: Banking portfolio constraints
* **Insurance Regulation**: Constraints specific to insurance portfolios

### Policy Implementation

* **Investment Policy Statement**: Translating policy into explicit constraints
* **Governance Framework**: Process for establishing and modifying constraints
* **Compliance Monitoring**: Ensuring ongoing adherence to constraints
* **Constraint Documentation**: Clear documentation of all applied constraints

## VeritasVault Implementation

VeritasVault provides comprehensive tools for constraint management:

* **Constraint Library**: Pre-defined constraints with customization options
* **Constraint Validation**: Tools to verify constraint feasibility
* **Constraint Visualization**: Visual representation of constraint impact
* **Multi-Period Constraint Handling**: Managing constraints across time
* **Constraint Sensitivity Analysis**: Tools to analyze constraint effects

## Practical Considerations

* **Constraint Complexity**: Balance between complexity and tractability
* **Constraint Interaction**: Understanding how constraints interact with each other
* **Constraint Stability**: Avoiding unstable solutions due to tight constraints
* **Rebalancing Frequency**: Impact of constraints on optimal rebalancing frequency
* **Performance Impact**: Evaluating performance impact of different constraints

For more detailed information on implementing specific constraints or constraint handling in different optimization frameworks, refer to the specialized implementation guides.