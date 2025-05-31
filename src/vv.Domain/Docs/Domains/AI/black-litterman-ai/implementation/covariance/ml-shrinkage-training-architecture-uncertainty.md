---
document_type: architecture
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

# ML-Enhanced Shrinkage: Uncertainty Estimation Architecture

> Architectural design for uncertainty quantification in ML-Enhanced Shrinkage models

---

## 1. Uncertainty Framework Overview

The uncertainty estimation architecture extends the base ML-Enhanced Shrinkage model to provide probabilistic predictions rather than point estimates, enabling more informed risk management and decision making.

### Uncertainty Quantification Goals

```
┌─────────────────────────────────────────────────────────────────┐
│               Uncertainty Estimation Architecture                │
└─────────────────────────────────────────────────────────────────┘
                                │
        ┌─────────────┬─────────┴────────┬─────────────┐
        │             │                  │             │
┌───────▼───────┐┌────▼─────┐    ┌───────▼───────┐┌────▼─────┐
│ Aleatoric     ││ Epistemic│    │ Calibration   ││ Prediction│
│ Uncertainty   ││ Uncertainty    │ Module       ││ Intervals │
└───────┬───────┘└────┬─────┘    └───────┬───────┘└────┬─────┘
        │             │                  │             │
        └─────────────┴──────────────────┴─────────────┘
                                │
                      ┌─────────▼─────────┐
                      │ Probabilistic     │
                      │ Shrinkage Output  │
                      └───────────────────┘
```

The uncertainty architecture addresses:

1. **Aleatoric Uncertainty**: Inherent randomness in the data (irreducible)
2. **Epistemic Uncertainty**: Model uncertainty due to limited knowledge/data (reducible)
3. **Calibration**: Ensuring uncertainty estimates reflect true error distributions
4. **Prediction Intervals**: Converting uncertainties to actionable confidence bounds

## 2. Architectural Approaches

The implementation supports multiple uncertainty estimation approaches:

### Bayesian Neural Networks

* **Model Architecture**: Extended with Bayesian layers for weight distributions
* **Weight Representation**: Mean and variance parameters for each weight
* **Training Method**: Variational inference with KL divergence regularization
* **Inference**: Monte Carlo sampling for posterior predictions

```python
class BayesianLinear(nn.Module):
    def __init__(self, in_features, out_features, prior_sigma=1.0):
        super().__init__()
        self.in_features = in_features
        self.out_features = out_features
        
        # Weight mean parameters
        self.weight_mu = nn.Parameter(torch.Tensor(out_features, in_features))
        # Weight variance parameters (log space for numerical stability)
        self.weight_log_sigma = nn.Parameter(torch.Tensor(out_features, in_features))
        
        # Bias parameters
        self.bias_mu = nn.Parameter(torch.Tensor(out_features))
        self.bias_log_sigma = nn.Parameter(torch.Tensor(out_features))
        
        self.prior_sigma = prior_sigma
        self.reset_parameters()
        
    def reset_parameters(self):
        # Initialize means with He initialization
        nn.init.kaiming_uniform_(self.weight_mu, a=math.sqrt(5))
        # Initialize log variances to small negative values
        nn.init.constant_(self.weight_log_sigma, -5)
        
        fan_in, _ = nn.init._calculate_fan_in_and_fan_out(self.weight_mu)
        bound = 1 / math.sqrt(fan_in)
        nn.init.uniform_(self.bias_mu, -bound, bound)
        nn.init.constant_(self.bias_log_sigma, -5)
        
    def forward(self, x):
        # Sample weights and biases from their distributions
        weight_epsilon = torch.randn_like(self.weight_mu)
        weight = self.weight_mu + torch.exp(self.weight_log_sigma) * weight_epsilon
        
        bias_epsilon = torch.randn_like(self.bias_mu)
        bias = self.bias_mu + torch.exp(self.bias_log_sigma) * bias_epsilon
        
        # Compute forward pass
        out = F.linear(x, weight, bias)
        return out
    
    def kl_divergence(self):
        # KL divergence between weight posterior and prior
        kl_weight = self._kl_divergence(
            self.weight_mu, torch.exp(self.weight_log_sigma), 
            0, self.prior_sigma
        )
        kl_bias = self._kl_divergence(
            self.bias_mu, torch.exp(self.bias_log_sigma), 
            0, self.prior_sigma
        )
        return kl_weight + kl_bias
        
    def _kl_divergence(self, mu_1, sigma_1, mu_2, sigma_2):
        kl = torch.log(sigma_2 / sigma_1) + (sigma_1**2 + (mu_1 - mu_2)**2) / (2 * sigma_2**2) - 0.5
        return torch.sum(kl)
```

### Deep Ensembles

* **Architecture**: Multiple independently trained models with the same structure
* **Training Method**: Standard training with different random initializations
* **Diversity Promotion**: Adversarial training and bootstrap sampling
* **Inference**: Statistical aggregation of ensemble predictions

```python
class DeepEnsemble:
    def __init__(self, model_class, model_args, n_models=5):
        self.models = [model_class(**model_args) for _ in range(n_models)]
        
    def train(self, train_loader, valid_loader, epochs, optimizer_class, optimizer_args):
        for i, model in enumerate(self.models):
            optimizer = optimizer_class(model.parameters(), **optimizer_args)
            
            # Train each model independently with different random initialization
            for epoch in range(epochs):
                # Training code for individual models...
                pass
                
    def predict_with_uncertainty(self, x, n_samples=10):
        # Collect predictions from all models
        predictions = []
        for model in self.models:
            model.eval()
            with torch.no_grad():
                preds = model(x)
                predictions.append(preds)
        
        # Stack predictions [n_models, batch_size, output_dim]
        predictions = torch.stack(predictions)
        
        # Compute mean and variance across models
        mean_pred = torch.mean(predictions, dim=0)
        variance = torch.var(predictions, dim=0)
        
        return mean_pred, variance
```

### Monte Carlo Dropout

* **Architecture**: Standard network with strategic dropout layers
* **Training Method**: Regular training with dropout
* **Inference**: Multiple forward passes with dropout enabled
* **Uncertainty**: Variance of predictions across multiple passes

```python
class MCDropoutModel(nn.Module):
    def __init__(self, input_dim, hidden_dims, output_dim, dropout_rate=0.3):
        super().__init__()
        self.dropout_rate = dropout_rate
        
        layers = []
        dims = [input_dim] + hidden_dims
        
        for i in range(len(dims) - 1):
            layers.append(nn.Linear(dims[i], dims[i+1]))
            layers.append(nn.ReLU())
            layers.append(nn.Dropout(p=dropout_rate))
            
        layers.append(nn.Linear(dims[-1], output_dim))
        self.network = nn.Sequential(*layers)
        
    def forward(self, x):
        return self.network(x)
    
    def predict_with_uncertainty(self, x, n_samples=50):
        self.train()  # Enable dropout during inference
        
        samples = []
        for _ in range(n_samples):
            with torch.no_grad():
                pred = self.forward(x)
                samples.append(pred)
                
        # Stack samples [n_samples, batch_size, output_dim]
        samples = torch.stack(samples)
        
        # Compute mean and variance
        mean_pred = torch.mean(samples, dim=0)
        variance = torch.var(samples, dim=0)
        
        return mean_pred, variance
```

## 3. Uncertainty Calibration

The architecture includes calibration components to ensure reliable uncertainty estimates:

### Temperature Scaling

* **Approach**: Post-hoc calibration of confidence by scaling logits
* **Training**: Optimization on validation set after main model training
* **Application**: Applied during inference to adjust prediction distributions

### Isotonic Regression

* **Approach**: Non-parametric calibration mapping predicted to empirical probabilities
* **Training**: Fit on validation set using predicted confidence vs. observed accuracy
* **Application**: Transforms raw model confidences into calibrated probabilities

### Quantile Regression

* **Approach**: Direct prediction of quantiles of the target distribution
* **Training**: Custom quantile loss functions for desired probability levels
* **Application**: Provides prediction intervals without assuming distribution form

## 4. Implementation Details

The uncertainty architecture includes specialized components:

### Variance Prediction Heads

```
┌─────────────────┐
│ Shared Feature  │
│ Layers          │
└────────┬────────┘
         │
    ┌────┴─────┐
    │          │
    ▼          ▼
┌─────────┐ ┌─────────┐
│ Mean    │ │ Variance│
│ Head    │ │ Head    │
└─────────┘ └─────────┘
```

* **Mean Head**: Predicts expected shrinkage parameter value
* **Variance Head**: Predicts uncertainty (aleatoric component)
* **Training**: Joint optimization with heteroscedastic loss function

### Bayesian Layers Integration

* **Weight Priors**: Informed by empirical shrinkage distributions
* **Variational Posterior**: Diagonal Gaussian approximation for efficiency
* **Loss Function**: ELBO with data likelihood and KL divergence terms
* **Sampling Strategy**: Efficient parallel sampling during inference

### Multi-Modal Output Distributions

* **Mixture Density Networks**: For complex uncertainty landscapes
* **Distribution Parameters**: Mean, variance, and mixture weights
* **Training Objective**: Negative log-likelihood of mixture model
* **Inference**: Full distribution for downstream decision-making

## 5. Practical Applications

The uncertainty estimates enable several advanced applications:

### Risk-Aware Portfolio Construction

* **Objective**: Incorporate parameter uncertainty into optimization
* **Method**: Robust optimization using prediction intervals
* **Benefit**: More conservative allocations when uncertainty is high

### Adaptive Shrinkage Intensity

* **Objective**: Adjust shrinkage based on prediction confidence
* **Method**: Higher shrinkage when uncertainty is high
* **Benefit**: Automatic risk management based on estimation quality

### Model Monitoring

* **Objective**: Detect when model predictions become unreliable
* **Method**: Track uncertainty metrics over time
* **Benefit**: Trigger retraining or fallback to simpler models when needed

### Active Learning

* **Objective**: Prioritize data collection for model improvement
* **Method**: Target scenarios with high epistemic uncertainty
* **Benefit**: More efficient model improvement over time

---

**Related Training Architecture Documents:**
* [Training Architecture Overview](./ml-shrinkage-training-architecture-overview.md)
* [Neural Network Architecture Details](./ml-shrinkage-training-architecture-network.md)
* [Ensemble Model Architecture](./ml-shrinkage-training-architecture-ensemble.md)

**Related Core Documents:**
* [ML-Enhanced Shrinkage: Architecture](./ml-shrinkage-architecture.md)
* [ML-Enhanced Shrinkage: Model Training](./ml-shrinkage-training.md)