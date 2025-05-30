# Time Series Performance Evaluation

> Methodologies for evaluating time series model performance in production

---

## Financial Time Series Metrics

### Forecast Accuracy Metrics

* **Standard Error Metrics**:
  * RMSE (Root Mean Squared Error)
  * MAE (Mean Absolute Error)
  * MAPE (Mean Absolute Percentage Error)
  * SMAPE (Symmetric MAPE)

* **Distribution-based Metrics**:
  * Quantile loss
  * CRPS (Continuous Ranked Probability Score)
  * Log-likelihood
  * Interval coverage rate

* **Directional Metrics**:
  * Directional accuracy
  * Correct trend proportion
  * Directional F1-score
  * Sign agreement

### Financial Performance Metrics

* **Return-Based Metrics**:
  * Annualized return
  * Information ratio
  * Excess return
  * Batting average

* **Risk-Adjusted Metrics**:
  * Sharpe ratio
  * Sortino ratio
  * Calmar ratio
  * Omega ratio

* **Drawdown Metrics**:
  * Maximum drawdown
  * Average drawdown
  * Drawdown duration
  * Recovery time

* **Custom Financial Metrics**:
  * Alpha generation
  * Beta neutrality
  * Risk factor exposure
  * Transaction cost-adjusted return

## Comparative Analysis Framework

### Benchmark Comparison

* **Naive Benchmarks**:
  * Last observation carried forward
  * Historical average
  * Seasonal naive model
  * Random walk with drift

* **Statistical Benchmarks**:
  * ARIMA
  * Exponential smoothing
  * VAR (Vector Autoregression)
  * GARCH family models

* **Market Benchmarks**:
  * Index return
  * Peer group average
  * ETF performance
  * Sector benchmark

### Performance Attribution

* **Components**:
  * Feature contribution
  * Temporal decomposition
  * Error source analysis
  * Market condition segmentation

* **Analysis Methods**:
  * SHAP values for time series
  * Partial dependence plots
  * Accumulated local effects
  * Counterfactual analysis

* **Regime Analysis**:
  * Bull vs. bear markets
  * High vs. low volatility
  * Trending vs. mean-reverting
  * Crisis vs. normal periods

## Evaluation Workflow

### Regular Evaluation Cycle

* **Daily Performance Tracking**:
  * Error metrics logging
  * Prediction vs. actual comparison
  * Alert threshold checking
  * Financial impact calculation

* **Weekly Analysis**:
  * Pattern detection in errors
  * Benchmark comparison
  * Feature importance stability
  * Parameter sensitivity

* **Monthly Deep Dive**:
  * Comprehensive performance review
  * Cross-model comparison
  * Retraining evaluation
  * Long-term drift analysis

### Evaluation Dashboard Design

* **Key Components**:
  * Performance metrics summary
  * Time series of key metrics
  * Drift indicator status
  * Error distribution charts

* **Interactive Elements**:
  * Time period selection
  * Benchmark toggling
  * Metric customization
  * Drill-down capability

* **View Customization**:
  * Executive view (high-level metrics)
  * Technical view (detailed statistics)
  * Risk view (uncertainty metrics)
  * Business impact view (financial outcomes)

## Implementation Example

Here's a simple implementation of a performance evaluation framework for financial time series models:

```python
import pandas as pd
import numpy as np
from sklearn.metrics import mean_squared_error, mean_absolute_error
import matplotlib.pyplot as plt

class TimeSeriesEvaluator:
    """Performance evaluation for time series forecasting models"""
    
    def __init__(self, actual_values, predicted_values, timestamps=None, prices=None):
        """
        Initialize evaluator with prediction data
        
        Parameters:
        -----------
        actual_values : array-like
            Ground truth values
        predicted_values : array-like
            Model predictions
        timestamps : array-like, optional
            Time index for the data
        prices : array-like, optional
            Asset prices for financial metrics
        """
        self.actual = np.array(actual_values)
        self.predicted = np.array(predicted_values)
        self.timestamps = timestamps
        self.prices = prices
        self.metrics = {}
        
    def calculate_error_metrics(self):
        """Calculate standard error metrics"""
        # Basic error metrics
        self.metrics['rmse'] = np.sqrt(mean_squared_error(self.actual, self.predicted))
        self.metrics['mae'] = mean_absolute_error(self.actual, self.predicted)
        
        # Percentage errors (avoiding division by zero)
        with np.errstate(divide='ignore', invalid='ignore'):
            mape = np.mean(np.abs((self.actual - self.predicted) / self.actual)) * 100
            self.metrics['mape'] = np.nan if np.isinf(mape) or np.isnan(mape) else mape
        
        # Directional accuracy (for returns/changes)
        if len(self.actual) > 1:
            actual_dirs = np.sign(np.diff(self.actual))
            pred_dirs = np.sign(np.diff(self.predicted))
            matching_dirs = (actual_dirs == pred_dirs).astype(int)
            self.metrics['dir_accuracy'] = np.mean(matching_dirs)
        
        return self.metrics
    
    def calculate_financial_metrics(self, trading_days=252, risk_free_rate=0.0):
        """Calculate financial performance metrics"""
        if self.prices is None:
            return {"error": "Price data required for financial metrics"}
        
        # Calculate strategy returns (simplified: long/short based on prediction)
        price_returns = np.diff(self.prices) / self.prices[:-1]
        predicted_direction = np.sign(self.predicted[:-1])  # Using predictions to determine position
        
        # Strategy returns (position * market return)
        strategy_returns = predicted_direction * price_returns
        
        # Basic return metrics
        self.metrics['total_return'] = np.prod(1 + strategy_returns) - 1
        self.metrics['annualized_return'] = (1 + self.metrics['total_return']) ** (trading_days / len(strategy_returns)) - 1
        self.metrics['volatility'] = np.std(strategy_returns) * np.sqrt(trading_days)
        
        # Risk metrics
        if self.metrics['volatility'] > 0:
            self.metrics['sharpe_ratio'] = (self.metrics['annualized_return'] - risk_free_rate) / self.metrics['volatility']
        
        # Drawdown analysis
        cum_returns = np.cumprod(1 + strategy_returns)
        running_max = np.maximum.accumulate(cum_returns)
        drawdowns = (cum_returns / running_max) - 1
        self.metrics['max_drawdown'] = np.min(drawdowns)
        
        return self.metrics
    
    def generate_evaluation_report(self, include_plots=True):
        """Generate comprehensive evaluation report"""
        # Calculate metrics if not already done
        if not self.metrics:
            self.calculate_error_metrics()
            if self.prices is not None:
                self.calculate_financial_metrics()
        
        # Create report structure
        report = {
            "error_metrics": {k: v for k, v in self.metrics.items() 
                             if k in ['rmse', 'mae', 'mape', 'dir_accuracy']},
            "financial_metrics": {k: v for k, v in self.metrics.items() 
                                 if k in ['total_return', 'annualized_return', 'volatility', 
                                         'sharpe_ratio', 'max_drawdown']},
            "sample_size": len(self.actual),
            "evaluation_timestamp": pd.Timestamp.now()
        }
        
        # Generate plots if requested
        if include_plots and self.timestamps is not None:
            # Actual vs predicted
            plt.figure(figsize=(10, 6))
            plt.plot(self.timestamps, self.actual, label='Actual')
            plt.plot(self.timestamps, self.predicted, label='Predicted')
            plt.title('Actual vs Predicted Values')
            plt.legend()
            plt.tight_layout()
            plt.savefig('actual_vs_predicted.png')
            
            # Error distribution
            plt.figure(figsize=(8, 5))
            errors = self.actual - self.predicted
            plt.hist(errors, bins=20)
            plt.title('Error Distribution')
            plt.axvline(x=0, color='red', linestyle='--')
            plt.tight_layout()
            plt.savefig('error_distribution.png')
            
            report["plots"] = ["actual_vs_predicted.png", "error_distribution.png"]
        
        return report
```

For a comprehensive performance evaluation system, refer to the full implementation in the evaluation module of the core library.

## Related Documentation

* [Monitoring Overview](./ts-implementation-monitoring.md)
* [Drift Detection Guide](./ts-implementation-monitoring-drift.md)
* [Alert Systems Guide](./ts-implementation-monitoring-alerts.md)
* [Continuous Improvement Guide](./ts-implementation-monitoring-improvement.md)

---

*Last Updated: 2025-05-29*