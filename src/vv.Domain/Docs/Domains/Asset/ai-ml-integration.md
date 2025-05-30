# AI/ML Integration

> Asset Domain Integration with AI/ML Domain

---

## Overview

This document describes how the Asset domain integrates with the AI/ML domain through well-defined interfaces. For the canonical interface definitions, see the [Cross-Domain Interface Definitions](../../Crosscutting/Contracts/domain-interfaces.md).

## Integration Summary

The Asset domain interacts with the AI/ML domain through explicit interfaces:

### Provided Interfaces (Asset → AI/ML)

* **IMarketDataProvider**: Provides market data for AI/ML model training and inference
  * Historical price and volume data
  * Order book snapshots and updates
  * Trading activity metrics
  * Asset correlation data

* **IModelParameterProvider**: Supplies parameters for financial models
  * Asset characteristics and constraints
  * Market equilibrium assumptions
  * Portfolio constraints and objectives
  * Risk tolerance parameters

### Consumed Interfaces (AI/ML → Asset)

* **ITradingSignalConsumer**: Consumes trading signals from AI/ML models
  * Entry and exit signals
  * Risk warnings and anomalies
  * Market regime change indicators
  * Volatility forecasts

* **IPortfolioOptimizationService**: Utilizes portfolio optimization services
  * Optimal portfolio weights
  * Efficient frontier calculations
  * Risk factor exposures
  * Rebalancing recommendations

## Asset Domain Implementation Details

### Market Data Provider

The Asset domain implements the `IMarketDataProvider` interface through:

* **OrderBookService**: Provides order book snapshots and updates
* **PriceHistoryService**: Supplies historical price and volume data
* **CorrelationService**: Calculates asset correlations
* **TradingMetricsService**: Tracks and reports trading activity

### Model Parameter Provider

The Asset domain implements the `IModelParameterProvider` interface through:

* **AssetCharacteristicsService**: Manages asset-specific characteristics
* **PortfolioConstraintService**: Defines and enforces portfolio constraints
* **RiskParameterService**: Configures risk tolerance parameters
* **EquilibriumCalculator**: Determines market equilibrium assumptions

### Trading Signal Consumer

The Asset domain consumes the `ITradingSignalProvider` interface through:

* **SignalProcessingService**: Processes and validates trading signals
* **RiskWarningHandler**: Responds to risk warnings and anomalies
* **MarketRegimeAdapter**: Adapts strategies based on market regime
* **VolatilityAdjuster**: Adjusts positions based on volatility forecasts

### Portfolio Optimization Consumer

The Asset domain consumes the `IPortfolioOptimizationService` interface through:

* **PortfolioOptimizer**: Requests and applies optimal portfolio weights
* **EfficientFrontierAnalyzer**: Analyzes efficient frontier calculations
* **RiskFactorManager**: Monitors and manages risk factor exposures
* **RebalancingService**: Implements rebalancing recommendations

## Event Integration

The Asset domain publishes and subscribes to events for asynchronous integration:

### Published Events

* **MarketDataUpdated**: Notifies when significant market data changes occur
* **PortfolioConstraintsChanged**: Signals changes to portfolio constraints
* **AssetCharacteristicsUpdated**: Indicates updates to asset characteristics

### Subscribed Events

* **TradingSignalGenerated**: Processes new trading signals
* **RiskWarningIssued**: Handles risk warnings
* **OptimizationCompleted**: Applies optimization results

## References

* [Cross-Domain Interface Definitions](../../Crosscutting/Contracts/domain-interfaces.md)
* [Asset Domain Documentation](./README.md)
* [AI/ML Domain Documentation](../AI/README.md)
* [Event Schema Standards](../../Crosscutting/Events/README.md)
