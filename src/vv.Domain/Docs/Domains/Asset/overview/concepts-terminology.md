```src/vv.Domain/Docs/Domains/Asset/concepts-terminology.md
# Concepts and Terminology

> Core concepts and terminology for the Asset Domain

---

## Overview

This document defines the key concepts and terminology used throughout the Asset Domain documentation. Understanding these fundamental terms is essential for navigating the domain's architecture and implementation.

## Core Asset Concepts

### Asset

An **asset** represents any financial instrument that can be owned and traded:

* **Equity**: Ownership stake in a business entity
* **Fixed Income**: Debt instruments with scheduled payments
* **Cash & Equivalents**: Liquid or near-liquid assets
* **Alternative Assets**: Non-traditional investments (real estate, commodities, etc.)
* **Derivatives**: Instruments whose value derives from underlying assets

### Portfolio

A **portfolio** is a collection of assets held with specific investment objectives:

* **Components**: Assets, weights, and associated metadata
* **Objectives**: Return targets, risk constraints, and other goals
* **Rebalancing Framework**: Rules for maintaining target allocations
* **Performance Metrics**: Measures of portfolio success
* **Risk Characteristics**: Aggregate risk profile and exposures

### Factor

A **factor** represents a systematic source of risk and return:

* **Macroeconomic Factors**: Economic growth, inflation, etc.
* **Style Factors**: Value, growth, momentum, etc.
* **Statistical Factors**: Derived from statistical analysis
* **Sector/Industry Factors**: Industry-specific risk drivers
* **Country/Regional Factors**: Geographic risk drivers

## Investment Strategy Terminology

### Active Management

**Active management** seeks to outperform a benchmark through:

* **Security Selection**: Choosing specific securities expected to outperform
* **Tactical Asset Allocation**: Short-term adjustments to asset weights
* **Factor Timing**: Dynamically adjusting factor exposures
* **Sector Rotation**: Shifting exposure across sectors based on outlook

### Passive Management

**Passive management** aims to match benchmark performance:

* **Index Replication**: Holding securities to track an index
* **Sampling**: Holding representative subset of benchmark securities
* **Optimization**: Minimizing tracking error with constraints
* **Factor Matching**: Replicating factor exposures of benchmark

### Smart Beta

**Smart beta** combines elements of active and passive approaches:

* **Factor Tilts**: Systematic exposure to rewarded factors
* **Alternative Weighting**: Non-market-cap weighting schemes
* **Rules-Based**: Systematic, transparent methodology
* **Risk Premia Harvesting**: Capturing persistent risk premiums

## Risk Management Terminology

### Risk Measures

**Risk measures** quantify investment uncertainty:

* **Volatility**: Standard deviation of returns
* **Value at Risk (VaR)**: Maximum expected loss at a confidence level
* **Conditional VaR (CVaR)**: Expected loss beyond VaR threshold
* **Maximum Drawdown**: Largest peak-to-trough decline
* **Tracking Error**: Standard deviation of excess returns vs. benchmark

### Risk Decomposition

**Risk decomposition** breaks down portfolio risk into components:

* **Factor Risk**: Risk attributed to factor exposures
* **Specific Risk**: Security-specific risk not explained by factors
* **Systematic vs. Idiosyncratic Risk**: Market-related vs. security-specific
* **Marginal Contribution to Risk**: Change in risk from small position changes
* **Percent Contribution to Risk**: Percentage of total risk from each position

## Performance Analysis Terminology

### Performance Measures

**Performance measures** evaluate investment results:

* **Total Return**: Combined price appreciation and income
* **Alpha**: Excess return beyond factor exposures
* **Sharpe Ratio**: Return per unit of total risk
* **Information Ratio**: Active return per unit of active risk
* **Sortino Ratio**: Return per unit of downside risk

### Attribution Analysis

**Attribution analysis** explains sources of performance:

* **Allocation Effect**: Impact of asset allocation decisions
* **Selection Effect**: Impact of security selection decisions
* **Currency Effect**: Impact of currency movements
* **Factor Attribution**: Performance explained by factor exposures
* **Residual Return**: Performance not explained by model factors

## Settlement and Operations Terminology

### Settlement

**Settlement** is the process of finalizing a transaction:

* **Settlement Date**: When assets and funds are exchanged
* **Settlement Cycle**: Standard time between trade and settlement (T+1, T+2)
* **Delivery versus Payment (DvP)**: Assets delivered only when payment received
* **Clearing**: Process of preparing transactions for settlement
* **Reconciliation**: Verification of transaction details between parties

### Corporate Actions

**Corporate actions** are events initiated by corporations affecting securities:

* **Dividends**: Cash or stock distributions to shareholders
* **Stock Splits**: Increase in shares with proportional price reduction
* **Rights Issues**: Offering of additional shares to existing shareholders
* **Mergers & Acquisitions**: Combination of companies
* **Spin-offs**: Creation of new company from existing company's assets

## ESG Terminology

### ESG Components

The three pillars of **ESG**:

* **Environmental**: Climate impact, resource usage, pollution, etc.
* **Social**: Employee relations, human rights, community impact, etc.
* **Governance**: Board structure, executive compensation, ethics, etc.

### ESG Integration Methods

Approaches to incorporating **ESG** in investments:

* **ESG Integration**: Incorporating ESG factors into investment analysis
* **Negative Screening**: Excluding certain sectors or companies
* **Positive Screening**: Selecting companies with strong ESG characteristics
* **Thematic Investing**: Focusing on specific sustainability themes
* **Impact Investing**: Targeting positive measurable impact alongside returns

## Data and Analytics Terminology

### Data Types

Key **data types** in asset management:

* **Market Data**: Prices, volumes, quotes from market exchanges
* **Fundamental Data**: Financial statements, ratios, earnings, etc.
* **Alternative Data**: Non-traditional data sources (satellite, web scraping, etc.)
* **Reference Data**: Security identifiers, classifications, corporate actions
* **Derived Data**: Calculated metrics, analytics, and indicators

### Analytics

Common **analytics** in asset management:

* **Descriptive Analytics**: Historical performance and characteristics
* **Diagnostic Analytics**: Understanding causes of performance
* **Predictive Analytics**: Forward-looking estimates and forecasts
* **Prescriptive Analytics**: Recommended actions based on analysis
* **Optimization**: Finding optimal portfolio allocations

## VeritasVault Specific Terminology

### Platform Components

Key components of the **VeritasVault platform**:

* **Asset Registry**: Central repository for asset information
* **Factor Engine**: System for factor model implementation
* **Portfolio Constructor**: Tools for portfolio creation and management
* **Risk Analyzer**: Risk measurement and decomposition system
* **Performance Calculator**: Return calculation and attribution system
* **Settlement Engine**: Transaction processing and settlement system

### Implementation Concepts

Important **implementation concepts** in VeritasVault:

* **Model Registry**: Repository of quantitative models
* **Data Pipeline**: Workflow for data acquisition and processing
* **Strategy Framework**: Infrastructure for strategy implementation
* **Scenario Engine**: System for scenario analysis
* **Version Control**: Tracking of model and strategy versions
* **Audit Trail**: Record of all system actions and changes

## Related Documentation

* [Core Modules](./core-modules.md) - Detailed description of system modules
* [Implementation Phases](./implementation-phases.md) - Development roadmap
* [Integration Points](./integration-points.md) - Connections to other systems