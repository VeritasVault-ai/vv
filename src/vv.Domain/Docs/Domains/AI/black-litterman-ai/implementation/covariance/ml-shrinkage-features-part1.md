# ML-Enhanced Shrinkage: Feature Engineering (Part 1)

> Overview and statistical feature extraction for ML-Enhanced Shrinkage models

---

## 1. Feature Engineering Overview

### Purpose and Importance

Feature engineering is a critical component of the ML-Enhanced Shrinkage approach. High-quality features enable the neural network to:

- Recognize patterns in covariance structure
- Identify optimal shrinkage intensities
- Adapt to different market conditions
- Provide uncertainty estimates

The feature extraction pipeline transforms raw return data and sample covariance matrices into structured inputs that capture relevant statistical properties and market dynamics.

### Feature Categories

The ML-Enhanced Shrinkage implementation uses four main categories of features:

1. **Eigenvalue Features**: Capture the statistical structure of the covariance matrix
2. **Correlation Features**: Represent relationships between assets
3. **Temporal Features**: Incorporate time-varying aspects (covered in Part 2)
4. **Regime Features**: Encode market regime information (covered in Part 2)

## 2. Eigenvalue Features

### Theoretical Motivation

Eigenvalue features are directly related to the theoretical foundations of shrinkage estimation. They provide information about:

- The condition number of the matrix
- The distribution of variance across dimensions
- The potential for estimation error

### Implementation

The following eigenvalue features are extracted from the sample covariance matrix:

```python
def extract_eigenvalue_features(sample_cov: np.ndarray) -> Dict[str, float]:
    """
    Extract features based on eigenvalue properties of the sample covariance matrix.
    
    Parameters:
    -----------
    sample_cov : numpy.ndarray
        Sample covariance matrix of shape (n_assets, n_assets)
        
    Returns:
    --------
    dict
        Dictionary of eigenvalue-based features
    """
    eigenvalues = np.linalg.eigvalsh(sample_cov)
    
    features = {
        "eigenvalue_max": np.max(eigenvalues),
        "eigenvalue_min": np.min(eigenvalues),
        "eigenvalue_mean": np.mean(eigenvalues),
        "eigenvalue_median": np.median(eigenvalues),
        "eigenvalue_std": np.std(eigenvalues),
        "condition_number": np.max(eigenvalues) / np.max(np.spacing(1), np.min(eigenvalues)),
        "trace": np.trace(sample_cov),
        "effective_rank": np.sum(eigenvalues) / np.max(eigenvalues),
        "entropy": -np.sum((eigenvalues / np.sum(eigenvalues)) * 
                           np.log(eigenvalues / np.sum(eigenvalues) + 1e-10)),
    }
    
    # Eigenvalue distribution features
    sorted_eigenvalues = np.sort(eigenvalues)[::-1]  # Descending order
    for i in range(min(10, len(eigenvalues))):
        features[f"eigenvalue_ratio_{i+1}"] = (sorted_eigenvalues[i] / 
                                              np.sum(eigenvalues))
    
    return features
```

### Key Eigenvalue Features

The most important eigenvalue features include:

1. **Condition Number**: Ratio of largest to smallest eigenvalue, indicating numerical stability
2. **Effective Rank**: Measure of the effective dimensionality of the data
3. **Eigenvalue Distribution**: Percentage of variance explained by principal components
4. **Eigenvalue Entropy**: Measure of the dispersion of eigenvalues

## 3. Correlation Features

### Theoretical Motivation

Correlation features provide insights into the interdependence structure of assets, which is crucial for:

- Identifying clustered relationships
- Detecting diversification opportunities
- Recognizing correlation breakdowns during market stress

### Implementation

The following correlation features are extracted from the sample correlation matrix:

```python
def extract_correlation_features(sample_cov: np.ndarray) -> Dict[str, float]:
    """
    Extract features based on correlation properties.
    
    Parameters:
    -----------
    sample_cov : numpy.ndarray
        Sample covariance matrix of shape (n_assets, n_assets)
        
    Returns:
    --------
    dict
        Dictionary of correlation-based features
    """
    # Convert covariance to correlation
    std_devs = np.sqrt(np.diag(sample_cov))
    corr_matrix = sample_cov / np.outer(std_devs, std_devs)
    np.fill_diagonal(corr_matrix, 0)  # Zero out diagonal for some calculations
    
    features = {
        "correlation_mean": np.sum(corr_matrix) / (corr_matrix.shape[0]**2 - corr_matrix.shape[0]),
        "correlation_std": np.std(corr_matrix[np.triu_indices(corr_matrix.shape[0], k=1)]),
        "correlation_max": np.max(corr_matrix),
        "correlation_min": np.min(corr_matrix),
        "correlation_abs_mean": np.mean(np.abs(corr_matrix[corr_matrix != 0])),
        "correlation_positive_ratio": np.sum(corr_matrix > 0) / np.sum(corr_matrix != 0),
        "correlation_negative_ratio": np.sum(corr_matrix < 0) / np.sum(corr_matrix != 0),
    }
    
    # Add diagonal statistics back
    np.fill_diagonal(corr_matrix, 1)
    
    # Block structure features
    features["correlation_block_indicator"] = detect_block_structure(corr_matrix)
    
    # Threshold-based features
    thresholds = [0.2, 0.4, 0.6, 0.8]
    for t in thresholds:
        features[f"correlation_above_{t}"] = np.mean(np.abs(corr_matrix) > t)
    
    return features
```

### Key Correlation Features

The most significant correlation features include:

1. **Average Correlation**: Mean of off-diagonal correlation values
2. **Correlation Dispersion**: Standard deviation of correlations
3. **Correlation Sign Distribution**: Ratio of positive to negative correlations
4. **Block Structure Indicator**: Measure of clustered correlation patterns
5. **Threshold Exceedance**: Percentage of correlations above specific thresholds

## 4. Feature Normalization and Preparation

### Scaling and Normalization

All extracted features are normalized to ensure model stability:

```python
def normalize_features(features: Dict[str, float]) -> np.ndarray:
    """
    Normalize features to prepare for model input.
    
    Parameters:
    -----------
    features : dict
        Dictionary of extracted features
        
    Returns:
    --------
    numpy.ndarray
        Normalized feature vector
    """
    # Convert to numpy array
    feature_names = sorted(features.keys())
    feature_vector = np.array([features[name] for name in feature_names])
    
    # Apply normalization (stored scalers are used in production)
    feature_vector = (feature_vector - FEATURE_MEANS) / FEATURE_STDS
    
    # Handle potential NaN or inf values
    feature_vector = np.nan_to_num(feature_vector, nan=0.0, posinf=3.0, neginf=-3.0)
    
    return feature_vector
```

### Feature Selection

Not all features are equally informative. The implementation uses feature importance analysis to select the most relevant features:

```python
def select_features(all_features: Dict[str, float]) -> Dict[str, float]:
    """
    Select most informative features based on importance analysis.
    
    Parameters:
    -----------
    all_features : dict
        Dictionary of all extracted features
        
    Returns:
    --------
    dict
        Dictionary of selected features
    """
    selected_features = {}
    
    for feature_name in SELECTED_FEATURE_NAMES:
        if feature_name in all_features:
            selected_features[feature_name] = all_features[feature_name]
    
    return selected_features
```

---

**Related Documents:**
* [ML-Enhanced Shrinkage: Feature Engineering (Part 2)](./ml-shrinkage-features-part2.md)
* [ML-Enhanced Shrinkage: Model Training](./ml-shrinkage-training.md)
* [ML-Enhanced Shrinkage: Architecture](./ml-shrinkage-architecture.md)