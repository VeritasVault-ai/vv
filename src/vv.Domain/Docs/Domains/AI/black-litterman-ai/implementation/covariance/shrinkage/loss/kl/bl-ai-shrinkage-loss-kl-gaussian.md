# Gaussian KL Implementation

> Implementation details for KL divergence between multivariate Gaussians in shrinkage estimation

---

## 1. Gaussian-Specific Formula

### Multivariate Gaussian KL

For two multivariate Gaussian distributions with identical means but different covariance matrices, the KL divergence has this specific form:

$$ D_{KL}(\mathcal{N}(0, \Sigma_1) || \mathcal{N}(0, \Sigma_2)) = \frac{1}{2} \left[ \log \frac{|\Sigma_2|}{|\Sigma_1|} + \text{Tr}(\Sigma_2^{-1} \Sigma_1) - n \right] $$

Where:
- $\Sigma_1$ is the covariance matrix of the first distribution
- $\Sigma_2$ is the covariance matrix of the second distribution
- $n$ is the dimensionality of the distributions
- $|\cdot|$ denotes the matrix determinant
- $\text{Tr}(\cdot)$ denotes the matrix trace

## 2. Numerical Implementation

### Basic Implementation

```python
def gaussian_kl_divergence(cov1, cov2):
    """
    Calculate KL divergence between two zero-mean multivariate Gaussians.
    
    Parameters:
    -----------
    cov1 : tensor
        First covariance matrix (n x n)
    cov2 : tensor
        Second covariance matrix (n x n)
    
    Returns:
    --------
    kl_div : float
        KL divergence value
    """
    # Get dimension
    n = cov1.shape[0]
    
    # Calculate log determinant ratio
    logdet1 = tf.linalg.logdet(cov1)
    logdet2 = tf.linalg.logdet(cov2)
    logdet_ratio = logdet2 - logdet1
    
    # Calculate trace term
    cov2_inv = tf.linalg.inv(cov2)
    trace_term = tf.linalg.trace(tf.matmul(cov2_inv, cov1))
    
    # Combine terms
    kl_div = 0.5 * (logdet_ratio + trace_term - n)
    
    return kl_div
```

### Numerical Stability Improvements

```python
def stable_gaussian_kl_divergence(cov1, cov2, epsilon=1e-8):
    """
    Numerically stable KL divergence between two zero-mean multivariate Gaussians.
    
    Parameters:
    -----------
    cov1 : tensor
        First covariance matrix (n x n)
    cov2 : tensor
        Second covariance matrix (n x n)
    epsilon : float
        Small constant for numerical stability
    
    Returns:
    --------
    kl_div : float
        KL divergence value
    """
    # Get dimension
    n = cov1.shape[0]
    
    # Ensure numerical stability by adding small constant to diagonal
    cov1_stable = cov1 + epsilon * tf.eye(n)
    cov2_stable = cov2 + epsilon * tf.eye(n)
    
    # Use Cholesky decomposition for numerical stability
    chol1 = tf.linalg.cholesky(cov1_stable)
    chol2 = tf.linalg.cholesky(cov2_stable)
    
    # Calculate log determinant ratio using Cholesky
    logdet1 = 2.0 * tf.reduce_sum(tf.math.log(tf.linalg.diag_part(chol1)))
    logdet2 = 2.0 * tf.reduce_sum(tf.math.log(tf.linalg.diag_part(chol2)))
    logdet_ratio = logdet2 - logdet1
    
    # Solve linear system instead of direct inversion
    identity = tf.eye(n)
    cov2_inv = tf.linalg.cholesky_solve(chol2, identity)
    
    # Calculate trace term
    trace_term = tf.linalg.trace(tf.matmul(cov2_inv, cov1_stable))
    
    # Combine terms
    kl_div = 0.5 * (logdet_ratio + trace_term - n)
    
    return kl_div
```

## 3. Application to Shrinkage Estimation

### KL Divergence Loss for Shrinkage

```python
def kl_shrinkage_loss(shrinkage_intensity, sample_cov, target_matrix, true_cov=None):
    """
    KL divergence loss between shrinkage estimator and true covariance.
    
    Parameters:
    -----------
    shrinkage_intensity : float or tensor
        Shrinkage intensity parameter(s) in range [0, 1]
    sample_cov : tensor
        Sample covariance matrix (n x n)
    target_matrix : tensor
        Shrinkage target matrix (n x n)
    true_cov : tensor, optional
        True covariance matrix if available (n x n)
        Default: None (uses out-of-sample data to estimate)
    
    Returns:
    --------
    loss : tensor
        KL divergence loss value
    """
    # Create shrinkage estimator
    shrinkage_estimator = (
        shrinkage_intensity * target_matrix + 
        (1 - shrinkage_intensity) * sample_cov
    )
    
    # If true covariance is not provided, it needs to be estimated
    # from out-of-sample data
    if true_cov is None:
        true_cov = estimate_true_covariance()
    
    # Calculate KL divergence
    kl_div = stable_gaussian_kl_divergence(true_cov, shrinkage_estimator)
    
    return kl_div
```

### Reverse KL Divergence

Sometimes the reverse KL divergence is more appropriate:

```python
def reverse_kl_shrinkage_loss(shrinkage_intensity, sample_cov, target_matrix, true_cov=None):
    """
    Reverse KL divergence (KL(shrinkage||true)) loss.
    
    Parameters:
    -----------
    shrinkage_intensity : float or tensor
        Shrinkage intensity parameter(s) in range [0, 1]
    sample_cov : tensor
        Sample covariance matrix (n x n)
    target_matrix : tensor
        Shrinkage target matrix (n x n)
    true_cov : tensor, optional
        True covariance matrix if available (n x n)
        Default: None (uses out-of-sample data to estimate)
    
    Returns:
    --------
    loss : tensor
        Reverse KL divergence loss value
    """
    # Create shrinkage estimator
    shrinkage_estimator = (
        shrinkage_intensity * target_matrix + 
        (1 - shrinkage_intensity) * sample_cov
    )
    
    # If true covariance is not provided, it needs to be estimated
    # from out-of-sample data
    if true_cov is None:
        true_cov = estimate_true_covariance()
    
    # Calculate reverse KL divergence
    reverse_kl = stable_gaussian_kl_divergence(shrinkage_estimator, true_cov)
    
    return reverse_kl
```

## 4. Symmetrized KL Divergence

### Jeffreys Divergence Implementation

```python
def jeffreys_shrinkage_loss(shrinkage_intensity, sample_cov, target_matrix, true_cov=None):
    """
    Jeffreys divergence (symmetrized KL) loss for shrinkage.
    
    Parameters:
    -----------
    shrinkage_intensity : float or tensor
        Shrinkage intensity parameter(s) in range [0, 1]
    sample_cov : tensor
        Sample covariance matrix (n x n)
    target_matrix : tensor
        Shrinkage target matrix (n x n)
    true_cov : tensor, optional
        True covariance matrix if available (n x n)
        Default: None (uses out-of-sample data to estimate)
    
    Returns:
    --------
    loss : tensor
        Jeffreys divergence loss value
    """
    # Create shrinkage estimator
    shrinkage_estimator = (
        shrinkage_intensity * target_matrix + 
        (1 - shrinkage_intensity) * sample_cov
    )
    
    # If true covariance is not provided, it needs to be estimated
    if true_cov is None:
        true_cov = estimate_true_covariance()
    
    # Calculate forward and reverse KL divergences
    forward_kl = stable_gaussian_kl_divergence(true_cov, shrinkage_estimator)
    reverse_kl = stable_gaussian_kl_divergence(shrinkage_estimator, true_cov)
    
    # Calculate Jeffreys divergence
    jeffreys = 0.5 * (forward_kl + reverse_kl)
    
    return jeffreys
```

## 5. Efficient Matrix Operations

### Direct Formula Implementation

For improved computational efficiency, we can implement the KL divergence directly:

```python
def efficient_gaussian_kl(cov1, cov2):
    """
    Efficient implementation of KL divergence using eigenvalue decomposition.
    
    Particularly useful for repeated calculations with the same matrices.
    """
    # Get dimension
    n = cov1.shape[0]
    
    # Compute eigendecomposition of cov2
    eigenvalues, eigenvectors = tf.linalg.eigh(cov2)
    
    # Ensure numerical stability
    eigenvalues = tf.maximum(eigenvalues, 1e-8)
    
    # Compute inverse of cov2 using eigendecomposition
    # This avoids direct matrix inversion
    sqrt_inv_eigenvalues = tf.math.rsqrt(eigenvalues)
    inv_sqrt_cov2 = eigenvectors * tf.expand_dims(sqrt_inv_eigenvalues, 0)
    
    # Transform cov1 into the eigenspace of cov2
    transformed_cov1 = tf.matmul(inv_sqrt_cov2, cov1, transpose_a=True)
    transformed_cov1 = tf.matmul(transformed_cov1, inv_sqrt_cov2)
    
    # Compute trace term directly
    trace_term = tf.reduce_sum(tf.linalg.diag_part(transformed_cov1))
    
    # Compute log determinant terms
    logdet1 = tf.reduce_sum(tf.math.log(tf.linalg.eigvalsh(cov1)))
    logdet2 = tf.reduce_sum(tf.math.log(eigenvalues))
    
    # Combine terms
    kl = 0.5 * (trace_term + logdet2 - logdet1 - n)
    
    return kl
```

## 6. Batch Processing

### Vectorized Implementation

For training with batches of covariance matrices:

```python
def batch_gaussian_kl(cov1_batch, cov2_batch, epsilon=1e-8):
    """
    Vectorized KL divergence for batches of covariance matrices.
    
    Parameters:
    -----------
    cov1_batch : tensor
        Batch of first covariance matrices (batch_size x n x n)
    cov2_batch : tensor
        Batch of second covariance matrices (batch_size x n x n)
    epsilon : float
        Small constant for numerical stability
    
    Returns:
    --------
    kl_div_batch : tensor
        Batch of KL divergence values (batch_size)
    """
    # Get dimensions
    batch_size = cov1_batch.shape[0]
    n = cov1_batch.shape[1]
    
    # Initialize output tensor
    kl_batch = tf.zeros(batch_size)
    
    # Process each matrix in the batch
    for i in range(batch_size):
        # Extract individual covariance matrices
        cov1 = cov1_batch[i]
        cov2 = cov2_batch[i]
        
        # Ensure numerical stability
        cov1 = cov1 + epsilon * tf.eye(n)
        cov2 = cov2 + epsilon * tf.eye(n)
        
        # Calculate KL divergence
        kl_batch = tf.tensor_scatter_nd_update(
            kl_batch, 
            [[i]], 
            [stable_gaussian_kl_divergence(cov1, cov2, epsilon)]
        )
    
    return kl_batch
```

## 7. Testing and Validation

### Unit Test Example

```python
def test_gaussian_kl():
    """Unit test for Gaussian KL divergence"""
    # Create sample covariance matrices
    n = 5
    np.random.seed(42)
    
    # Create positive definite matrices
    A = np.random.randn(n, n)
    cov1 = A @ A.T + np.eye(n)  # Ensure positive definiteness
    
    B = np.random.randn(n, n)
    cov2 = B @ B.T + np.eye(n)  # Ensure positive definiteness
    
    # Calculate KL divergence
    kl = gaussian_kl_divergence(cov1, cov2)
    
    # Manually calculate for verification
    # Formula: 0.5 * [log(|Σ2|/|Σ1|) + Tr(Σ2^-1 Σ1) - n]
    logdet1 = np.log(np.linalg.det(cov1))
    logdet2 = np.log(np.linalg.det(cov2))
    inv_cov2 = np.linalg.inv(cov2)
    trace_term = np.trace(inv_cov2 @ cov1)
    
    expected_kl = 0.5 * (logdet2 - logdet1 + trace_term - n)
    
    # Check result
    assert np.abs(kl - expected_kl) < 1e-5
```

---

**Related Documents:**
* [KL Divergence Definition](./bl-ai-shrinkage-loss-kl-definition.md)
* [KL Loss Integration](./bl-ai-shrinkage-loss-kl-integration.md)
* [KL Loss Optimization](./bl-ai-shrinkage-loss-kl-optimization.md)
* [ML-Enhanced Shrinkage: Overview & Theory](../../../bl-ai-shrinkage-overview.md)
* [Frobenius Norm Loss Function](../../frobenius/bl-ai-shrinkage-loss-frobenius.md)
* [Spectral Norm Loss Function](../../spectral/bl-ai-shrinkage-loss-spectral.md)