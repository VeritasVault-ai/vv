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

# Time Series Model Selection

> Framework for selecting appropriate time series models for financial applications

---

## Model Selection Framework

### Key Selection Criteria

* **Problem Characteristics**:
  * Forecast horizon (short vs. long term)
  * Required forecast frequency
  * Univariate vs. multivariate
  * Point forecast vs. distribution

* **Data Characteristics**:
  * Data volume and quality
  * Stationarity properties
  * Seasonality patterns
  * Noise levels and outliers

* **Operational Requirements**:
  * Inference latency needs
  * Interpretability requirements
  * Deployment environment constraints
  * Maintenance considerations

* **Performance Priorities**:
  * Accuracy vs. robustness tradeoff
  * Specific error metric importance
  * Handling of rare events
  * Stability across regimes

## Model Categories and Selection Guidance

### Classical Methods

| Model Type | Recommended When | Not Recommended When |
|------------|------------------|----------------------|
| ARIMA | Data is stationary after differencing | Long-range dependencies exist |
|       | Linear relationships dominate | High dimensionality |
|       | Interpretability is critical | Complex seasonality patterns |
|       | Limited data is available | Strong non-linearities present |
| Exponential Smoothing | Clear trend and/or seasonality | Complex dependencies between variables |
|                      | Need for robust forecasts | Non-decomposable patterns exist |
|                      | Computationally constrained | Many external regressors needed |
|                      | Automated system required | High-frequency data with noise |
| GARCH | Volatility clustering is present | Mean forecasting is primary goal |
|       | Risk modeling is the focus | Long-horizon forecasting needed |
|       | Financial returns are the target | Complex multivariate relationships |
|       | Parametric distribution needed | Limited computational resources |

### Machine Learning Methods

| Model Type | Recommended When | Not Recommended When |
|------------|------------------|----------------------|
| Gradient Boosting | Feature-rich environment | Temporal order is critical |
|                  | Non-linear relationships | Long sequences must be modeled |
|                  | Feature importance needed | Online learning required |
|                  | Robust to outliers required | Transfer learning needed |
| LSTM/GRU | Long-range dependencies | Very limited training data |
|          | Sequential patterns important | Real-time low-latency required |
|          | Variable input length | Full interpretability needed |
|          | Multivariate with interactions | Memory constraints severe |
| TCN/WaveNet | Parallel training desirable | Memory usage is highly constrained |
|             | Multi-scale patterns present | Continual learning required |
|             | Large receptive field needed | Model size must be very small |
|             | Stable gradients required | Variable-length inputs common |
| Transformers | Very long sequences | Training data is limited |
|             | Global pattern recognition | Edge deployment required |
|             | Attention interpretation useful | Ultra-low latency needed |
|             | Transfer learning beneficial | Small-scale simple problem |

## Decision Trees for Common Use Cases

### Return Forecasting Decision Tree

```
Is interpretability critical?
├── Yes: Are relationships likely linear?
│   ├── Yes: Use ARIMA or VAR
│   └── No: Use Gradient Boosting or MARS
└── No: Is forecast horizon long?
    ├── Yes: Are there complex patterns?
    │   ├── Yes: Use Transformer-based models
    │   └── No: Use LSTM or TCN
    └── No: Is computational efficiency critical?
        ├── Yes: Use TCN or N-BEATS
        └── No: Use Ensemble methods
```

### Volatility Forecasting Decision Tree

```
Is parametric distribution required?
├── Yes: Use GARCH family models
└── No: Is forecast horizon short?
    ├── Yes: Are there regime changes?
    │   ├── Yes: Use LSTM-GARCH hybrid
    │   └── No: Use GARCH or Neural GARCH
    └── No: Is forecast granularity important?
        ├── Yes: Use Transformer or LSTM
        └── No: Use Gradient Boosting
```

### Multi-horizon Forecasting Decision Tree

```
Is direct multi-horizon output needed?
├── Yes: Are forecasts for multiple series?
│   ├── Yes: Use Temporal Fusion Transformer
│   └── No: Use Seq2Seq or N-BEATS
└── No: Is recursive forecasting acceptable?
    ├── Yes: Use LSTM, GRU, or TCN
    └── No: Are computational resources limited?
        ├── Yes: Use Multiple direct models
        └── No: Use Temporal Fusion Transformer
```

## Model Selection by Financial Application

### Asset Return Prediction

* **Short-term (Intraday to Days)**:
  * Primary: TCN, Attention-enhanced RNN
  * Alternative: Gradient Boosting, LSTM
  * Classical baseline: ARIMA with exogenous variables

* **Medium-term (Weeks to Months)**:
  * Primary: Temporal Fusion Transformer, LSTM-based ensembles
  * Alternative: Gradient Boosting with time features
  * Classical baseline: Vector autoregression (VAR)

* **Long-term (Months to Years)**:
  * Primary: Decomposition-based models (Neural Prophet, N-BEATS)
  * Alternative: Transformer architectures
  * Classical baseline: State space models

### Volatility and Risk Forecasting

* **Realized Volatility Prediction**:
  * Primary: LSTM-GARCH hybrids, Temporal Fusion Transformer
  * Alternative: Gradient Boosting, TCN
  * Classical baseline: HAR (Heterogeneous Autoregressive) model

* **Option-Implied Volatility Surface**:
  * Primary: Graph Neural Networks, 3D-CNN
  * Alternative: Multi-output Gradient Boosting
  * Classical baseline: Parametric surface models

* **Risk Factor Modeling**:
  * Primary: Multivariate GARCH, Copula-GARCH
  * Alternative: Neural state space models
  * Classical baseline: Dynamic factor models

### Market Microstructure Applications

* **Order Book Dynamics**:
  * Primary: Convolutional LSTM, Attention mechanisms
  * Alternative: WaveNet-based architectures
  * Classical baseline: Point process models

* **Trade Flow Prediction**:
  * Primary: TCN, Causal Convolutions
  * Alternative: GRU with volume features
  * Classical baseline: ARMA-ACD models

* **Market Impact Modeling**:
  * Primary: Neural ODEs, Sequence models
  * Alternative: Gradient Boosting
  * Classical baseline: Power-law decay models

## Implementation Examples

### Example: Model Selection Function

```python
def select_ts_model(problem_config):
    """
    Select appropriate time series model based on problem configuration
    
    Parameters:
    -----------
    problem_config : dict
        Dictionary with problem configuration parameters
        - forecast_horizon: 'short', 'medium', or 'long'
        - data_size: 'small', 'medium', or 'large'
        - interpretability: boolean, is interpretability required
        - multivariate: boolean, is this a multivariate problem
        - computational_constraint: 'low', 'medium', or 'high'
    
    Returns:
    --------
    model_recommendations : dict
        Dictionary with primary, alternative and baseline model recommendations
    """
    p = problem_config
    
    # Initialize empty recommendations
    recommendations = {
        'primary': None,
        'alternative': None,
        'baseline': None
    }
    
    # Short-term forecasting
    if p['forecast_horizon'] == 'short':
        if p['interpretability']:
            if p['data_size'] == 'small':
                recommendations['primary'] = 'ARIMA'
                recommendations['alternative'] = 'Exponential Smoothing'
                recommendations['baseline'] = 'Historical Average'
            else:
                recommendations['primary'] = 'Gradient Boosting'
                recommendations['alternative'] = 'Linear Models with Features'
                recommendations['baseline'] = 'ARIMA'
        else:
            if p['multivariate']:
                if p['computational_constraint'] == 'high':
                    recommendations['primary'] = 'TCN'
                    recommendations['alternative'] = 'GRU'
                    recommendations['baseline'] = 'VAR'
                else:
                    recommendations['primary'] = 'Attention-enhanced RNN'
                    recommendations['alternative'] = 'TCN'
                    recommendations['baseline'] = 'LSTM'
            else:
                recommendations['primary'] = 'LSTM'
                recommendations['alternative'] = 'TCN'
                recommendations['baseline'] = 'ARIMA'
                
    # Medium-term forecasting
    elif p['forecast_horizon'] == 'medium':
        if p['interpretability']:
            recommendations['primary'] = 'Gradient Boosting'
            recommendations['alternative'] = 'N-BEATS'
            recommendations['baseline'] = 'VAR or ARIMA'
        else:
            if p['multivariate']:
                if p['data_size'] == 'large':
                    recommendations['primary'] = 'Temporal Fusion Transformer'
                    recommendations['alternative'] = 'LSTM with Attention'
                    recommendations['baseline'] = 'Gradient Boosting'
                else:
                    recommendations['primary'] = 'LSTM Ensemble'
                    recommendations['alternative'] = 'TCN'
                    recommendations['baseline'] = 'VAR'
            else:
                recommendations['primary'] = 'Neural Prophet'
                recommendations['alternative'] = 'LSTM'
                recommendations['baseline'] = 'ETS or ARIMA'

    # Long-term forecasting
    else:  # p['forecast_horizon'] == 'long'
        if p['interpretability']:
            recommendations['primary'] = 'Neural Prophet'
            recommendations['alternative'] = 'N-BEATS'
            recommendations['baseline'] = 'Structural Time Series Models'
        else:
            if p['multivariate']:
                recommendations['primary'] = 'Temporal Fusion Transformer'
                recommendations['alternative'] = 'Autoformer'
                recommendations['baseline'] = 'Ensemble of Classical Methods'
            else:
                if p['data_size'] == 'large':
                    recommendations['primary'] = 'Informer or Autoformer'
                    recommendations['alternative'] = 'N-BEATS'
                    recommendations['baseline'] = 'Neural Prophet'
                else:
                    recommendations['primary'] = 'N-BEATS'
                    recommendations['alternative'] = 'LSTM with Seq2Seq'
                    recommendations['baseline'] = 'ETS with Regressors'
        
    return recommendations
```

### Example: Comparative Model Assessment

```python
def comparative_model_assessment(data, models_to_test, evaluation_metrics, test_periods=None):
    """
    Perform comparative assessment of multiple time series models
    
    Parameters:
    -----------
    data : pd.DataFrame
        Time series data with datetime index
    models_to_test : list of dict
        List of model configurations to test
        Each dict contains:
        - name: Model name for display
        - model: Model object or function
        - params: Parameters for the model
        - preprocessing: Optional preprocessing steps
    evaluation_metrics : list of str
        Metrics to evaluate (e.g., 'rmse', 'mae', 'mape', 'direction')
    test_periods : list of tuple, optional
        List of (start_date, end_date) tuples for testing periods
        If None, uses single train/test split
        
    Returns:
    --------
    results : pd.DataFrame
        DataFrame with evaluation results
    """
    import pandas as pd
    import numpy as np
    from sklearn.metrics import mean_squared_error, mean_absolute_error
    import time
    
    # If no specific test periods, create single train/test split
    if test_periods is None:
        split_idx = int(len(data) * 0.8)
        train_data = data.iloc[:split_idx]
        test_data = data.iloc[split_idx:]
        test_periods = [(test_data.index[0], test_data.index[-1])]
    
    # Initialize results
    results = []
    
    # For each test period
    for start_date, end_date in test_periods:
        print(f"Evaluating period: {start_date} to {end_date}")
        
        # Split data
        train_data = data.loc[:start_date].iloc[:-1]  # Exclude start_date
        test_data = data.loc[start_date:end_date]
        
        # For each model
        for model_config in models_to_test:
            model_name = model_config['name']
            model = model_config['model']
            params = model_config.get('params', {})
            preprocessing = model_config.get('preprocessing', None)
            
            print(f"  Training model: {model_name}")
            
            # Apply preprocessing if specified
            train_processed = preprocessing(train_data) if preprocessing else train_data
            test_processed = preprocessing(test_data) if preprocessing else test_data
            
            # Record training time
            start_time = time.time()
            
            # Train model (implementation depends on model interface)
            try:
                if hasattr(model, 'fit'):
                    fitted_model = model.fit(train_processed, **params)
                else:
                    # Assume it's a function that returns a fitted model
                    fitted_model = model(train_processed, **params)
                    
                training_time = time.time() - start_time
                
                # Make predictions
                predictions = fitted_model.predict(test_processed)
                
                # Calculate metrics
                period_results = {
                    'model': model_name,
                    'period_start': start_date,
                    'period_end': end_date,
                    'training_time': training_time,
                }
                
                # Add standard metrics
                actuals = test_data[test_data.columns[0]]  # Assuming first column is target
                
                if 'rmse' in evaluation_metrics:
                    period_results['rmse'] = np.sqrt(mean_squared_error(actuals, predictions))
                
                if 'mae' in evaluation_metrics:
                    period_results['mae'] = mean_absolute_error(actuals, predictions)
                
                if 'mape' in evaluation_metrics:
                    period_results['mape'] = np.mean(np.abs((actuals - predictions) / actuals)) * 100
                    
                if 'direction' in evaluation_metrics:
                    # Calculate directional accuracy
                    actual_dirs = np.sign(actuals.diff().dropna())
                    pred_dirs = np.sign(pd.Series(predictions).diff().dropna())
                    dir_acc = np.mean((actual_dirs == pred_dirs).astype(int))
                    period_results['direction'] = dir_acc
                
                # Add financial metrics if specified
                if 'sharpe' in evaluation_metrics:
                    # Simple trading strategy based on predictions
                    returns = actuals.pct_change().dropna()
                    strategy_returns = np.sign(predictions[:-1]) * returns
                    sharpe = strategy_returns.mean() / strategy_returns.std() * np.sqrt(252)  # Annualized
                    period_results['sharpe'] = sharpe
                
                results.append(period_results)
                
            except Exception as e:
                print(f"  Error with model {model_name}: {e}")
                results.append({
                    'model': model_name,
                    'period_start': start_date,
                    'period_end': end_date,
                    'error': str(e)
                })
    
    # Convert to DataFrame
    results_df = pd.DataFrame(results)
    
    # Calculate average metrics across periods
    avg_metrics = results_df.groupby('model').mean()
    
    return results_df, avg_metrics
```

## Related Documentation

* [Time Series Fundamentals](../ts-fundamentals.md)
* [ML Approaches for Time Series](../ts-ml-approaches.md)
* [Implementation Guide Overview](../ts-implementation.md)
* [Training Pipeline Guide](./ts-implementation-training.md)

---

*Last Updated: 2025-05-29*