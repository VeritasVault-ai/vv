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

# Maximum Diversification Portfolio

> Optimizing Diversification Benefits

---

## Overview

The Maximum Diversification approach constructs portfolios that maximize the ratio of weighted average asset volatility to portfolio volatility. This captures the essence of diversification: achieving a portfolio volatility that is significantly lower than the weighted average of individual asset volatilities.

## Core Principles

### Diversification Ratio

The key metric is the diversification ratio (DR):

DR = (w'σ) / √(w'Σw)

Where:
* w = Vector of asset weights
* σ = Vector of asset volatilities (standard deviations)
* Σ = Covariance matrix of returns

A higher DR indicates greater diversification benefits.

### Mathematical Intuition

* When assets are perfectly correlated, DR = 1 (no diversification benefit)
* When assets are less correlated, DR > 1 (diversification benefit)
* Maximum DR portfolio represents the allocation that extracts maximum diversification

## Mathematical Framework

### Optimization Problem

The maximum diversification portfolio solves:

* **Maximize** DR = (w'σ) / √(w'Σw)
* **Subject to** Σw_i = 1, w_i ≥ 0

### Equivalent Formulation

This can be reformulated as:

* **Minimize** w'Σw
* **Subject to** w'σ = constant, Σw_i = 1, w_i ≥ 0

## Implementation

### Calculating Diversification Ratio

```typescript
function calculateDiversificationRatio(
  weights: number[],
  assetVolatilities: number[],
  covarianceMatrix: Matrix
): number {
  // Convert weights to vector
  const weightVector = Vector.fromArray(weights);
  
  // Calculate weighted average of individual volatilities
  const weightedVolSum = weights.reduce(
    (sum, w, i) => sum + w * assetVolatilities[i], 0
  );
  
  // Calculate portfolio variance
  const portfolioVariance = weightVector.transpose()
    .multiply(covarianceMatrix)
    .multiply(weightVector)
    .get(0, 0);
  
  // Calculate portfolio volatility
  const portfolioVolatility = Math.sqrt(portfolioVariance);
  
  // Calculate diversification ratio
  return weightedVolSum / portfolioVolatility;
}
```

### Optimization Solution

```typescript
function solveMaximumDiversificationPortfolio(
  assetVolatilities: number[],
  covarianceMatrix: Matrix,
  constraints: PortfolioConstraint[] = []
): PortfolioAllocation {
  const n = covarianceMatrix.rows;
  
  try {
    // Define objective function: maximize diversification ratio
    // (equivalently, minimize negative of diversification ratio)
    const objectiveFunction = (weights: number[]) => {
      return -calculateDiversificationRatio(
        weights, 
        assetVolatilities, 
        covarianceMatrix
      );
    };
    
    // Define gradient function for faster convergence
    const gradientFunction = (weights: number[]) => {
      // Numerical or analytical gradient calculation
      return calculateDiversificationRatioGradient(
        weights, 
        assetVolatilities, 
        covarianceMatrix
      );
    };
    
    // Initial guess: inverse volatility weights
    const initialGuess = assetVolatilities.map(vol => 1/vol);
    const sumInitial = initialGuess.reduce((sum, w) => sum + w, 0);
    const normalizedGuess = initialGuess.map(w => w / sumInitial);
    
    // Standard constraints
    const standardConstraints = [
      // Sum of weights equals 1
      {
        type: ConstraintType.EQUALITY,
        function: (weights: number[]) => 
          weights.reduce((sum, w) => sum + w, 0) - 1
      }
    ];
    
    // Combine with user constraints
    const allConstraints = [...standardConstraints, ...constraints];
    
    // Solve using nonlinear optimization
    const solution = solveNonlinearConstrainedOptimization(
      objectiveFunction,
      gradientFunction,
      normalizedGuess,
      allConstraints
    );
    
    // Calculate portfolio metrics
    const diversificationRatio = calculateDiversificationRatio(
      solution.weights, 
      assetVolatilities, 
      covarianceMatrix
    );
    
    const riskMetrics = calculatePortfolioRiskMetrics(
      solution.weights, 
      covarianceMatrix
    );
    
    return {
      weights: solution.weights,
      diversificationRatio: diversificationRatio,
      variance: riskMetrics.variance,
      volatility: riskMetrics.volatility,
      riskContributions: riskMetrics.riskContributions,
      success: true
    };
  } catch (error) {
    return {
      success: false,
      error: error.message,
      solverDetails: error.details
    };
  }
}
```

## Alternative Formulation

### Correlation-Based Approach

For assets with identical volatilities, maximum diversification is equivalent to minimum variance. This leads to an alternative formulation:

```typescript
function solveMaximumDiversificationAlternative(
  assetVolatilities: number[],
  covarianceMatrix: Matrix,
  constraints: PortfolioConstraint[] = []
): PortfolioAllocation {
  const n = covarianceMatrix.rows;
  
  // Create diagonal matrix of volatilities
  const D = Matrix.diagonal(assetVolatilities);
  
  // Calculate correlation matrix
  const correlationMatrix = D.inverse().multiply(covarianceMatrix).multiply(D.inverse());
  
  // Adjust weights to account for volatilities
  const transformedConstraints = transformConstraintsForMDP(
    constraints, 
    assetVolatilities
  );
  
  // Solve minimum variance with correlation matrix
  const mvpSolution = solveMinimumVariancePortfolio(
    correlationMatrix, 
    transformedConstraints
  );
  
  // Transform weights back
  const mdpWeights = new Array(n);
  for (let i = 0; i < n; i++) {
    mdpWeights[i] = mvpSolution.weights[i] * assetVolatilities[i];
  }
  
  // Normalize weights to sum to 1
  const weightSum = mdpWeights.reduce((sum, w) => sum + w, 0);
  const normalizedWeights = mdpWeights.map(w => w / weightSum);
  
  // Calculate diversification ratio and other metrics
  // ...
  
  return {
    weights: normalizedWeights,
    // Other metrics...
    success: true
  };
}
```

## Portfolio Characteristics

### Typical Features

Maximum diversification portfolios often exhibit these characteristics:

1. **High Diversification Benefit**: Maximum possible diversification ratio
2. **Allocation to Uncorrelated Assets**: Favors assets with low correlations
3. **Volatility Weighting**: Considers both correlations and individual volatilities
4. **Concentration in Low-Correlation Assets**: May concentrate in assets with unique risk profiles
5. **Sensitivity to Correlation Changes**: Performance depends on stability of correlation estimates

### Performance Metrics

```typescript
interface MaximumDiversificationMetrics {
  diversificationRatio: number;      // Primary metric
  effectiveBets: number;            // Number of effective independent bets
  volatilityReduction: number;      // Reduction from weighted average volatility
  concentrationIndex: number;       // Measure of weight concentration
  correlationSensitivity: number;   // Sensitivity to correlation changes
}
```

## Extensions and Variations

### Constrained Maximum Diversification

Common constraint variations:

1. **Sector Constraints**: Prevents excessive sector concentration
2. **Geographic Constraints**: Ensures global diversification
3. **Maximum Position Sizes**: Prevents excessive concentration in single assets
4. **Factor Exposure Constraints**: Controls exposure to systematic factors
5. **Turnover Constraints**: Limits portfolio changes

### Risk-Adjusted Maximum Diversification

Incorporating return expectations:

```typescript
function solveRiskAdjustedMaximumDiversification(
  expectedReturns: number[],
  assetVolatilities: number[],
  covarianceMatrix: Matrix,
  riskReturnTradeoff: number,
  constraints: PortfolioConstraint[] = []
): PortfolioAllocation {
  try {
    // Define objective function: maximize combination of DR and expected return
    const objectiveFunction = (weights: number[]) => {
      const dr = calculateDiversificationRatio(
        weights, 
        assetVolatilities, 
        covarianceMatrix
      );
      
      const expectedReturn = weights.reduce(
        (sum, w, i) => sum + w * expectedReturns[i], 0
      );
      
      // Combine with trade-off parameter
      return -(dr + riskReturnTradeoff * expectedReturn);
    };
    
    // Continue with optimization similar to standard maximum diversification
    // ...
    
    return {
      weights: solution.weights,
      diversificationRatio: diversificationRatio,
      expectedReturn: expectedReturn,
      // Other metrics...
      success: true
    };
  } catch (error) {
    return {
      success: false,
      error: error.message
    };
  }
}
```

## Practical Considerations

### Correlation Estimation

The approach is sensitive to correlation estimation:

```typescript
function estimateRobustCorrelationForMDP(
  returns: Matrix,
  method: CorrelationEstimationMethod,
  parameters: CorrelationEstimationParameters
): Matrix {
  switch (method) {
    case CorrelationEstimationMethod.SAMPLE:
      return calculateSampleCorrelation(returns);
      
    case CorrelationEstimationMethod.SHRINKAGE:
      return calculateShrinkageCorrelation(returns, parameters.shrinkageTarget, parameters.shrinkageIntensity);
      
    case CorrelationEstimationMethod.FACTOR_BASED:
      return calculateFactorModelCorrelation(returns, parameters.factors);
      
    case CorrelationEstimationMethod.DETONED:
      return calculateDetonedCorrelation(returns, parameters.eigenvalueCutoff);
      
    default:
      throw new Error(`Unsupported correlation estimation method: ${method}`);
  }
}
```

### Implementation Challenges

Key challenges in implementing maximum diversification:

1. **Estimation Error**: Sensitive to correlation estimation errors
2. **Numerical Stability**: Non-convex optimization can be unstable
3. **Concentration Risk**: May concentrate in assets with estimation errors
4. **Correlation Stability**: Performance depends on correlation stability
5. **Trading Costs**: May require significant rebalancing as correlations change

### Monitoring and Rebalancing

Special considerations for monitoring:

```typescript
function monitorDiversificationEffectiveness(
  portfolio: Portfolio,
  currentCorrelations: Matrix,
  assetVolatilities: number[]
): MonitoringResult {
  // Calculate current diversification ratio
  const currentDR = calculateDiversificationRatio(
    portfolio.weights, 
    assetVolatilities, 
    currentCorrelations
  );
  
  // Compare with original DR from portfolio construction
  const drDegradation = (portfolio.originalDR - currentDR) / portfolio.originalDR;
  
  // Check if rebalancing threshold is crossed
  const needsRebalancing = drDegradation > portfolio.rebalancingThreshold;
  
  return {
    currentDiversificationRatio: currentDR,
    diversificationDegradation: drDegradation,
    needsRebalancing: needsRebalancing
  };
}
```