---
document_type: domain-overview
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

# Tail Risk Measures: Overview

> Introduction to measures focusing on extreme market events

---

## Overview

Tail risk measures focus on quantifying the risk of extreme, low-probability events in the tails of return distributions. Unlike traditional risk measures that focus on average behavior, tail risk measures specifically address the severity and probability of significant losses that occur during market stress and crises.

## Tail Risk Measurement Map

Due to the complexity of tail risk measurement, this topic is divided into several specialized documents:

* **[Value-at-Risk (VaR)](./value-at-risk.md)**: Measures potential loss at a specific confidence level
* **[Conditional Value-at-Risk (CVaR)](./conditional-value-at-risk.md)**: Expected loss beyond VaR threshold
* **[Extreme Value Theory](./extreme-value-theory.md)**: Methods for modeling extreme events
* **[Stress Testing Approaches](../scenario-analysis/stress-testing.md)**: Scenario-based methods for assessing portfolio vulnerability

## Why Measure Tail Risk?

* **Fat-Tailed Returns**: Financial returns exhibit fatter tails than normal distributions
* **Black Swan Events**: Rare, high-impact events occur more frequently than predicted by normal models
* **Contagion Effects**: Correlations often increase during market stress
* **Risk Management**: Need to quantify potential extreme losses for risk control
* **Regulatory Requirements**: Many regulatory frameworks require tail risk assessment

## Key Considerations

When implementing tail risk measurement, consider:

* **Data Requirements**: Long historical series needed to capture extreme events
* **Model Risk**: Sensitivity to modeling assumptions
* **Estimation Error**: Challenge of estimating rare events with limited data
* **Dynamic Nature**: Changing tail behavior across market regimes
* **Implementation Complexity**: More complex than traditional risk measures

For detailed explanations of specific tail risk measures, refer to the specialized documents linked above.