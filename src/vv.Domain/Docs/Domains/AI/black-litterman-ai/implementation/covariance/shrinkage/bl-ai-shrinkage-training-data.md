# ML-Enhanced Shrinkage: Training Data

> Data sources and dataset creation for training ML-enhanced shrinkage models

---

## 1. Data Requirements

### Data Types

The ML-enhanced shrinkage model requires several types of data for effective training:

1. **Asset Return Data**:
   - Daily returns for all assets in the investment universe
   - Minimum history of 10 years (preferably covering multiple market regimes)
   - Adjusted for corporate actions and dividends
   - Minimum of 500 assets for diversity

2. **Market Factor Data**:
   - Major market indices (S&P 500, NASDAQ, Russell 2000, etc.)
   - Style factors (value, momentum, quality, size, etc.)
   - Sector indices and ETFs
   - Volatility indices (VIX, VSTOXX, etc.)

3. **Macroeconomic Data**:
   - Interest rates and yield curves
   - GDP and economic growth indicators
   - Inflation metrics
   - Central bank policy rates

4. **Market Microstructure Data**:
   - Trading volumes
   - Bid-ask spreads
   - Market depth metrics
   - Order flow imbalance indicators

### Data Quality Requirements

For successful training, the data must satisfy these quality criteria:

* **Completeness**: Minimal missing values (<1% per asset)
* **Cleanliness**: Outliers and data errors addressed
* **Consistency**: Aligned timestamps across all data sources
* **Representativeness**: Covers multiple market regimes and conditions

## 2. Data Sources

### Recommended Data Providers

| Data Type | Recommended Providers | Alternative Sources |
|-----------|----------------------|---------------------|
| Asset Returns | Bloomberg, Refinitiv | Yahoo Finance, AlphaVantage |
| Factor Data | MSCI Barra, AQR | French-Fama Data Library |
| Macroeconomic | Federal Reserve (FRED) | World Bank, IMF |
| Market Microstructure | TAQ Database, LOBSTER | NASDAQ TotalView-ITCH |

### Internal Data Sources

For organizations with existing data infrastructure:

* **Market Data Warehouse**: Centralized repository of clean market data
* **Alternative Data Platform**: Repository for non-traditional data sources
* **Factor Library**: Pre-calculated factor exposures and returns
* **Backtest Database**: Historical model predictions and actual outcomes

## 3. Data Processing Pipeline

### Initial Data Acquisition

```
┌───────────────┐     ┌───────────────┐     ┌───────────────┐
│ Data Source   │     │ Data Source   │     │ Data Source   │
│ Connectors    │────▶│ API Clients   │────▶│ Raw Data      │
│               │     │               │     │ Storage       │
└───────────────┘     └───────────────┘     └───────────────┘
```

1. **Connector Implementation**:
   - REST API clients for web-based sources
   - Database connectors for internal data
   - FTP/SFTP clients for file-based data providers

2. **Data Fetching Schedule**:
   - Daily updates for market data
   - Weekly updates for factor data
   - Monthly updates for macroeconomic data

3. **Raw Data Storage**:
   - Parquet files for tabular data
   - HDF5 for multi-dimensional data
   - Time-series databases for continuous updates

### Data Cleaning and Preprocessing

```
┌───────────────┐     ┌───────────────┐     ┌───────────────┐
│ Raw Data      │     │ Validation &  │     │ Preprocessed  │
│ Storage       │────▶│ Cleaning      │────▶│ Data Storage  │
│               │     │               │     │               │
└───────────────┘     └───────────────┘     └───────────────┘
```

1. **Data Validation**:
   - Schema validation
   - Range checks for numeric data
   - Timestamp alignment verification
   - Completeness checks

2. **Cleaning Operations**:
   - Outlier detection and treatment
   - Missing value imputation
   - Deduplication
   - Time zone normalization

3. **Preprocessing Steps**:
   - Return calculation from price data
   - Normalization/standardization
   - Calendar adjustments (trading days)
   - Log transformations where appropriate

## 4. Feature Engineering

### Covariance Matrix Features

These features are derived from the sample covariance matrix:

1. **Eigenvalue Features**:
   - Largest eigenvalue
   - Eigenvalue dispersion (ratio of largest to smallest)
   - Effective rank (sum of eigenvalues / max eigenvalue)
   - Eigenvalue decay rate

2. **Correlation Structure Features**:
   - Average correlation
   - Correlation dispersion
   - Block structure detection metrics
   - Minimum spanning tree properties

3. **Statistical Properties**:
   - Condition number
   - Determinant
   - Trace
   - Frobenius norm

### Market Regime Features

Features that help the model adapt to different market conditions:

1. **Volatility Regime Indicators**:
   - Rolling window volatility
   - GARCH model parameters
   - Relative VIX levels
   - Volatility clustering metrics

2. **Correlation Regime Indicators**:
   - Average correlation trends
   - Correlation breakdown events
   - Sector correlation divergence
   - Safe haven correlation patterns

3. **Liquidity Condition Indicators**:
   - Bid-ask spread metrics
   - Market impact estimates
   - Trading volume trends
   - Order book depth measures

### Temporal Context Features

Features that provide time-related context:

1. **Time Series Properties**:
   - Autocorrelation at various lags
   - Hurst exponent
   - Trend strength indicators
   - Seasonality metrics

2. **Market Calendar Features**:
   - Month-of-year indicators
   - Day-of-week indicators
   - Holiday proximity
   - Earnings season indicators

3. **Event Proximity Features**:
   - Days to/from major economic releases
   - Central bank meeting proximity
   - Political event indicators
   - Fiscal period boundaries

## 5. Dataset Creation

### Training Dataset Structure

The dataset is structured as follows:

```
┌────────────────────────────────────────────┐
│ Dataset Record                             │
├────────────────┬─────────────┬─────────────┤
│ Input Features │ Ground Truth│ Metadata    │
├────────────────┼─────────────┼─────────────┤
│ - Matrix       │ - Optimal   │ - Date      │
│   Features     │   Shrinkage │ - Asset     │
│ - Market       │   Parameters│   Universe  │
│   Features     │ - Target    │ - Market    │
│ - Temporal     │   Weights   │   Regime    │
│   Features     │             │   Label     │
└────────────────┴─────────────┴─────────────┘
```

### Ground Truth Generation

The optimal shrinkage parameters (ground truth) are generated using:

1. **Analytical Methods**:
   - Ledoit-Wolf optimal linear shrinkage formula
   - Oracle shrinkage (when simulation is used)
   - Cross-validated shrinkage intensity

2. **Empirical Performance**:
   - Minimum variance portfolio volatility
   - Maximum Sharpe ratio
   - Minimum tracking error

3. **Bootstrapped Optimization**:
   - Grid search over shrinkage parameters
   - Out-of-sample performance optimization
   - Monte Carlo simulation for robustness

### Data Splitting Strategy

```
┌─────────────────────────────────────────────────────────┐
│ Full Dataset (10+ years)                                │
├───────────────┬───────────────────┬─────────────────────┤
│ Training Set  │ Validation Set    │ Test Set            │
│ (70%)         │ (15%)             │ (15%)               │
├───────────────┴───────────────────┴─────────────────────┤
│ Time-Based Split with Gap Periods                       │
└─────────────────────────────────────────────────────────┘
```

1. **Temporal Splitting**:
   - Training data: Oldest 70% of time periods
   - Validation data: Next 15% of time periods
   - Test data: Most recent 15% of time periods

2. **Gap Periods**:
   - 1-month gaps between splits to prevent leakage
   - Ensures independence of evaluation data

3. **Regime-Aware Splitting**:
   - Ensures all market regimes represented in each split
   - Stratified sampling based on regime labels

## 6. Data Augmentation Techniques

### Synthetic Data Generation

1. **Covariance Matrix Perturbation**:
   - Random noise addition to correlation structure
   - Targeted perturbation of eigenvalues
   - Block structure modifications

2. **Bootstrap Resampling**:
   - Asset return resampling with replacement
   - Block bootstrap for time series
   - Stationary bootstrap with varying block lengths

3. **Simulation-Based Augmentation**:
   - Factor model-based return simulation
   - GARCH process simulation
   - Copula-based multivariate simulation

### Scenario Enrichment

1. **Stress Scenario Injection**:
   - Historical crisis period replication
   - Synthetic stress scenarios
   - Factor shock scenarios

2. **Regime Transition Enrichment**:
   - Oversampling regime transition periods
   - Synthetic regime transition creation
   - Gradual and sudden transition scenarios

## 7. Dataset Versioning and Management

### Versioning Strategy

* **Semantic Versioning**: Major.Minor.Patch format
* **Immutable Datasets**: New version for any changes
* **Versioned Feature Definitions**: Tracked alongside data
* **Data Lineage Tracking**: Source tracking for all features

### Storage and Distribution

* **Dataset Storage Format**: Parquet files with partitioning
* **Metadata Storage**: JSON files with dataset descriptors
* **Access Control**: Role-based permissions for sensitive data
* **Distribution Method**: Data versioning system (DVC or similar)

---

**Related Documents:**
* [ML-Enhanced Shrinkage: Overview & Theory](./bl-ai-shrinkage-overview.md)
* [ML-Enhanced Shrinkage: Model Architecture](./bl-ai-shrinkage-model.md)
* [ML-Enhanced Shrinkage: Training Loss Functions](./bl-ai-shrinkage-training-loss.md)
* [ML-Enhanced Shrinkage: Optimization Strategy](./bl-ai-shrinkage-training-optimization.md)
* [ML-Enhanced Shrinkage: Training Procedures](./bl-ai-shrinkage-training-procedures.md)
* [ML-Enhanced Shrinkage: Model Validation](./bl-ai-shrinkage-training-validation.md)