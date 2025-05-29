# Time Series Drift Detection

> Techniques for detecting and responding to drift in time series models

---

## Feature Drift Monitoring

### Statistical Tests

* **Kolmogorov-Smirnov Test**:
  * Non-parametric test comparing distributions
  * Measures maximum distance between distributions
  * Effective for detecting location and shape changes
  * Implementation: `scipy.stats.ks_2samp`

* **Jensen-Shannon Divergence**:
  * Measures similarity between distributions
  * Symmetric version of KL divergence
  * Bounded between 0 and 1
  * More stable for probability distributions

* **Population Stability Index (PSI)**:
  * Common in credit risk modeling
  * PSI > 0.25 indicates significant changes
  * Formula: `PSI = Σ (Actual% - Expected%) × ln(Actual% / Expected%)`
  * Sensitive to distribution changes

### Visualization Techniques

* **Distribution Overlays**:
  * Reference vs current histograms
  * QQ plots for direct comparison
  * Percentile distribution charts
  * Moving window distribution charts

* **Time Series Visualization**:
  * Statistical control charts
  * Moving statistics (mean, variance)
  * Feature correlation heatmaps
  * Dimension reduction plots (PCA, t-SNE)

### Financial-Specific Approaches

* **Regime Change Detection**:
  * Hidden Markov Models
  * CUSUM algorithms
  * Correlation structure monitoring
  * Volatility clustering analysis

* **Market Condition Monitoring**:
  * Volatility indices (VIX)
  * Liquidity measures
  * Trading volume analysis
  * Macro indicator tracking

## Prediction Drift Monitoring

### Distribution Monitoring

* **Output Distribution Shifts**:
  * Mean and variance tracking
  * Extreme value frequency
  * Prediction histogram comparison
  * Higher moment statistics (skewness, kurtosis)

* **Temporal Pattern Changes**:
  * Seasonality strength changes
  * Autocorrelation function shifts
  * Cyclic pattern disruption
  * Trend component stability

### Performance Degradation

* **Rolling Window Metrics**:
  * Moving RMSE/MAE tracking
  * Directional accuracy stability
  * Residual distribution monitoring
  * Financial metric deterioration

* **Benchmark Comparison**:
  * Gap to naive forecasts
  * Performance vs simple models
  * Relative decay rate
  * Competitive ranking shifts

### Confidence Analysis

* **Uncertainty Metrics**:
  * Calibration curve monitoring
  * Prediction interval coverage
  * Confidence score distribution
  * Expected vs. actual error rates

* **Ensemble Disagreement**:
  * Model voting patterns
  * Variance between ensemble members
  * Confidence correlation
  * Majority/minority prediction tracking

## Drift Response Strategies

### Automated Responses

* **Alert Triggering**:
  * Severity classification
  * Compound alert conditions
  * Contextual alert enrichment
  * Priority determination

* **Model Adjustment**:
  * Online learning activation
  * Dynamic feature importance
  * Adaptive threshold adjustment
  * Conservative parameter shifts

### Manual Investigation Workflows

* **Investigation Tools**:
  * Drift visualization dashboards
  * Feature contribution analysis
  * Historical drift patterns
  * Comparative model evaluation

* **Root Cause Analysis**:
  * Incremental feature analysis
  * Data quality assessment
  * External event correlation
  * Market regime identification

### Governance Procedures

* **Documentation Requirements**:
  * Drift event logging
  * Investigation records
  * Action justification
  * Resolution tracking

* **Stakeholder Communication**:
  * Alert notification templates
  * Escalation procedures
  * Impact assessment reports
  * Remediation plans

## Implementation Example

Below is a simple implementation of a drift detector for time series features:

```python
import numpy as np
from scipy import stats

class FeatureDriftDetector:
    """Basic feature drift detector for time series data"""
    
    def __init__(self, reference_data, p_threshold=0.01):
        """
        Initialize with reference data
        
        Parameters:
        -----------
        reference_data : pd.DataFrame
            Reference dataset with features
        p_threshold : float
            p-value threshold for KS test
        """
        self.reference_data = reference_data
        self.p_threshold = p_threshold
        self.feature_stats = self._compute_feature_stats()
        
    def _compute_feature_stats(self):
        """Compute statistics for reference features"""
        stats_dict = {}
        for col in self.reference_data.columns:
            stats_dict[col] = {
                'mean': self.reference_data[col].mean(),
                'std': self.reference_data[col].std(),
                'distribution': self.reference_data[col].values
            }
        return stats_dict
    
    def check_drift(self, current_data):
        """
        Check for drift in current data
        
        Parameters:
        -----------
        current_data : pd.DataFrame
            Current data to check for drift
            
        Returns:
        --------
        dict : Drift detection results
        """
        results = {
            'drift_detected': False,
            'details': {}
        }
        
        for col in current_data.columns:
            if col in self.feature_stats:
                # Perform KS test
                ks_stat, p_value = stats.ks_2samp(
                    self.feature_stats[col]['distribution'],
                    current_data[col].values
                )
                
                # Check if drift detected
                is_drift = p_value < self.p_threshold
                
                if is_drift:
                    results['drift_detected'] = True
                
                # Store details
                results['details'][col] = {
                    'drift_detected': is_drift,
                    'p_value': p_value,
                    'ks_statistic': ks_stat,
                    'ref_mean': self.feature_stats[col]['mean'],
                    'current_mean': current_data[col].mean(),
                    'mean_change_pct': (current_data[col].mean() - self.feature_stats[col]['mean']) / 
                                      self.feature_stats[col]['mean'] * 100 if self.feature_stats[col]['mean'] != 0 else float('inf')
                }
        
        return results
```

For prediction drift detection, a similar approach can be used:

```python
class PredictionDriftDetector:
    """Drift detector for model predictions"""
    
    def __init__(self, reference_predictions, p_threshold=0.01):
        """
        Initialize with reference predictions
        
        Parameters:
        -----------
        reference_predictions : array-like
            Reference prediction distribution
        p_threshold : float
            p-value threshold for KS test
        """
        self.reference_predictions = np.array(reference_predictions)
        self.p_threshold = p_threshold
        self.reference_stats = {
            'mean': np.mean(reference_predictions),
            'std': np.std(reference_predictions),
            'min': np.min(reference_predictions),
            'max': np.max(reference_predictions)
        }
        
    def check_drift(self, current_predictions):
        """
        Check for drift in current predictions
        
        Parameters:
        -----------
        current_predictions : array-like
            Current predictions to check for drift
            
        Returns:
        --------
        dict : Drift detection results
        """
        current_predictions = np.array(current_predictions)
        
        # Perform KS test
        ks_stat, p_value = stats.ks_2samp(
            self.reference_predictions,
            current_predictions
        )
        
        # Check if drift detected
        is_drift = p_value < self.p_threshold
        
        # Calculate current stats
        current_stats = {
            'mean': np.mean(current_predictions),
            'std': np.std(current_predictions),
            'min': np.min(current_predictions),
            'max': np.max(current_predictions)
        }
        
        # Calculate changes
        changes = {
            key: ((current_stats[key] - self.reference_stats[key]) / 
                 self.reference_stats[key] * 100 if self.reference_stats[key] != 0 else float('inf'))
            for key in ['mean', 'std']
        }
        
        return {
            'drift_detected': is_drift,
            'p_value': p_value,
            'ks_statistic': ks_stat,
            'reference_stats': self.reference_stats,
            'current_stats': current_stats,
            'percent_changes': changes
        }
```

For a complete drift detection system implementation, refer to the drift detection module in the monitoring library.

## Related Documentation

* [Monitoring Overview](./ts-implementation-monitoring.md)
* [Performance Evaluation Guide](./ts-implementation-monitoring-performance.md)
* [Alert Systems Guide](./ts-implementation-monitoring-alerts.md)
* [Continuous Improvement Guide](./ts-implementation-monitoring-improvement.md)

---

*Last Updated: 2025-05-29*