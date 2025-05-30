# ML-Enhanced Shrinkage: Training Evaluation - Part 1

> Core evaluation framework and statistical metrics for ML-Enhanced Shrinkage models

---

## 1. Evaluation Framework Overview

The ML-Enhanced Shrinkage models employ a comprehensive evaluation framework designed to assess both statistical and financial performance across varying market conditions.

### Evaluation Objectives

The evaluation process focuses on these key objectives:

* **Statistical Accuracy**: How well the model estimates the true covariance structure
* **Financial Utility**: Impact on downstream portfolio construction and performance
* **Generalization**: Performance consistency across different market regimes
* **Calibration Quality**: Accuracy of uncertainty estimates and confidence bounds

### Evaluation Workflow

```
┌─────────────────┐     ┌─────────────────┐     ┌─────────────────┐     ┌─────────────────┐
│ In-Sample       │     │ Out-of-Sample   │     │ Financial       │     │ Uncertainty     │
│ Validation      │────▶│ Testing         │────▶│ Metrics         │────▶│ Calibration     │
│                 │     │                 │     │                 │     │                 │
└─────────────────┘     └─────────────────┘     └─────────────────┘     └─────────────────┘
```

## 2. Statistical Evaluation Metrics

### Matrix Distance Metrics

Several specialized metrics are used to evaluate covariance matrix estimates:

* **Frobenius Norm**: 
  ```python
  def frobenius_distance(predicted_cov, true_cov):
      """Calculate Frobenius norm between predicted and true covariance matrices"""
      return torch.norm(predicted_cov - true_cov, p='fro')
  ```

* **Log-Determinant Distance**:
  ```python
  def logdet_distance(predicted_cov, true_cov):
      """Calculate log-determinant based distance between covariance matrices"""
      n = predicted_cov.shape[0]
      pred_logdet = torch.logdet(predicted_cov)
      true_logdet = torch.logdet(true_cov)
      pred_inv_true = torch.mm(torch.inverse(predicted_cov), true_cov)
      return (torch.trace(pred_inv_true) - n - (true_logdet - pred_logdet))
  ```

* **Eigenvalue Distribution Metrics**:
  ```python
  def eigenvalue_distance(predicted_cov, true_cov):
      """Measure distance between eigenvalue distributions"""
      pred_eigvals = torch.linalg.eigvalsh(predicted_cov)
      true_eigvals = torch.linalg.eigvalsh(true_cov)
      return torch.mean((torch.sort(pred_eigvals).values - torch.sort(true_eigvals).values)**2)
  ```

### Shrinkage Intensity Evaluation

The accuracy of predicted shrinkage intensities is evaluated using:

* **Mean Squared Error**:
  ```python
  def shrinkage_intensity_mse(predicted_intensities, optimal_intensities):
      """Calculate MSE between predicted and optimal shrinkage intensities"""
      return torch.mean((predicted_intensities - optimal_intensities)**2)
  ```

* **Directional Accuracy**:
  ```python
  def shrinkage_directional_accuracy(predicted_intensities, optimal_intensities):
      """Calculate percentage of correctly predicted intensity directions"""
      pred_directions = torch.sign(predicted_intensities[1:] - predicted_intensities[:-1])
      true_directions = torch.sign(optimal_intensities[1:] - optimal_intensities[:-1])
      correct = (pred_directions == true_directions).float()
      return torch.mean(correct)
  ```

## 3. Cross-Validation Methodology

### Time-Series Cross-Validation

Given the temporal nature of financial data, specialized cross-validation approaches are employed:

* **Rolling Window Validation**:
  ```python
  def rolling_window_cv(data, model, window_size, step_size):
      """Implement rolling window cross-validation"""
      results = []
      for i in range(0, len(data) - window_size, step_size):
          train_data = data[i:i+window_size]
          test_data = data[i+window_size:i+window_size+step_size]
          
          # Train model on window
          model.fit(train_data)
          
          # Evaluate on test period
          performance = evaluate_model(model, test_data)
          results.append(performance)
      
      return results
  ```

* **Expanding Window Validation**:
  ```python
  def expanding_window_cv(data, model, initial_size, step_size):
      """Implement expanding window cross-validation"""
      results = []
      for i in range(initial_size, len(data), step_size):
          train_data = data[:i]
          test_data = data[i:i+step_size]
          
          # Train model on expanding window
          model.fit(train_data)
          
          # Evaluate on test period
          performance = evaluate_model(model, test_data)
          results.append(performance)
      
      return results
  ```

### Regime-Based Evaluation

Performance is also assessed across different market regimes:

* **Volatility Regimes**: High, medium, and low volatility periods
* **Correlation Regimes**: High, medium, and low average correlation periods
* **Market Direction**: Bull, bear, and sideways markets
* **Crisis Periods**: Specific stress events and market crashes

## 4. Benchmarking Framework

### Baseline Models

The ML-Enhanced Shrinkage models are compared against several baseline approaches:

* **Sample Covariance**: Unregularized maximum likelihood estimator
* **Constant Correlation Shrinkage**: Ledoit-Wolf shrinkage to constant correlation
* **Identity Shrinkage**: Ledoit-Wolf shrinkage to identity matrix
* **Factor Models**: Principal Component and statistical factor models
* **EWMA Models**: Exponentially weighted moving average estimators

### Comparative Metrics

Benchmark comparisons use these key metrics:

* **Relative Improvement**:
  ```python
  def relative_improvement(model_error, baseline_error):
      """Calculate relative improvement over baseline"""
      return (baseline_error - model_error) / baseline_error
  ```

* **Statistical Significance**:
  ```python
  def test_significance(model_errors, baseline_errors, alpha=0.05):
      """Test if improvement is statistically significant"""
      from scipy.stats import ttest_rel
      t_stat, p_value = ttest_rel(baseline_errors, model_errors)
      return p_value < alpha
  ```

* **Percentile Ranking**: Ranking among all benchmarks across test periods
* **Consistency Metrics**: Frequency of outperformance against benchmarks

---

**Related Documents:**
* [ML-Shrinkage: Training Evaluation - Part 2](./ml-shrinkage-training-evaluation-part2.md)
* [ML-Shrinkage: Training Process](./ml-shrinkage-training-process.md)
* [ML-Shrinkage: Training Architecture Overview](./ml-shrinkage-training-architecture-overview.md)