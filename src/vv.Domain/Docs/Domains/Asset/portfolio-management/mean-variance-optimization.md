---
document_type: portfolio-optimization-guide
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

# Mean-Variance Optimization

> Classic Portfolio Optimization Framework

---

## Overview

Mean-Variance Optimization (MVO) is the foundation of Modern Portfolio Theory, allowing investors to construct portfolios that maximize expected return for a given level of risk. This document details the implementation of MVO in VeritasVault.

## Mathematical Framework

### Core Concept

The mean-variance framework represents the portfolio optimization problem as:

* **Maximize** Expected Return (μₚ = w'μ)
* **Subject to** Risk Constraint (σₚ² = w'Σw ≤ target)
* **And** Weight Constraints (e.g., Σwᵢ = 1, wᵢ ≥ 0)

Where:
* w = Vector of asset weights
* μ = Vector of expected returns
* Σ = Covariance matrix of returns

### Efficient Frontier

The efficient frontier represents the set of optimal portfolios:

```typescript
interface EfficientFrontierPoint {
  weights: number[];         // Asset allocation weights
  expectedReturn: number;    // Portfolio expected return
  risk: number;              // Portfolio standard deviation
  sharpeRatio: number;       // Risk-adjusted return
  otherMetrics: Map<string, number>; // Additional portfolio metrics
}

interface EfficientFrontier {
  points: EfficientFrontierPoint[];  // Set of optimal portfolios
  minimumVariancePoint: number;      // Index of minimum variance portfolio
  maximumReturnPoint: number;        // Index of maximum return portfolio
  optimalPoint: number;              // Index of optimal portfolio for given preferences
}
```

## Implementation

### Optimization Problem Formulation

```typescript
function formulateMeanVarianceOptimization(
  expectedReturns: number[],
  covarianceMatrix: number[][],
  constraints: OptimizationConstraint[],
  objective: OptimizationObjective,
  riskAversion?: number
): OptimizationProblem {
  const n = expectedReturns.length;
  
  // Create variables for asset weights
  const variables = createVariables(n);
  
  // Define objective function based on selected objective
  let objectiveFunction;
  
  switch (objective) {
    case OptimizationObjective.MAXIMIZE_RETURN:
      objectiveFunction = createReturnObjective(variables, expectedReturns);
      break;
      
    case OptimizationObjective.MINIMIZE_RISK:
      objectiveFunction = createRiskObjective(variables, covarianceMatrix);
      break;
      
    case OptimizationObjective.MAXIMIZE_SHARPE_RATIO:
      objectiveFunction = createSharpeObjective(variables, expectedReturns, covarianceMatrix, riskFreeRate);
      break;
      
    case OptimizationObjective.MAXIMIZE_UTILITY:
      objectiveFunction = createUtilityObjective(variables, expectedReturns, covarianceMatrix, riskAversion);
      break;
      
    default:
      throw new Error(`Unsupported optimization objective: ${objective}`);
  }
  
  // Convert constraints to appropriate format
  const formattedConstraints = formatConstraints(constraints, variables, n);
  
  return {
    variables,
    objective: objectiveFunction,
    constraints: formattedConstraints
  };
}
```

### Optimization Solver

```typescript
function solveMeanVarianceOptimization(
  problem: OptimizationProblem
): OptimizationResult {
  try {
    // Choose appropriate solver based on problem characteristics
    const solver = selectSolver(problem);
    
    // Solve the optimization problem
    const solution = solver.solve(problem);
    
    // Extract weights from solution
    const weights = extractWeights(solution, problem.variables);
    
    // Calculate portfolio metrics
    const metrics = calculatePortfolioMetrics(
      weights,
      expectedReturns,
      covarianceMatrix
    );
    
    return {
      success: true,
      weights,
      metrics,
      solverDetails: solution.details
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

## Efficient Frontier Generation

```typescript
function generateEfficientFrontier(
  expectedReturns: number[],
  covarianceMatrix: number[][],
  constraints: OptimizationConstraint[],
  points: number
): EfficientFrontier {
  // Find minimum variance portfolio
  const minVarianceResult = solveMeanVarianceOptimization({
    expectedReturns,
    covarianceMatrix,
    constraints,
    objective: OptimizationObjective.MINIMIZE_RISK
  });
  
  // Find maximum return portfolio
  const maxReturnResult = solveMeanVarianceOptimization({
    expectedReturns,
    covarianceMatrix,
    constraints,
    objective: OptimizationObjective.MAXIMIZE_RETURN
  });
  
  // Calculate return range
  const minReturn = calculatePortfolioReturn(minVarianceResult.weights, expectedReturns);
  const maxReturn = calculatePortfolioReturn(maxReturnResult.weights, expectedReturns);
  const returnStep = (maxReturn - minReturn) / (points - 1);
  
  // Generate frontier points
  const frontierPoints: EfficientFrontierPoint[] = [];
  
  for (let i = 0; i < points; i++) {
    const targetReturn = minReturn + i * returnStep;
    
    // Add return constraint
    const returnConstraint: OptimizationConstraint = {
      type: ConstraintType.EQUALITY,
      parameter: "return",
      value: targetReturn
    };
    
    // Solve for minimum risk at this return level
    const result = solveMeanVarianceOptimization({
      expectedReturns,
      covarianceMatrix,
      constraints: [...constraints, returnConstraint],
      objective: OptimizationObjective.MINIMIZE_RISK
    });
    
    if (result.success) {
      frontierPoints.push({
        weights: result.weights,
        expectedReturn: targetReturn,
        risk: result.metrics.risk,
        sharpeRatio: result.metrics.sharpeRatio,
        otherMetrics: result.metrics.other
      });
    }
  }
  
  // Find optimal point based on maximum Sharpe ratio
  const optimalIndex = findMaxSharpeRatioIndex(frontierPoints);
  
  return {
    points: frontierPoints,
    minimumVariancePoint: 0,
    maximumReturnPoint: points - 1,
    optimalPoint: optimalIndex
  };
}
```

## Challenges and Solutions

### Input Sensitivity

MVO is highly sensitive to input estimates. Solutions include:

1. **Robust Optimization**: Explicitly accounts for estimation errors
2. **Resampling**: Generates multiple frontiers through resampling
3. **Shrinkage Estimators**: Improves covariance estimates
4. **Black-Litterman Integration**: Combines with market equilibrium

Implementation example:

```typescript
function applyRobustOptimization(
  expectedReturns: number[],
  covarianceMatrix: number[][],
  uncertaintyParameters: UncertaintyParameters
): RobustOptimizationResult {
  // Generate perturbed inputs based on uncertainty
  const perturbedInputs = generatePerturbedInputs(
    expectedReturns,
    covarianceMatrix,
    uncertaintyParameters
  );
  
  // Solve worst-case optimization
  return solveWorstCaseOptimization(perturbedInputs);
}
```

### Corner Solutions

MVO often produces extreme allocations. Solutions include:

1. **Position Constraints**: Minimum/maximum position sizes
2. **Regularization**: Penalizing extreme weights
3. **Resampling**: Averaging multiple optimized portfolios
4. **Risk Factor Constraints**: Limiting exposure to specific factors

Implementation example:

```typescript
function applyRegularization(
  objective: ObjectiveFunction,
  regularizationParameter: number
): ObjectiveFunction {
  // Add L2 regularization term to objective
  return function(weights) {
    const originalValue = objective(weights);
    const regularizationTerm = regularizationParameter * 
      weights.reduce((sum, w) => sum + w * w, 0);
    
    return originalValue + regularizationTerm;
  };
}
```

## Performance Considerations

### Computational Efficiency

Optimizations for efficient computation:

1. **Matrix Decomposition**: Using Cholesky decomposition for covariance
2. **Sparse Matrix Handling**: Efficient storage for large matrices
3. **Problem Reformulation**: Quadratic to conic transformation
4. **Warm Starting**: Using previous solutions as starting points
5. **Hierarchical Optimization**: Breaking large problems into smaller ones

Example implementation:

```typescript
function optimizeComputation(
  covarianceMatrix: number[][],
  expectedReturns: number[]
): OptimizedProblemData {
  // Check if covariance matrix is sparse
  if (isSparseMatrix(covarianceMatrix)) {
    return prepareSparseProblem(covarianceMatrix, expectedReturns);
  }
  
  // Perform Cholesky decomposition
  const choleskyFactor = choleskyDecomposition(covarianceMatrix);
  
  // Transform problem using decomposition
  return transformProblemWithCholesky(choleskyFactor, expectedReturns);
}
```

## Integration Points

MVO integrates with other components:

1. **Risk Models**: Sources covariance matrices
2. **Return Models**: Sources expected returns
3. **Constraint Engine**: Applies investment constraints
4. **Black-Litterman Model**: Enhances input estimates
5. **Portfolio Rebalancing**: Guides portfolio updates