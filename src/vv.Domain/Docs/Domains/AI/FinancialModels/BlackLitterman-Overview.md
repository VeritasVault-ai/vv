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

# Black-Litterman Portfolio Optimization Model: Overview

> Advanced asset allocation framework integrating market equilibrium with investor views

---

## 1. Overview

The Black-Litterman (BL) model is an advanced portfolio optimization framework that extends traditional mean-variance optimization by combining market equilibrium expected returns with investor-specific views. This implementation provides a robust, secure, and auditable way to optimize portfolio allocation across the VeritasVault platform.

## 2. Key Concepts

### Market Equilibrium

* **Implied Equilibrium Returns:** Derived from market capitalization weights, assuming markets are in equilibrium
* **Reference Portfolio:** Typically market-cap weighted portfolio serving as the neutral starting point
* **Risk Aversion Parameter:** Global risk tolerance parameter that affects the magnitude of expected returns

### Investor Views

* **View Specification:** Investor beliefs about expected returns, either absolute or relative
* **View Confidence:** Uncertainty levels associated with each view
* **View Matrix:** Mathematical representation of investor views for the model

### Combined Estimation

* **Posterior Returns:** Blended expected returns incorporating both equilibrium and views
* **Posterior Covariance:** Adjusted covariance matrix reflecting uncertainty in views
* **Optimal Weights:** Portfolio weights derived from the posterior estimates

---

**Related Documentation:**
* [Black-Litterman Implementation](./BlackLitterman-Implementation.md)
* [Black-Litterman Integration](./BlackLitterman-Integration.md)
* [Black-Litterman Reference](./BlackLitterman-Reference.md)