# Asset Domain Integration

> AI/ML Domain Integration with Asset Domain

---

## Overview

This document describes how the AI/ML domain integrates with the Asset domain through well-defined interfaces. For the canonical interface definitions, see the [Cross-Domain Interface Definitions](../../Crosscutting/Contracts/domain-interfaces.md).

## Integration Summary

The AI/ML domain interacts with the Asset domain through explicit interfaces:

### Provided Interfaces (AI/ML → Asset)

* **ITradingSignalProvider**: Generates trading signals for Asset domain
  * Entry and exit signals
  * Risk warnings and anomalies
  * Market regime change indicators
  * Volatility forecasts

* **IPortfolioOptimizationService**: Provides portfolio optimization services
  * Optimal portfolio weights
  * Efficient frontier calculations
  * Risk factor exposures
  * Rebalancing recommendations

### Consumed Interfaces (Asset → AI/ML)

* **IMarketDataProvider**: Consumes market data for model training and inference
  * Historical price and volume data
  * Order book snapshots and updates
  * Trading activity metrics
  * Asset correlation data

* **IModelParameterProvider**: Receives parameters for financial models
  * Asset characteristics and constraints
  * Market equilibrium assumptions
  * Portfolio constraints and objectives
  * Risk tolerance parameters

## AI/ML Domain Implementation Details

### Trading Signal Provider

The AI/ML domain implements the `ITradingSignalProvider` interface through:

* **SignalGenerationService**: Creates entry and exit signals
* **AnomalyDetectionService**: Identifies and reports risk warnings
* **RegimeClassificationModel**: Determines market regime states
* **VolatilityPredictionModel**: Forecasts future volatility

### Portfolio Optimization Provider

The AI/ML domain implements the `IPortfolioOptimizationService` interface through:

* **OptimalWeightCalculator**: Determines optimal portfolio weights
* **EfficientFrontierGenerator**: Calculates efficient frontier points
* **RiskFactorAnalyzer**: Analyzes portfolio risk factor exposures
* **RebalancingEngine**: Generates rebalancing recommendations

### Market Data Consumer

The AI/ML domain consumes the `IMarketDataProvider` interface through:

* **DataPreprocessingService**: Prepares market data for model training
* **FeatureEngineeringService**: Extracts features from market data
* **ModelTrainingService**: Uses historical data for model training
* **RealTimeInferenceService**: Processes live market data for predictions

### Model Parameter Consumer

The AI/ML domain consumes the `IModelParameterProvider` interface through:

* **ParameterValidationService**: Validates model parameters
* **ModelConfigurationService**: Configures models with parameters
* **ConstraintEnforcementService**: Enforces portfolio constraints
* **BlackLittermanService**: Implements Black-Litterman model

## Event Integration

The AI/ML domain publishes and subscribes to events for asynchronous integration:

### Published Events

* **TradingSignalGenerated**: Signals when new trading signals are available
* **RiskWarningIssued**: Alerts about detected risk anomalies
* **OptimizationCompleted**: Notifies when portfolio optimization is complete

### Subscribed Events

* **MarketDataUpdated**: Triggers model updates with new market data
* **PortfolioConstraintsChanged**: Updates optimization constraints
* **AssetCharacteristicsUpdated**: Refreshes model parameters

## References

* [Cross-Domain Interface Definitions](../../Crosscutting/Contracts/domain-interfaces.md)
* [AI/ML Domain Documentation](./README.md)
* [Asset Domain Documentation](../Asset/README.md)
* [Event Schema Standards](../../Crosscutting/Events/README.md)
