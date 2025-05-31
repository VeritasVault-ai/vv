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

# Conditional Value-at-Risk (CVaR)

> Expected loss beyond the VaR threshold

---

## Overview

Conditional Value-at-Risk (CVaR), also known as Expected Shortfall (ES) or Expected Tail Loss (ETL), measures the expected loss of a portfolio in the worst cases that exceed the Value-at-Risk threshold. By focusing on the average of extreme losses rather than just their frequency, CVaR provides a more comprehensive view of tail risk than traditional VaR.

## Key Principles

Conditional Value-at-Risk is built on these fundamental principles:

* **Beyond VaR**: Measures severity of tail events beyond the VaR threshold
* **Conditional Expectation**: Average loss given that loss exceeds VaR
* **Coherent Risk Measure**: Satisfies mathematical properties of coherent risk measures
* **Tail Focus**: Explicitly accounts for shape and heaviness of distribution tails
* **Risk Management**: Better suited for managing extreme event risk

## Quick Definition

CVaR at confidence level α is defined as the expected loss given that the loss exceeds the VaR threshold:

```
CVaR(α) = E[L | L ≥ VaR(α)]
```

Where:
- L represents the loss
- E[·|·] is the conditional expectation
- VaR(α) is the Value-at-Risk at confidence level α

## Detailed Documentation

This overview is supplemented by detailed documentation on specific aspects of CVaR:

### Theoretical Foundation
* [Mathematical Definition](./cvar-components/cvar-mathematical-definition.md) - Formal definition and mathematical properties

### Implementation
* [Implementation Methodologies](./cvar-components/cvar-implementation-methodologies.md) - Different calculation approaches
* [Advanced Estimation Techniques](./cvar-components/cvar-advanced-estimation.md) - EVT, kernel, and Bayesian methods

### Applications
* [Portfolio Applications](./cvar-components/cvar-portfolio-applications.md) - Optimization, contribution analysis, and performance evaluation

### Practical Aspects
* [Practical Considerations](./cvar-components/cvar-practical-considerations.md) - Backtesting, stability, and parameter selection
* [Regulatory Perspective](./cvar-components/cvar-regulatory-perspective.md) - Basel and other regulatory frameworks
* [Advantages and Limitations](./cvar-components/cvar-advantages-limitations.md) - Benefits, challenges, and case studies

## Visual Representation

CVaR represents the average loss in the worst cases beyond the VaR threshold:

```
    Probability
    Density
    │
    │              ┌─────── CVaR = average loss
    │              │        in this region
    │              │
    │              │
    │       VaR────┘
    │       │
    │       │
    │       │
    │      /│\
    │     / │ \    Tail region
    │    /  │  \   (worst α% of outcomes)
    │___/___│___\____________________
        Loss →
```

## Related Documents

* [Value-at-Risk (VaR)](./value-at-risk.md) - Threshold-based risk measure
* [Extreme Value Theory](./extreme-value-theory.md) - Advanced modeling of extreme events
* [Tail Risk Overview](./tail-risk-overview.md) - General introduction to tail risk measures