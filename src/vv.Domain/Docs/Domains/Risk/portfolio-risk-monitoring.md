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

# Portfolio Risk Monitoring Guide

> Real-time and batch monitoring of portfolio risk metrics, limits, and exposures

---

## 1. Overview

The Portfolio Risk Monitoring framework establishes the methodologies, processes, and systems for continuous monitoring of all risk dimensions across VeritasVault portfolios. This document outlines the comprehensive approach to identifying, measuring, and responding to evolving risk profiles at both individual and aggregate portfolio levels.

## 2. Monitoring Framework

### Monitoring Dimensions

* **Risk Type Coverage:**
  * Market risk
  * Credit risk
  * Liquidity risk
  * Concentration risk
  * Operational risk
  * Model risk

* **Time Horizon Coverage:**
  * Intraday monitoring (real-time)
  * End-of-day analysis
  * Forward-looking projections
  * Historical trend analysis
  * Stress period analysis

* **Aggregation Levels:**
  * Individual position level
  * Strategy/portfolio level
  * Asset class level
  * Entity level
  * System-wide level

### Monitoring Frequency

| Risk Type | Real-time | Intraday | Daily | Weekly | Monthly |
|-----------|-----------|----------|-------|--------|---------|
| Market Risk - Core | ✓ | ✓ | ✓ | ✓ | ✓ |
| Market Risk - Comprehensive | | ✓ | ✓ | ✓ | ✓ |
| Credit Risk - Counterparty | ✓ | ✓ | ✓ | ✓ | ✓ |
| Credit Risk - Issuer | | ✓ | ✓ | ✓ | ✓ |
| Liquidity Risk - Trading | ✓ | ✓ | ✓ | ✓ | ✓ |
| Liquidity Risk - Funding | | | ✓ | ✓ | ✓ |
| Concentration Risk | | ✓ | ✓ | ✓ | ✓ |
| Operational Risk | ✓ | ✓ | ✓ | ✓ | ✓ |
| Model Risk | | | ✓ | ✓ | ✓ |

## 3. Risk Metrics and Indicators

### Market Risk Metrics

* **Value at Risk (VaR):**
  * Parametric VaR (95%, 99%)
  * Historical simulation VaR (95%, 99%)
  * Monte Carlo VaR (95%, 99%)
  * Expected Shortfall (95%, 99%)
  * Conditional VaR (95%, 99%)

* **Sensitivity Measures:**
  * Delta (price sensitivity)
  * Gamma (delta sensitivity)
  * Vega (volatility sensitivity)
  * Theta (time decay)
  * Rho (interest rate sensitivity)

* **Scenario Analysis:**
  * Historical stress scenarios
  * Hypothetical stress scenarios
  * Sensitivity stress tests
  * Reverse stress tests
  * Macroeconomic scenarios

### Credit Risk Metrics

* **Counterparty Exposure:**
  * Current exposure
  * Potential future exposure
  * Expected exposure profile
  * Credit valuation adjustment (CVA)
  * Wrong-way risk exposure

* **Default Risk:**
  * Probability of default (PD)
  * Loss given default (LGD)
  * Exposure at default (EAD)
  * Expected credit loss (ECL)
  * Credit risk capital

* **Credit Quality Indicators:**
  * Internal credit ratings
  * External ratings
  * Credit default swap spreads
  * Bond spread to benchmark
  * Market-implied ratings

### Liquidity Risk Metrics

* **Market Liquidity:**
  * Bid-ask spreads
  * Market depth
  * Trading volume ratios
  * Time to liquidation estimates
  * Liquidation cost estimates

* **Funding Liquidity:**
  * Liquidity coverage ratio
  * Net stable funding ratio
  * Cash flow projections
  * Funding concentration
  * Contingent funding needs

* **Asset Liquidity Classification:**
  * Highly liquid (0-1 day)
  * Liquid (2-7 days)
  * Less liquid (8-30 days)
  * Illiquid (>30 days)
  * Liquidation value haircuts

### Concentration Risk Metrics

* **Exposure Concentration:**
  * Single counterparty exposure
  * Industry/sector concentration
  * Geographic concentration
  * Asset class concentration
  * Risk factor concentration

* **Concentration Indices:**
  * Herfindahl-Hirschman Index
  * Gini coefficient
  * Concentration ratio (CR5, CR10)
  * Diversity score
  * Entropy measures

## 4. Limit Structure and Monitoring

### Limit Hierarchy

* **Enterprise Limits:**
  * Aggregate risk capacity
  * Risk appetite allocation
  * Capital allocation
  * Loss tolerance
  * Liquidity requirements

* **Portfolio Limits:**
  * VaR limits
  * Exposure limits
  * Concentration limits
  * Leverage limits
  * Liquidity limits

* **Trading Limits:**
  * Position size limits
  * Risk sensitivity limits
  * Loss limits
  * Holding period limits
  * Trader-specific limits

### Limit Types

* **Hard Limits:**
  * Regulatory requirements
  * Board-approved limits
  * System-enforced constraints
  * Critical risk thresholds
  * Legal or contractual limits

* **Soft Limits:**
  * Early warning indicators
  * Management action triggers
  * Escalation thresholds
  * Trending indicators
  * Performance boundaries

### Limit Monitoring Process

* **Continuous Monitoring:**
  * Real-time limit checks
  * Automated alerts
  * Visualization dashboards
  * Utilization trending
  * Pre-trade limit validation

* **Limit Breach Management:**
  * Breach identification
  * Classification (technical vs. actual)
  * Notification and escalation
  * Remediation planning
  * Documentation and reporting

* **Limit Review and Adjustment:**
  * Periodic limit reviews
  * Utilization analysis
  * Business need assessment
  * Market condition alignment
  * Risk appetite reconciliation

## 5. Monitoring Infrastructure

### Technical Architecture

![Portfolio Risk Monitoring Architecture](../Architecture/diagrams/portfolio-risk-monitoring-architecture.svg)

* **Data Integration Layer:**
  * Position data feeds
  * Market data integration
  * Reference data services
  * Transaction capture
  * External data sources

* **Calculation Engine:**
  * Risk model execution
  * Scenario processors
  * Aggregation engine
  * Limit verification
  * Historical analysis

* **Monitoring and Alerting:**
  * Real-time dashboards
  * Alert generation
  * Notification services
  * Escalation workflows
  * Mobile alerting

* **Reporting and Analysis:**
  * Standard reports
  * Ad-hoc analysis tools
  * Trend visualization
  * Drill-down capabilities
  * Export functionality

### Performance Requirements

* **Real-time Monitoring:**
  * Risk update latency: <10 seconds
  * Dashboard refresh: <5 seconds
  * Alert generation: <1 second
  * Position update reflection: <30 seconds
  * Limit check: <1 second

* **Batch Processing:**
  * Full portfolio risk calculation: <30 minutes
  * Comprehensive stress testing: <2 hours
  * Historical simulation: <1 hour
  * Monte Carlo simulation: <3 hours
  * Full risk report generation: <15 minutes

### Availability and Resilience

* **System Availability:**
  * Real-time monitoring: 99.99% uptime
  * Trading hours coverage: 100% (with failover)
  * Batch processing: 99.9% completion rate
  * Critical alerts: Redundant delivery paths
  * Multiple access channels

* **Resilience Features:**
  * Geographically distributed processing
  * Active-active monitoring configuration
  * Degraded mode capabilities
  * Offline calculation options
  * Manual override procedures

## 6. Monitoring Processes

### Daily Risk Monitoring Cycle

1. **Pre-Market Preparation:**
   * Overnight batch validation
   * Market data quality checks
   * Scenario update review
   * Limit structure verification
   * Alert configuration check

2. **Active Trading Period:**
   * Real-time risk updates
   * Limit utilization monitoring
   * Intraday risk recalculation
   * Alert response and escalation
   * Ad-hoc analysis support

3. **End-of-Day Processing:**
   * Final position capture
   * Comprehensive risk calculation
   * Limit compliance verification
   * Daily risk report generation
   * Next-day preparation

4. **Overnight Analysis:**
   * Extended scenario analysis
   * Trend evaluation
   * Anomaly investigation
   * Stress test execution
   * Regulatory calculation

### Escalation Procedures

* **Level 1 (Awareness):**
  * Soft limit approach/breach
  * Unusual market movements
  * Position growth acceleration
  * New risk concentrations
  * Model performance changes

* **Level 2 (Action Required):**
  * Hard limit approach (80%+)
  * Significant market moves
  * Rapid risk metric changes
  * Liquidity deterioration
  * Credit quality changes

* **Level 3 (Urgent Intervention):**
  * Hard limit breaches
  * Extreme market conditions
  * Major credit events
  * Significant losses
  * System stability issues

### Special Situation Monitoring

* **Volatile Market Protocol:**
  * Increased monitoring frequency
  * Enhanced scenario analysis
  * Intraday limit reassessment
  * Heightened escalation thresholds
  * Extended coverage hours

* **New Product Monitoring:**
  * Enhanced scrutiny period
  * Supplemental risk metrics
  * Conservative initial limits
  * Model performance validation
  * Increased reporting frequency

* **Counterparty Stress Monitoring:**
  * Enhanced exposure tracking
  * Collateral adequacy review
  * Settlement risk monitoring
  * Default scenario planning
  * Communication protocol activation

## 7. Reporting Framework

### Standard Reports

* **Daily Risk Summary:**
  * Key risk metrics overview
  * Limit utilization status
  * Notable position changes
  * Market movement impacts
  * Actionable highlights

* **Weekly Risk Review:**
  * Trend analysis
  * Limit utilization patterns
  * Stress test results
  * Concentration evolution
  * Risk appetite alignment

* **Monthly Risk Package:**
  * Comprehensive risk profile
  * Performance attribution
  * Risk-adjusted returns
  * Scenario analysis results
  * Strategic risk assessment

### Dashboard Design

* **Executive Dashboard:**
  * Enterprise risk overview
  * Key risk indicators
  * Limit status summary
  * Alert highlights
  * Trend indicators

* **Portfolio Manager Dashboard:**
  * Portfolio-specific risk metrics
  * Position-level risk details
  * Scenario impact analysis
  * Historical performance context
  * Limit utilization details

* **Risk Analyst Dashboard:**
  * Detailed risk decomposition
  * Model performance metrics
  * Data quality indicators
  * Correlation analysis
  * Advanced analytical tools

* **Operations Dashboard:**
  * System status indicators
  * Processing completion status
  * Data flow monitoring
  * Exception tracking
  * Service level metrics

### Alert Design

* **Alert Components:**
  * Clear risk identifier
  * Specific metric and threshold
  * Current value and context
  * Trend information
  * Required action guidance

* **Alert Channels:**
  * Dashboard notifications
  * Email alerts (tiered urgency)
  * Mobile push notifications
  * SMS for critical alerts
  * Voice calls for emergencies

* **Alert Categorization:**
  * Risk type classification
  * Severity rating
  * Required response time
  * Responsible party
  * Resolution tracking

## 8. Implementation Guidelines

### Implementation Priorities

1. **Foundation:**
   * Core market risk monitoring
   * Basic limit framework
   * Essential dashboards
   * Minimum viable reporting
   * Critical alert capability

2. **Enhancement:**
   * Comprehensive risk metrics
   * Advanced limit structure
   * Full dashboard suite
   * Complete reporting framework
   * Multi-channel alerting

3. **Optimization:**
   * Real-time risk analytics
   * Predictive risk indicators
   * Machine learning enhancements
   * Advanced visualization
   * Mobile monitoring capabilities

### Best Practices

* **Data Quality Management:**
  * Implement robust data validation
  * Monitor data timeliness and completeness
  * Establish clear data ownership
  * Develop reconciliation processes
  * Create data quality metrics

* **Balanced Coverage:**
  * Focus on material risks
  * Right-size monitoring frequency
  * Align metric sophistication with needs
  * Balance complexity with clarity
  * Consider audience requirements

* **Actionable Information:**
  * Provide context with metrics
  * Include trend information
  * Highlight exceptions
  * Suggest potential actions
  * Enable drill-down to details

## 9. References

* [Risk Management Overview](./README.md)
* [Risk Architecture](./risk-architecture.md)
* [Model Validation Framework](./model-validation-framework.md)
* [Stress Testing Guidelines](./stress-testing-guidelines.md)
* [Limit Management Policy](./limit-management-policy.md)
* [System Architecture](../Architecture/system-architecture.md)

---

## 10. Document Control

* **Owner:** Head of Portfolio Risk
* **Last Updated:** 2025-05-29
* **Status:** Draft

* **Change Log:**

  | Version | Date | Author | Changes | Reviewers |
  |---------|------|--------|---------|-----------|
  | 1.0.0 | 2025-05-29 | Head of Portfolio Risk | Initial document creation | CRO, Portfolio Management Team |

* **Review Schedule:** Quarterly or with significant portfolio strategy changes