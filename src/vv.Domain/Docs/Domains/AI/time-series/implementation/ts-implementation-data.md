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

# Time Series Data Preparation

> Best practices for preparing financial time series data for modeling

---

## Data Collection

### Data Sources

* **Market Data**:
  * Primary sources: Bloomberg, Refinitiv, FactSet
  * Alternative sources: Yahoo Finance, Alpha Vantage
  * Internal sources: Order management systems, trading platforms

* **Fundamental Data**:
  * Company financials: S&P Capital IQ, Compustat
  * Economic indicators: FRED, World Bank, IMF
  * Alternative data: Satellite imagery, sentiment scores, ESG metrics

* **Data Quality Assessment**:
  * Consistency checks across sources
  * Data completeness audits
  * Outlier identification
  * Corporate action adjustment verification

### Data Frequency Considerations

| Frequency | Use Cases | Considerations |
|-----------|-----------|----------------|
| Tick data | Market microstructure, HFT | Volume, storage, noise filtering |
| Intraday (1min-1hr) | Day trading, intraday risk | Seasonality patterns, data gaps |
| Daily | Most financial applications | Balance of signal and noise |
| Weekly/Monthly | Macro trends, long-term forecasting | Small sample challenges |

## Data Preprocessing

### Missing Value Handling

* **Detection Methods**:
  * Calendar-based gap identification
  * Trading holiday mapping
  * Exchange-specific session times
  * Structured vs. unstructured absence

* **Imputation Techniques**:
  * Forward/backward filling for short gaps
  * Linear interpolation for small gaps
  * Model-based imputation (KNN, MICE, GAN)
  * Trading day adjustments

* **When to Exclude**:
  * Extended missing periods
  * Critical market events with data gaps
  * Non-recoverable corruption
  * When imputation introduces bias

### Outlier Treatment

* **Identification Approaches**:
  * Z-score methods (3-sigma rule)
  * Modified Z-score for non-normal data
  * Isolation Forest for multivariate detection
  * Domain-specific rules (circuit breakers, etc.)

* **Treatment Strategies**:
  * Winsorization vs. trimming
  * Replacement with median/mean
  * Local regression replacement
  * Contextual evaluation (true events vs. errors)

* **Financial Domain Considerations**:
  * Market crash events (valid extreme values)
  * Corporate actions (splits, dividends)
  * Flash crashes and market anomalies
  * Regulatory changes impact

## Feature Engineering

### Temporal Features

* **Calendar Features**:
  * Day of week, month, quarter
  * Holiday proximity indicators
  * Fiscal period boundaries
  * Exchange-specific trading calendars

* **Lagged Features**:
  * Auto-regressive lags selection
  * Cross-asset lags
  * Optimal lag determination
  * Lag feature normalization

* **Rolling Window Features**:
  * Moving averages (simple, exponential, weighted)
  * Rolling volatility measures
  * Moving correlation/covariance
  * Min/max/range over windows

### Domain-Specific Financial Features

* **Technical Indicators**:
  * Momentum indicators (RSI, MACD)
  * Volatility indicators (Bollinger Bands, ATR)
  * Volume indicators (OBV, Money Flow)
  * Pattern recognition features

* **Market Microstructure Features**:
  * Bid-ask spread metrics
  * Order book imbalance
  * Trade signing (Lee-Ready)
  * Market impact measures

* **Cross-Asset Features**:
  * Correlation-based features
  * Lead-lag relationships
  * Basis spreads
  * Risk factor exposures

## Data Normalization

### Scaling Techniques

* **Standard Scaling**:
  * Z-score normalization
  * Expanding window vs. rolling window
  * Robust scaling with median/IQR
  * Asset-specific vs. pooled scaling

* **Min-Max Scaling**:
  * [0,1] vs. [-1,1] normalization
  * Adaptive range adjustment
  * Quantile-based boundaries
  * Dealing with new extremes

* **Financial-Specific Approaches**:
  * Returns vs. price scaling
  * Log-returns transformation
  * Volatility normalization
  * Cross-sectional standardization

### Stationarity Transformations

* **Testing for Stationarity**:
  * Augmented Dickey-Fuller test
  * KPSS test
  * Variance ratio test
  * Visual inspection methods

* **Transformation Methods**:
  * Differencing (first and seasonal)
  * Log transformations
  * Box-Cox transformations
  * Fractional differencing

* **When to Preserve Non-Stationarity**:
  * Cointegration modeling
  * Some deep learning approaches
  * Trend prediction tasks
  * Relative value models

## Data Splitting

### Time-Based Splitting

* **Training-Validation-Test Splits**:
  * Chronological partitioning
  * Non-overlapping periods
  * Regime-aware splitting
  * Multi-period validation

* **Cross-Validation Approaches**:
  * Time series cross-validation
  * Expanding window validation
  * Blocked cross-validation
  * Purged cross-validation

* **Look-Ahead Bias Prevention**:
  * Feature calculation timeline
  * Forward-looking information audit
  * Point-in-time data reconstruction
  * Backtest boundary enforcement

### Train-Test Contamination Prevention

* **Data Leakage Sources**:
  * Scaling with full dataset knowledge
  * Future-informed feature selection
  * Cross-validation with time dependency
  * Survival bias in security selection

* **Safeguard Measures**:
  * Time-step validation
  * Information barrier testing
  * Pipeline encapsulation
  * Timestamp-based access control

## Implementation Examples

### Example: Daily Stock Return Preprocessing

```python
import pandas as pd
import numpy as np
from sklearn.preprocessing import RobustScaler

def preprocess_stock_returns(price_df, window_size=252):
    """
    Preprocess daily stock prices into model-ready features
    
    Parameters:
    -----------
    price_df : pandas.DataFrame
        DataFrame with date index and price columns for stocks
    window_size : int
        Lookback window for feature calculation (default: 252 trading days)
    
    Returns:
    --------
    features_df : pandas.DataFrame
        Model-ready features
    """
    # Calculate returns
    returns_df = price_df.pct_change().dropna()
    
    # Handle outliers (winsorization at 3 std)
    returns_clipped = returns_df.clip(
        lower=returns_df.mean() - 3*returns_df.std(),
        upper=returns_df.mean() + 3*returns_df.std(),
    )
    
    # Create lagged features
    features = []
    
    # Add lag features (t-1, t-2, t-3, t-5, t-10)
    for lag in [1, 2, 3, 5, 10]:
        features.append(returns_clipped.shift(lag).add_suffix(f'_lag_{lag}'))
    
    # Add rolling window features
    features.append(
        returns_clipped.rolling(5).mean().add_suffix('_ma_5')
    )
    features.append(
        returns_clipped.rolling(20).mean().add_suffix('_ma_20')
    )
    features.append(
        returns_clipped.rolling(20).std().add_suffix('_vol_20')
    )
    
    # Combine features
    features_df = pd.concat(features, axis=1).dropna()
    
    # Apply robust scaling
    scaler = RobustScaler()
    scaled_features = pd.DataFrame(
        scaler.fit_transform(features_df),
        index=features_df.index,
        columns=features_df.columns
    )
    
    return scaled_features
```

## Related Documentation

* [Time Series Fundamentals](../ts-fundamentals.md)
* [Implementation Guide Overview](../ts-implementation.md)
* [Model Selection Guide](./ts-implementation-model-selection.md)
* [Training Pipeline Guide](./ts-implementation-training.md)

---

*Last Updated: 2025-05-29*