---
document_type: portfolio-optimization-guide
classification: internal
status: draft
version: 1.0.0
last_updated: "2025-05-31"
applies_to: [asset-trading-settlement]
dependencies:
  - ../../AI/black-litterman-ai/bl-ai-overview.md
  - ../../AI/ai-architecture.md
  - ../settlement-protocol.md
  - ../../SECURITY.md
  - ../../Integration/FinancialModels/PortfolioOptimization.md
reviewers: [trading-lead, ai-lead, risk-lead, secops-lead]
next_review: "2026-05-31"
priority: p1
---

# Portfolio Optimization Guide  

> **Asset, Trading & Settlement Domain** – Institutional-grade portfolio construction integrating AI-assisted financial models.

---

## 1. Overview

The Portfolio Optimization module delivers **automated, auditable, and constraint-aware** construction of multi-asset portfolios.  
Key capabilities:

* Integration with AI/ML Black-Litterman view generation  
* Classical Markowitz mean-variance optimisation (MVO)  
* Risk-parity & Equal Risk Contribution (ERC) allocations  
* Hard & soft constraint handling (position, sector, factor, ESG)  
* Deterministic rebalancing flows tied to the **[Settlement Protocol](settlement/settlement-protocol.md)**  
* Real-time performance analytics and risk monitoring  

All optimisation engines expose uniform interfaces allowing strategy selection at runtime through governance parameters.

---

## 2. Black-Litterman Integration (AI/ML Domain)

| Component (AI/ML) | Interaction |
|-------------------|-------------|
| **ViewGenerator** | Generates investor/AI sentiment views (`InvestorView`) signed & versioned |
| **CovarianceEstimator** | Supplies shrinkage-enhanced covariance matrices |
| **ParameterOptimizer** | Derives tau, risk-aversion, and blending parameters |
| **BlackLittermanProcessor** | Returns posterior returns vector `π*` passed to Asset domain |

Flow:

1. `InvestorView` & market caps streamed via MessageBus `ai.bl_views`  
2. `BlackLittermanProcessor.process()` outputs **posterior expected returns** and **Σ**  
3. Asset domain consumes output, runs constraint handling (see §6), produces weight vector `w_bl`  
4. Result persisted & emitted as `PortfolioOptimized` event (audit-signed)  

---

## 3. Markowitz Mean-Variance Optimisation

*Objective*:  
\[
\min_{w} w^T \Sigma w - \lambda \, \mu^T w
\]

Where  

* \( w \) – asset weights  
* \( \Sigma \) – covariance matrix  
* \( \mu \) – expected returns (AI-assisted or historical)  
* \( \lambda \) – risk-aversion (governance parameter)

Algorithm options:

1. **Closed-form** (no constraints)  
2. **Quadratic Programming** via interior-point solver (supports inequality constraints)  
3. **Resampled Efficiency** (Michaud) for robustness – optional flag

---

## 4. Risk Parity & Equal Risk Contribution (ERC)

Risk contribution of asset *i*:

\[
RC_i = \frac{w_i (\Sigma w)_i}{w^T \Sigma w}
\]

Algorithms:

| Method | Description | Complexity |
|--------|-------------|------------|
| **Naïve RP** | Equalises \( RC_i \) using gradient descent | O(n²) |
| **ERC-QP** | QP formulation with target \( RC_i = 1/n \) | O(n³) |
| **ERC-Newton** | Newton-Raphson root-finding | O(n²) per iter |

Selectable via `optimizerType` enum: `MARKOWITZ`, `BLACK_LITTERMAN`, `ERC`.

---

## 5. Constraint Handling

| Constraint Type | Example | Enforcement |
|-----------------|---------|-------------|
| **Position Limit** | \( w_i \le 10\% \) | Bound constraints in solver |
| **Sector Limit** | Tech ≤ 30 % | Aggregation matrix \( A w \le b \) |
| **Tracking Error** | σ\_tracking ≤ 2 % | Quadratic penalty term |
| **ESG Score** | Portfolio score ≥ 70 | Linear inequality |
| **Liquidity** | Daily turnover ≤ 10 % ADV | Pre-trade filter + soft penalty |

Constraint sets are provided as **`ConstraintSet`** objects (see Integration domain spec) and are versioned & audit-logged.

---

## 6. Portfolio Rebalancing Mechanisms

| Trigger | Description | Implementation |
|---------|-------------|----------------|
| **Calendar** | Fixed schedule (e.g., monthly) | Cron-like TaskScheduler job |
| **Drift** | Deviation from target weights > threshold | Real-time monitor compares holdings vs target |
| **Risk Spike** | Volatility > risk budget | Risk domain alert triggers optimisation run |
| **Signal** | AI/ML model emits `RebalanceSignal` | MessageBus `ai.rebalance` topic |

Rebalance produces `RebalanceInstruction` entities routed to **Settlement Controller** for atomic execution.

---

## 7. Integration with Settlement Protocol

1. **RebalanceInstruction** converted into trade legs  
2. Trades locked in escrow (`EscrowLocked`) → `PendingFinality`  
3. On `SettlementFinalized`, new holdings recorded; Audit system logs hash chain  
4. Failure triggers automatic rollback per settlement design

This guarantees optimisation outputs are faithfully realised on-chain.

---

## 8. Performance Monitoring & Analytics

Metrics (streamed to `analytics.portfolio`):

* Tracking error, Sharpe, Sortino, Beta  
* Realised vs target weights drift  
* p95 optimisation runtime, solver iterations  
* Slippage & settlement latency  
* Risk contribution by asset/sector

Dashboards in Integration domain provide real-time KPI visualisation; anomalies raise alerts via SIEM.

---

## 9. Security Considerations

All controls in **[`../../SECURITY.md`](../../SECURITY.md)** apply. Specific points:

* **Model Tampering** – Black-Litterman inputs must carry EIP-712 signatures; verified before use  
* **Optimiser Integrity** – Deterministic builds; hash pinned in `ModelRegistry`  
* **Circuit Breakers** – Optimiser halted if risk metrics exceed thresholds  
* **Audit Events** – `PortfolioOptimized`, `RebalanceInstructionIssued`, `ConstraintBreached` logged and hash-chained  
* **Multi-Sig Governance** – Parameter & constraint changes require DAO approval with 48 h timelock  

---

## 10. References

* Integration Domain specs:  
  * [`PortfolioOptimization.md`](../AI/financial-models/PortfolioOptimization.md)  
  * [`BlackLitterman-Integration.md`](../AI/FinancialModels/BlackLitterman-Integration.md)  
* AI/ML Domain:  
  * [`black-litterman-ai/bl-ai-overview.md`](../AI/black-litterman-ai/bl-ai-overview.md)  
  * [`ai-architecture.md`](../AI/ai-architecture.md)  
* Risk Parity techniques – Maillard et al. (2010)  
* Mean-Variance – Markowitz (1952)  

---

*Prepared by Factory Assistant – 2025-05-30*  
