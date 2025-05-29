# ML-Enhanced Shrinkage: Feature Engineering (Part 2)

> Temporal and regime-based feature extraction for ML-Enhanced Shrinkage models

---

## 1. Temporal Features

### Theoretical Motivation

Temporal features capture the time-varying nature of financial markets. They provide information about:

- The stability of correlations over time
- Recent market volatility patterns
- Trends in covariance structure
- Seasonality effects

These features enable the model to adapt to changing market conditions and recognize when historical patterns may be less relevant.

### Implementation

The following temporal features are extracted from return time series:

```python
def extract_temporal_features(returns: np.ndarray, 
                              lookback_windows: List[int] = [10, 21, 63, 126, 252]
                             ) -> Dict[str, float]:
    """
    Extract features based on temporal properties of returns.
    
    Parameters:
    -----------
    returns : numpy.ndarray
        Asset returns array of shape (time_periods, n_assets)
    lookback_windows : list of int
        List of lookback window sizes in trading days
        
    Returns:
    --------
    dict
        Dictionary of temporal features
    """
    n_periods, n_assets = returns.shape
    features = {}
    
    # Ensure we have enough data for the longest lookback
    max_lookback = max(lookback_windows)
    if n_periods < max_lookback:
        raise ValueError(f"Not enough return data for temporal features. "
                        f"Need at least {max_lookback} periods.")
    
    # Calculate volatility changes across different time windows
    for window in lookback_windows:
        if window >= n_periods:
            continue
            
        # Recent vs earlier volatility
        recent_vol = np.std(returns[-window:], axis=0)
        if window*2 <= n_periods:
            earlier_vol = np.std(returns[-(window*2):-window], axis=0)
            vol_ratio = np.mean(recent_vol / earlier_vol)
            features[f"vol_change_{window}d"] = vol_ratio
        
        # Correlation stability
        recent_corr = np.corrcoef(returns[-window:].T)
        if window*2 <= n_periods:
            earlier_corr = np.corrcoef(returns[-(window*2):-window].T)
            corr_diff = np.mean(np.abs(recent_corr - earlier_corr))
            features[f"corr_stability_{window}d"] = corr_diff
    
    # Temporal correlation decay
    features["correlation_decay_rate"] = estimate_correlation_decay(returns)
    
    # Detect serial correlation in volatility
    features["garch_effect"] = estimate_garch_effect(returns)
    
    # Trend features
    features["return_autocorrelation"] = calculate_return_autocorrelation(returns)
    features["vol_of_vol"] = calculate_volatility_of_volatility(returns)
    
    return features

def estimate_correlation_decay(returns: np.ndarray, max_lag: int = 20) -> float:
    """
    Estimate the rate at which correlations decay over time.
    
    Parameters:
    -----------
    returns : numpy.ndarray
        Asset returns array
    max_lag : int
        Maximum lag to consider
        
    Returns:
    --------
    float
        Estimated correlation decay rate
    """
    # Implementation details...
    # This would fit an exponential decay model to correlation as a function of lag
    
    return decay_rate  # Placeholder

def calculate_return_autocorrelation(returns: np.ndarray, lag: int = 1) -> float:
    """
    Calculate average autocorrelation of asset returns.
    
    Parameters:
    -----------
    returns : numpy.ndarray
        Asset returns array
    lag : int
        Lag for autocorrelation
        
    Returns:
    --------
    float
        Average autocorrelation across assets
    """
    n_assets = returns.shape[1]
    autocorrs = []
    
    for i in range(n_assets):
        series = returns[:, i]
        # Remove NaN values
        valid_indices = ~np.isnan(series)
        if np.sum(valid_indices) <= lag:
            continue
            
        series = series[valid_indices]
        if len(series) > lag:
            autocorr = np.corrcoef(series[:-lag], series[lag:])[0, 1]
            if not np.isnan(autocorr):
                autocorrs.append(autocorr)
    
    if autocorrs:
        return np.mean(autocorrs)
    return 0.0
```

### Key Temporal Features

The most important temporal features include:

1. **Volatility Regime Change**: Ratio of recent to historical volatility
2. **Correlation Stability**: Measure of correlation structure changes
3. **Serial Correlation**: Evidence of momentum or mean reversion
4. **Volatility Clustering**: Indication of GARCH-like effects
5. **Correlation Decay Rate**: How quickly correlations diminish with time

## 2. Market Regime Features

### Theoretical Motivation

Market regime features identify distinct states of the financial markets that may require different covariance estimation approaches:

- Stress regimes often exhibit correlation breakdowns
- Trending markets may have different factor structures
- Volatility regimes impact the reliability of historical estimates

These features help the model adjust shrinkage intensity based on prevailing market conditions.

### Implementation

The following regime detection features are implemented:

```python
def extract_regime_features(returns: np.ndarray, 
                           market_data: Optional[Dict] = None
                          ) -> Dict[str, float]:
    """
    Extract features related to market regimes.
    
    Parameters:
    -----------
    returns : numpy.ndarray
        Asset returns array of shape (time_periods, n_assets)
    market_data : dict, optional
        Additional market data like VIX, yield curves, etc.
        
    Returns:
    --------
    dict
        Dictionary of regime features
    """
    features = {}
    
    # Volatility regime
    returns_volatility = np.std(returns, axis=0)
    features["volatility_regime"] = detect_volatility_regime(returns)
    
    # Correlation regime
    features["correlation_regime"] = detect_correlation_regime(returns)
    
    # Tail event indicator
    features["tail_event_probability"] = estimate_tail_probability(returns)
    
    # Market stress indicators
    if market_data and "vix" in market_data:
        features["vix_percentile"] = calculate_percentile(
            market_data["vix"][-1], market_data["vix"][-252:])
        features["vix_change"] = market_data["vix"][-1] / market_data["vix"][-22:].mean() - 1
    
    # Trend regime
    features["trend_strength"] = calculate_trend_strength(returns)
    
    # Market rotation
    features["market_rotation"] = detect_market_rotation(returns)
    
    return features

def detect_volatility_regime(returns: np.ndarray) -> float:
    """
    Detect current volatility regime (normalized scale from 0 to 1).
    
    Parameters:
    -----------
    returns : numpy.ndarray
        Asset returns array
        
    Returns:
    --------
    float
        Volatility regime indicator (0=low, 1=high)
    """
    # Use a rolling window to compute recent volatility
    if len(returns) < 252:
        lookback = len(returns) // 2
    else:
        lookback = 126
    
    recent_vol = np.std(returns[-lookback:])
    
    if len(returns) >= 756:  # Approximately 3 years
        historical_vol = np.std(returns[-756:-lookback])
        # Normalize the ratio between 0 and 1 using a sigmoid function
        vol_ratio = recent_vol / historical_vol
        regime_indicator = 1 / (1 + np.exp(-5 * (vol_ratio - 1)))
    else:
        # Less history available, use a simple percentile approach
        vol_history = []
        for i in range(1, len(returns) // lookback):
            start_idx = max(0, len(returns) - i * lookback)
            end_idx = len(returns) - (i - 1) * lookback
            vol_history.append(np.std(returns[start_idx:end_idx]))
        
        if vol_history:
            regime_indicator = calculate_percentile(recent_vol, vol_history)
        else:
            regime_indicator = 0.5  # Default when insufficient data
    
    return regime_indicator

def calculate_percentile(value: float, history: List[float]) -> float:
    """
    Calculate the percentile of a value within a historical distribution.
    
    Parameters:
    -----------
    value : float
        Current value
    history : list of float
        Historical values
        
    Returns:
    --------
    float
        Percentile (0 to 1)
    """
    return sum(1 for h in history if h < value) / len(history)
```

### Key Regime Features

The most significant market regime features include:

1. **Volatility Regime Indicator**: Normalized measure of current volatility level
2. **Correlation Regime Indicator**: Measure of correlation strength
3. **Tail Event Probability**: Likelihood of being in a market stress period
4. **Market Rotation Indicator**: Measure of sector/factor rotation
5. **VIX-based Features**: Market-implied volatility indicators (when available)

## 3. Combined Feature Pipeline

### Integration with Core Pipeline

The temporal and regime features are integrated with the statistical features from Part 1:

```python
def extract_all_features(
    returns: np.ndarray,
    sample_cov: Optional[np.ndarray] = None,
    market_data: Optional[Dict] = None
) -> Dict[str, float]:
    """
    Extract all features for ML-Enhanced Shrinkage estimation.
    
    Parameters:
    -----------
    returns : numpy.ndarray
        Asset returns array of shape (time_periods, n_assets)
    sample_cov : numpy.ndarray, optional
        Pre-computed sample covariance matrix
    market_data : dict, optional
        Additional market data
        
    Returns:
    --------
    dict
        Combined feature dictionary
    """
    # Calculate sample covariance if not provided
    if sample_cov is None:
        sample_cov = np.cov(returns.T)
    
    # Extract features from each category
    eigenvalue_features = extract_eigenvalue_features(sample_cov)
    correlation_features = extract_correlation_features(sample_cov)
    temporal_features = extract_temporal_features(returns)
    regime_features = extract_regime_features(returns, market_data)
    
    # Combine all features
    all_features = {}
    all_features.update(eigenvalue_features)
    all_features.update(correlation_features)
    all_features.update(temporal_features)
    all_features.update(regime_features)
    
    return all_features
```

### Feature Importance

The relative importance of different feature categories varies by market condition:

| Feature Category    | Normal Markets | Stress Periods | Regime Transitions |
|---------------------|----------------|----------------|-------------------|
| Eigenvalue Features | High           | Medium         | Medium            |
| Correlation Features| Medium         | High           | Medium            |
| Temporal Features   | Medium         | Medium         | High              |
| Regime Features     | Low            | High           | High              |

Feature importance is periodically re-evaluated during model retraining to ensure optimal feature selection.

## 4. Feature Engineering Best Practices

### Data Quality Checks

Before feature extraction, several data quality checks are performed:

```python
def validate_inputs(returns: np.ndarray) -> bool:
    """
    Validate input data before feature extraction.
    
    Parameters:
    -----------
    returns : numpy.ndarray
        Asset returns array
        
    Returns:
    --------
    bool
        True if inputs are valid
    """
    # Check for sufficient data
    if returns.shape[0] < 21:  # Minimum required periods
        return False
    
    # Check for excessive missing data
    missing_ratio = np.isnan(returns).sum() / returns.size
    if missing_ratio > 0.1:  # More than 10% missing
        return False
    
    # Check for data quality issues
    if np.max(np.abs(returns)) > 1.0:  # Suspiciously large returns
        # Verify these are not price returns instead of log returns
        return False
    
    return True
```

### Handling Missing Data

Various strategies are employed to handle missing data in feature extraction:

1. **Pairwise calculation** for correlation features
2. **Imputation** for temporal features
3. **Feature exclusion** when quality thresholds aren't met

---

**Related Documents:**
* [ML-Enhanced Shrinkage: Feature Engineering (Part 1)](./ml-shrinkage-features-part1.md)
* [ML-Enhanced Shrinkage: Model Training](./ml-shrinkage-training.md)
* [ML-Enhanced Shrinkage: Architecture](./ml-shrinkage-architecture.md)