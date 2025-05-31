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

# ML-Enhanced Shrinkage: Neural Network Architecture

> Detailed specification of the neural network design for ML-Enhanced Shrinkage models

---

## 1. Network Architecture Design

The neural network architecture for ML-Enhanced Shrinkage models is designed to effectively learn optimal shrinkage parameters from financial data while maintaining interpretability and robustness.

### Network Topology

```
┌─────────────────┐
│ Input Layer     │
│ (Features)      │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Hidden Layer 1  │
│ (Representation)│
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Hidden Layer 2  │
│ (Context)       │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Output Layer    │
│ (Shrinkage      │
│  Parameters)    │
└─────────────────┘
```

The network consists of carefully designed layers that transform input features into optimal shrinkage parameters:

1. **Input Layer**:
   * Dimension: Varies based on feature extraction configuration
   * Handles matrix-derived features, temporal indicators, and market context
   * Applies appropriate normalization for numerical stability

2. **Hidden Layers**:
   * Multiple dense layers with non-linear activations
   * Decreasing dimensionality through the network
   * Skip connections to preserve low-level feature information

3. **Output Layer**:
   * Dimension: Number of shrinkage targets
   * Sigmoid activation to constrain outputs to [0,1] range
   * Optional softmax for target weighting when using multiple targets

## 2. Layer Specifications

### Input Layer

The input layer is designed to efficiently handle matrix-derived features:

* **Feature Dimensionality**: Varies based on asset count and feature extraction configuration
* **Normalization**: Batch normalization with running statistics for production
* **Dropout**: Input dropout (10-20%) for regularization
* **Embedding**: Optional categorical feature embedding for market regimes

### Hidden Layers

The hidden layers implement the following architecture:

* **Layer 1** (Representation Layer):
  * Units: 128-256 (configurable)
  * Activation: LeakyReLU (alpha=0.2)
  * Regularization: L2 weight regularization (lambda=0.001)
  * Batch Normalization: Applied post-activation

* **Layer 2** (Context Integration Layer):
  * Units: 64-128 (configurable)
  * Activation: LeakyReLU (alpha=0.2)
  * Dropout: 30% for regularization
  * Skip Connection: Residual connection from Layer 1

* **Layer 3** (Decision Layer):
  * Units: 32-64 (configurable)
  * Activation: LeakyReLU (alpha=0.2)
  * Attention Mechanism: Optional self-attention for feature importance

### Output Layer

The output layer is designed for interpretable shrinkage parameters:

* **Units**: Equal to number of shrinkage targets
* **Activation**: Sigmoid for individual intensity values
* **Constraints**: Non-negativity and upper bounds
* **Optional Auxiliary Outputs**:
  * Uncertainty estimates (prediction variance)
  * Target weights for multi-target models

## 3. Activation Functions

The network uses carefully selected activation functions:

* **Hidden Layers**: LeakyReLU
  * Prevents dead neurons
  * Allows for negative values
  * Provides non-saturation for gradient flow

* **Output Layer**: Sigmoid/Specialized
  * Constrains outputs to valid parameter ranges
  * Smooth gradients near boundaries
  * Specialized variants for specific parameter types

## 4. Initialization Strategy

Model weights are initialized using strategies optimized for financial data:

* **Hidden Layers**: He initialization (accounting for LeakyReLU)
* **Output Layer**: Glorot uniform initialization with bias towards empirical priors
* **Custom Initialization**: Pre-trained values from synthetic data when available

## 5. Network Size Considerations

The network size is calibrated based on these considerations:

* **Asset Universe**: Scales with sqrt(number of assets) to balance complexity
* **Data Availability**: Smaller networks for limited training data
* **Computational Constraints**: Optimized for inference performance
* **Training Stability**: Sufficient capacity without overfitting

## 6. Implementation Details

### PyTorch Implementation

```python
class MLShrinkageNetwork(nn.Module):
    def __init__(self, input_dim, hidden_dims=[128, 64, 32], 
                 num_targets=1, dropout_rate=0.3):
        super(MLShrinkageNetwork, self).__init__()
        
        # Input layer
        layers = [
            nn.Linear(input_dim, hidden_dims[0]),
            nn.BatchNorm1d(hidden_dims[0]),
            nn.LeakyReLU(0.2),
            nn.Dropout(dropout_rate)
        ]
        
        # Hidden layers with skip connections
        for i in range(1, len(hidden_dims)):
            layers.extend([
                nn.Linear(hidden_dims[i-1], hidden_dims[i]),
                nn.BatchNorm1d(hidden_dims[i]),
                nn.LeakyReLU(0.2),
                nn.Dropout(dropout_rate if i < len(hidden_dims)-1 else 0)
            ])
        
        self.feature_extractor = nn.Sequential(*layers)
        
        # Output layer for shrinkage intensities
        self.shrinkage_predictor = nn.Linear(hidden_dims[-1], num_targets)
        self.shrinkage_activation = nn.Sigmoid()
        
    def forward(self, x):
        features = self.feature_extractor(x)
        shrinkage = self.shrinkage_activation(self.shrinkage_predictor(features))
        return shrinkage
```

### TensorFlow Implementation

```python
def create_ml_shrinkage_model(input_dim, hidden_dims=[128, 64, 32], 
                             num_targets=1, dropout_rate=0.3):
    inputs = tf.keras.Input(shape=(input_dim,))
    
    # Input layer
    x = tf.keras.layers.Dense(hidden_dims[0])(inputs)
    x = tf.keras.layers.BatchNormalization()(x)
    x = tf.keras.layers.LeakyReLU(0.2)(x)
    x = tf.keras.layers.Dropout(dropout_rate)(x)
    
    # Hidden layers
    for i in range(1, len(hidden_dims)):
        x_prev = x
        x = tf.keras.layers.Dense(hidden_dims[i])(x)
        x = tf.keras.layers.BatchNormalization()(x)
        x = tf.keras.layers.LeakyReLU(0.2)(x)
        
        # Add skip connection if shapes match
        if hidden_dims[i] == hidden_dims[i-1]:
            x = tf.keras.layers.Add()([x, x_prev])
            
        if i < len(hidden_dims)-1:
            x = tf.keras.layers.Dropout(dropout_rate)(x)
    
    # Output layer
    outputs = tf.keras.layers.Dense(num_targets, 
                                   activation='sigmoid')(x)
    
    model = tf.keras.Model(inputs=inputs, outputs=outputs)
    return model
```

---

**Related Training Architecture Documents:**
* [Training Architecture Overview](./ml-shrinkage-training-architecture-overview.md)
* [Ensemble Model Architecture](./ml-shrinkage-training-architecture-ensemble.md)
* [Uncertainty Estimation Architecture](./ml-shrinkage-training-architecture-uncertainty.md)

**Related Core Documents:**
* [ML-Enhanced Shrinkage: Architecture](./ml-shrinkage-architecture.md)
* [ML-Enhanced Shrinkage: Model Training](./ml-shrinkage-training.md)