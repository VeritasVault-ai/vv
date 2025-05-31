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

# CVaR: Mathematical Definition

> Mathematical foundations of Conditional Value-at-Risk

---

## Formal Definition

Conditional Value-at-Risk at confidence level α over time horizon T is defined as:

* **Formal Definition**: CVaR(α) = E[L | L ≥ VaR(α)]
  * Where L represents the loss and E[·|·] is the conditional expectation
* **Alternative Definition**: CVaR(α) is the expected loss in the worst (1-α)% of cases
* **Typical Parameters**: Same as VaR - α = 95%, 99%, or 99.5% confidence level

## Relationship to VaR

* **Magnitude**: CVaR is always greater than or equal to VaR at the same confidence level
* **Information Content**: CVaR provides information about the entire tail beyond VaR
* **Nested Property**: CVaR at confidence level α includes information about VaR at all higher confidence levels

## Mathematical Properties

* **Subadditivity**: CVaR(A+B) ≤ CVaR(A) + CVaR(B)
* **Positive Homogeneity**: CVaR(λA) = λCVaR(A) for λ > 0
* **Monotonicity**: If A ≤ B in all states, then CVaR(A) ≤ CVaR(B)
* **Translation Invariance**: CVaR(A+c) = CVaR(A) + c
* **Coherence**: Satisfies all four properties required for coherent risk measures

## Related Documents

* [Conditional Value-at-Risk Overview](../conditional-value-at-risk.md)
* [CVaR Implementation Methodologies](./cvar-implementation-methodologies.md)
* [Value-at-Risk (VaR)](../value-at-risk.md)