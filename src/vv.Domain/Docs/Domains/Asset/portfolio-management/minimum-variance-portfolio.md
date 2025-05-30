# Minimum Variance Portfolio

> Constructing the Lowest-Volatility Portfolio

---

## Overview

The Minimum Variance Portfolio (MVP) represents the allocation that minimizes overall portfolio volatility. This approach focuses purely on risk minimization without consideration of expected returns, making it useful when return forecasts are unreliable or when risk reduction is the primary objective.

## Mathematical Framework

### Core Concept

The minimum variance portfolio solves the optimization problem:

* **Minimize** σₚ² = w'Σw
* **Subject to** Σwᵢ = 1 (and typically wᵢ ≥ 0)

Where:
* w = Vector of asset weights
* Σ = Covariance matrix of returns

### Analytical Solution

For the unconstrained case (allowing negative weights), the minimum variance portfolio has a closed-form solution:

```typescript
function calculateMinimumVarianceWeights(
  covarianceMatrix: Matrix
): number[] {
  const n = covarianceMatrix.rows;
  
  // Create a vector of ones
  const ones = Vector.ones(n);
  
  // Calculate inverse of covariance matrix
  const covInverse = covarianceMatrix.inverse();
  
  // Calculate weights: w = (Σ⁻¹1)/(1'Σ⁻¹1)
  const numerator = covInverse.multiply(ones);
  const denominator = ones.transpose().multiply(numerator).get(0, 0);
  
  // Normalize weights
  return numerator.scalarMultiply(1/denominator).toArray();
}
```

## Implementation

### Optimization Formulation

When constraints are added (like no short-selling), we need to solve numerically:

```typescript
function formulateMinimumVarianceProblem(
  covarianceMatrix: Matrix,
  constraints: PortfolioConstraint[]
): OptimizationProblem {
  const n = covarianceMatrix.rows;
  
  // Create variables for asset weights
  const variables = createVariables(n);
  
  // Define objective function: minimize w'Σw
  const objectiveFunction = createQuadraticObjective(variables, covarianceMatrix);
  
  // Add standard constraints
  const standardConstraints = [
    // Sum of weights equals 1
    createEqualityConstraint(variables, Vector.ones(n), 1)
  ];
  
  // Convert user constraints to appropriate format
  const userConstraints = formatConstraints(constraints, variables, n);
  
  return {
    variables,
    objective: {
      type: 'minimize',
      function: objectiveFunction
    },
    constraints: [...standardConstraints, ...userConstraints]
  };
}
```

### Solution Process

```typescript
function solveMinimumVariancePortfolio(
  covarianceMatrix: Matrix,
  constraints: PortfolioConstraint[] = []
): PortfolioAllocation {
  try {
    // Formulate the optimization problem
    const problem = formulateMinimumVarianceProblem(covarianceMatrix, constraints);
    
    // Select appropriate solver based on problem characteristics
    const solver = selectQuadraticProgrammingSolver(problem);
    
    // Solve the optimization problem
    const solution = solver.solve(problem);
    
    // Extract weights from solution
    const weights = extractWeights(solution, problem.variables);
    
    // Calculate portfolio metrics
    const metrics = calculatePortfolioRiskMetrics(weights, covarianceMatrix);
    
    return {
      weights,
      variance: metrics.variance,
      volatility: metrics.volatility,
      riskContributions: metrics.riskContributions,
      diversificationRatio: metrics.diversificationRatio,
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

## Portfolio Characteristics

### Typical Features

Minimum variance portfolios often exhibit these characteristics:

1. **Defensive Bias**: Overweight to low-volatility assets
2. **Sector Concentration**: May concentrate in less volatile sectors (utilities, consumer staples)
3. **Size Bias**: Often favors large-cap over small-cap stocks
4. **Low Beta**: Typically has market beta less than 1.0
5. **Drawdown Protection**: Usually outperforms in market downturns
6. **Volatility Reduction**: Significantly lower volatility than cap-weighted indices

### Risk Metrics

```typescript
interface MinimumVarianceMetrics {
  variance: number;                // Portfolio variance
  volatility: number;              // Portfolio standard deviation
  riskContributions: number[];     // Contribution of each asset to risk
  diversificationRatio: number;    // Measure of diversification
  volatilityReduction: number;     // Reduction vs. benchmark
  downmarketCapture: number;       // Performance in down markets
}
```

## Practical Considerations

### Covariance Estimation

The quality of the covariance matrix is crucial:

```typescript
function estimateRobustCovariance(
  returns: Matrix,
  method: CovarianceEstimationMethod
): Matrix {
  switch (method) {
    case CovarianceEstimationMethod.SAMPLE:
      return calculateSampleCovariance(returns);
      
    case CovarianceEstimationMethod.SHRINKAGE:
      return calculateShrinkageCovariance(returns);
      
    case CovarianceEstimationMethod.FACTOR_BASED:
      return calculateFactorCovariance(returns);
      
    case CovarianceEstimationMethod.EXPONENTIAL_WEIGHTING:
      return calculateExponentialWeightedCovariance(returns);
      
    default:
      throw new Error(`Unsupported covariance estimation method: ${method}`);
  }
}
```

### Constraint Importance

Key constraints to consider:

1. **Position Limits**: Prevent excessive concentration
2. **Sector/Country Constraints**: Ensure adequate diversification
3. **Turnover Constraints**: Limit portfolio changes
4. **Factor Exposure**: Control unintended factor tilts
5. **Liquidity Requirements**: Ensure adequate liquidity

### Rebalancing Approach

Optimal rebalancing for minimum variance portfolios:

```typescript
function determineRebalancingNeed(
  currentWeights: number[],
  optimalWeights: number[],
  rebalancingParams: RebalancingParameters
): boolean {
  // Calculate portfolio drift
  const drift = calculateWeightDrift(currentWeights, optimalWeights);
  
  // Calculate risk impact of drift
  const riskImpact = calculateRiskImpact(
    currentWeights, 
    optimalWeights, 
    covarianceMatrix
  );
  
  // Determine if rebalancing is needed based on thresholds
  return drift > rebalancingParams.maxDrift || 
         riskImpact > rebalancingParams.maxRiskImpact;
}
```

## Extensions and Variations

### Regularized Minimum Variance

Addresses estimation error through regularization:

```typescript
function solveRegularizedMinimumVariance(
  covarianceMatrix: Matrix,
  regularizationParameter: number,
  constraints: PortfolioConstraint[] = []
): PortfolioAllocation {
  // Add regularization term to covariance matrix
  const regularized = covarianceMatrix.clone();
  
  // Add to diagonal elements
  for (let i = 0; i < regularized.rows; i++) {
    regularized.set(
      i, 
      i, 
      regularized.get(i, i) + regularizationParameter
    );
  }
  
  // Solve using regularized covariance
  return solveMinimumVariancePortfolio(regularized, constraints);
}
```

### Constrained Minimum Variance

Common constraint variations:

1. **Beta-Neutral**: Constrains portfolio beta to zero
2. **Sector-Neutral**: Constrains sector weights to match benchmark
3. **Factor-Constrained**: Limits exposure to specific factors
4. **Tracking Error Constrained**: Limits deviation from benchmark