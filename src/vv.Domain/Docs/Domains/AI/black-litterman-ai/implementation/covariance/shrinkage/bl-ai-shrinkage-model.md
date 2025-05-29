# ML-Enhanced Shrinkage Estimation: Model Architecture

> Neural network architecture design for ML-enhanced covariance shrinkage

---

## 1. Architectural Overview

### High-Level Design

The ML-Enhanced Shrinkage model employs a specialized neural network architecture designed to estimate optimal shrinkage parameters from market data. The architecture balances expressiveness with financial domain knowledge to ensure both performance and generalization.

```
┌─────────────────┐     ┌─────────────────┐     ┌─────────────────┐     ┌─────────────────┐
│                 │     │                 │     │                 │     │                 │
│  Feature        │     │  Core Shrinkage │     │  Target-Specific│     │  Shrinkage      │
│  Extraction     │────▶│  Network        │────▶│  Heads          │────▶│  Parameter      │
│  Layers         │     │                 │     │                 │     │  Outputs        │
│                 │     │                 │     │                 │     │                 │
└─────────────────┘     └─────────────────┘     └─────────────────┘     └─────────────────┘
```

### Design Principles

The architecture follows these key principles:

1. **Structured Inductive Bias**: Incorporates financial domain knowledge into the network structure
2. **Ensemble Approach**: Combines multiple specialized sub-networks for robustness
3. **Residual Connections**: Allows learning deviations from established shrinkage methods
4. **Attention Mechanisms**: Focuses on relevant market features for different conditions
5. **Regularization Techniques**: Prevents overfitting to market noise
6. **Scale Invariance**: Design ensures performance across different asset universe sizes

## 2. Input Features and Preprocessing

### Feature Categories

The network accepts three categories of input features:

1. **Covariance Matrix Features**:
   - Eigenvalue spectrum statistics
   - Condition number
   - Average correlation
   - Correlation dispersion measures
   - Matrix rank estimators

2. **Market Regime Indicators**:
   - Volatility measures (VIX, realized vol)
   - Liquidity metrics
   - Trading volume statistics
   - Cross-sectional dispersion
   - Market stress indicators

3. **Temporal Context**:
   - Time series stability metrics
   - Trend indicators
   - Seasonality features
   - Event proximity features

### Feature Preprocessing

Input features undergo several preprocessing steps:

1. **Normalization**:
   - Robust scaling using median and interquartile range
   - Temporal normalization with expanding windows
   - Rank transformation for non-Gaussian features

2. **Dimensionality Reduction**:
   - Covariance matrix compression using random projections
   - Principal component analysis for high-dimensional features
   - Feature selection based on importance metrics

3. **Missing Data Handling**:
   - Forward filling for time series
   - Median imputation for cross-sectional features
   - Indicator variables for missing patterns

## 3. Network Layer Architecture

### Feature Extraction Layers

```
┌─────────────────┐
│ Input Features  │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ BatchNorm Layer │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Dense Layer     │
│ (128 units)     │
│ + ELU           │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Dropout (0.3)   │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Dense Layer     │
│ (64 units)      │
│ + ELU           │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Feature         │
│ Representation  │
└─────────────────┘
```

### Core Shrinkage Network

```
┌─────────────────────────┐
│ Feature Representation  │
└────────────┬────────────┘
             │
             ▼
┌─────────────────────────┐
│ Self-Attention Layer    │
└────────────┬────────────┘
             │
             ▼
┌─────────────────────────┐
│ Residual Block 1        │
│ ┌───────────────────┐   │
│ │ Dense (64 units)  │   │
│ │ + BatchNorm + ELU │   │
│ └──────────┬────────┘   │
│            │            │
│ ┌──────────▼────────┐   │
│ │ Dense (64 units)  │   │
│ │ + BatchNorm       │   │
│ └──────────┬────────┘   │
│            │            │
│ ┌──────────▼────────┐   │
│ │ Add Input         │   │
│ └──────────┬────────┘   │
│            │            │
│ ┌──────────▼────────┐   │
│ │ ELU Activation    │   │
│ └───────────────────┘   │
└────────────┬────────────┘
             │
             ▼
┌─────────────────────────┐
│ Residual Block 2        │
│ (Same structure as 1)   │
└────────────┬────────────┘
             │
             ▼
┌─────────────────────────┐
│ Shared Representation   │
└─────────────────────────┘
```

### Target-Specific Heads

The network includes specialized heads for different shrinkage targets:

```
                 ┌───────────────────┐
                 │ Shared            │
                 │ Representation    │
                 └─────┬──────┬──────┘
                       │      │      │
           ┌───────────┘      │      └────────────┐
           │                  │                   │
┌──────────▼───────┐ ┌────────▼────────┐ ┌────────▼────────┐
│ Identity Target  │ │ Constant Corr   │ │ Single Factor   │
│ Head             │ │ Target Head     │ │ Target Head     │
└──────────┬───────┘ └────────┬────────┘ └────────┬────────┘
           │                  │                   │
           │                  │                   │
┌──────────▼───────┐ ┌────────▼────────┐ ┌────────▼────────┐
│ Dense (32 units) │ │ Dense (32 units)│ │ Dense (32 units)│
│ + ELU            │ │ + ELU           │ │ + ELU           │
└──────────┬───────┘ └────────┬────────┘ └────────┬────────┘
           │                  │                   │
           │                  │                   │
┌──────────▼───────┐ ┌────────▼────────┐ ┌────────▼────────┐
│ Dense (16 units) │ │ Dense (16 units)│ │ Dense (16 units)│
│ + ELU            │ │ + ELU           │ │ + ELU           │
└──────────┬───────┘ └────────┬────────┘ └────────┬────────┘
           │                  │                   │
           │                  │                   │
┌──────────▼───────┐ ┌────────▼────────┐ ┌────────▼────────┐
│ Output Layer     │ │ Output Layer    │ │ Output Layer    │
│ (Sigmoid)        │ │ (Sigmoid)       │ │ (Sigmoid)       │
└──────────────────┘ └─────────────────┘ └─────────────────┘
```

### Output Layer

The final output layer produces:

1. **Shrinkage Intensity Parameters**:
   - Global shrinkage parameter (δ)
   - Target-specific shrinkage weights
   - Block-specific shrinkage parameters (optional)

2. **Target Selection Weights**:
   - Weighting for each shrinkage target
   - Confidence scores for each target

3. **Auxiliary Outputs**:
   - Covariance matrix condition estimate
   - Expected estimation error
   - Regime classification probabilities

## 4. Special Architectural Components

### Regime-Aware Attention Module

```
┌────────────────────┐
│ Feature Vectors    │
└──────────┬─────────┘
           │
           ▼
┌────────────────────┐
│ Query Projection   │
└──────────┬─────────┘
           │
           ▼
┌────────────────────┐    ┌────────────────────┐    ┌────────────────────┐
│ Key Projection     │    │ Value Projection   │    │ Regime Embedding   │
└──────────┬─────────┘    └──────────┬─────────┘    └──────────┬─────────┘
           │                         │                         │
           └─────────┬───────────────┘                         │
                     │                                         │
                     ▼                                         │
┌────────────────────────────────────────┐                     │
│ Scaled Dot-Product Attention           │                     │
└──────────────────────┬─────────────────┘                     │
                       │                                       │
                       └───────────────────┬───────────────────┘
                                           │
                                           ▼
                              ┌────────────────────────┐
                              │ Context-Aware Features │
                              └────────────────────────┘
```

### Uncertainty Estimation Module

The model includes an uncertainty estimation branch:

```
┌────────────────────┐
│ Core Representation│
└──────────┬─────────┘
           │
           ▼
┌────────────────────┐
│ Uncertainty Head   │
└──────────┬─────────┘
           │
           ▼
┌────────────────────┐
│ Aleatoric          │
│ Uncertainty        │
└────────────────────┘
```

### Ensemble Integration

The architecture employs model ensembling:

```
┌─────────────┐  ┌─────────────┐  ┌─────────────┐
│ Model       │  │ Model       │  │ Model       │
│ Instance 1  │  │ Instance 2  │  │ Instance 3  │
└──────┬──────┘  └──────┬──────┘  └──────┬──────┘
       │                │                │
       └────────┬───────┘                │
                │                        │
        ┌───────▼────────┐               │
        │ Weighted Avg   │◄──────────────┘
        └───────┬────────┘
                │
                ▼
       ┌─────────────────┐
       │ Final Shrinkage │
       │ Parameters      │
       └─────────────────┘
```

## 5. Hyperparameters

### Network Size Hyperparameters

| Component                  | Parameter                   | Value Range         | Default |
|----------------------------|-----------------------------|--------------------|---------|
| Feature Extraction Layers  | Number of layers            | 2-4                | 2       |
|                            | Units per layer             | 64-256             | 128, 64 |
| Core Network               | Number of residual blocks   | 2-4                | 2       |
|                            | Units per block             | 32-128             | 64      |
| Target-Specific Heads      | Number of layers            | 2-3                | 2       |
|                            | Units per layer             | 16-64              | 32, 16  |
| Attention Module           | Number of attention heads   | 1-4                | 2       |
|                            | Attention dimension         | 16-64              | 32      |

### Training Hyperparameters

| Parameter                  | Value Range                | Default  |
|----------------------------|----------------------------|----------|
| Dropout rate               | 0.1-0.5                   | 0.3      |
| L2 regularization          | 1e-5 - 1e-3               | 1e-4     |
| Learning rate              | 1e-4 - 1e-2               | 5e-4     |
| Batch size                 | 16-128                    | 32       |
| Learning rate schedule     | Step/Cosine/Exponential   | Cosine   |

## 6. Hardware Acceleration

The architecture is optimized for:

* **GPU Acceleration**: Tensor operations parallelized for GPU computation
* **Multi-GPU Training**: Support for distributed training across multiple GPUs
* **Inference Optimization**: TensorRT/ONNX compatibility for production deployment
* **Quantization Support**: Int8/FP16 quantization for efficient inference

---

**Related Documents:**
* [ML-Enhanced Shrinkage: Overview & Theory](./bl-ai-shrinkage-overview.md)
* [ML-Enhanced Shrinkage: Training Methodology](./bl-ai-shrinkage-training.md)
* [ML-Enhanced Shrinkage: Code Implementation](./bl-ai-shrinkage-code.md)
* [ML-Enhanced Shrinkage: Integration Guide](./bl-ai-shrinkage-integration.md)