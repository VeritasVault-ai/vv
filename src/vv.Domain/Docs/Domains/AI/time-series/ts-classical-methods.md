# Classical Time Series Methods

> Traditional statistical approaches for financial time series forecasting

---

## Overview

Classical time series methods provide robust, interpretable, and computationally efficient approaches for modeling and forecasting financial data. This document covers established statistical techniques that form the foundation of time series analysis within the VeritasVault platform.

## ARIMA Family Models

### ARIMA (AutoRegressive Integrated Moving Average)

* **Components:**
  * AR(p): Autoregressive component modeling dependency on past values
  * I(d): Integration order indicating differencing required for stationarity
  * MA(q): Moving average component modeling dependency on past errors

* **Mathematical Representation:**
  * AR(p): X<sub>t</sub> = c + φ<sub>1</sub>X<sub>t-1</sub> + ... + φ<sub>p</sub>X<sub>t-p</sub> + ε<sub>t</sub>
  * MA(q): X<sub>t</sub> = μ + ε<sub>t</sub> + θ<sub>1</sub>ε<sub>t-1</sub> + ... + θ<sub>q</sub>ε<sub>t-q</sub>
  * ARMA(p,q): Combines both components
  * ARIMA(p,d,q): ARMA model on d-times differenced series

* **Model Selection:**
  * ACF/PACF analysis for order identification
  * Information criteria (AIC, BIC, HQIC)
  * Cross-validation approaches
  * Residual diagnostics

* **Financial Applications:**
  * Short-term return forecasting
  * Trend component modeling
  * Mean reversion analysis
  * Pairs trading strategy development

### Seasonal ARIMA (SARIMA)

* **Extended Notation:**
  * SARIMA(p,d,q)(P,D,Q)<sub>s</sub>
  * s: Seasonal period (e.g., 5 for weekly seasonality in daily data)
  * P, D, Q: Seasonal AR, differencing, and MA orders

* **Key Features:**
  * Captures both seasonal and non-seasonal patterns
  * Allows for multiple seasonality modeling
  * Requires identification of appropriate seasonal periods
  * More parameters increase estimation complexity

* **Financial Applications:**
  * Day-of-week effects modeling
  * Month-of-year seasonal patterns
  * Holiday effect capture
  * Trading calendar anomalies

### ARIMAX and Regression with ARIMA Errors

* **External Regressors:**
  * Incorporation of exogenous variables
  * ARIMAX: ARIMA with external regressors
  * RegARIMA: Regression with ARIMA errors

* **Model Formulation:**
  * Y<sub>t</sub> = β<sub>0</sub> + β<sub>1</sub>X<sub>1,t</sub> + ... + β<sub>k</sub>X<sub>k,t</sub> + N<sub>t</sub>
  * N<sub>t</sub> follows ARIMA(p,d,q) process

* **Financial Applications:**
  * Macro-factor enhanced return forecasting
  * Event impact modeling
  * Fundamental data integration
  * Policy response prediction

### Fractionally Integrated ARIMA (ARFIMA)

* **Long Memory Modeling:**
  * Fractional differencing parameter d ∈ (-0.5, 0.5)
  * Captures long-range dependencies
  * Slower decay of autocorrelation function

* **Key Features:**
  * More flexible than standard ARIMA
  * Better suited for persistent financial series
  * Computationally more intensive
  * Requires estimation of fractional parameter

* **Financial Applications:**
  * Volatility persistence modeling
  * Long-horizon return forecasting
  * Risk premium estimation
  * Market efficiency analysis

## GARCH Family Models

### GARCH (Generalized Autoregressive Conditional Heteroskedasticity)

* **Volatility Modeling:**
  * Captures time-varying conditional variance
  * Models volatility clustering
  * Mean equation + variance equation

* **Standard GARCH(p,q) Formulation:**
  * Mean: Y<sub>t</sub> = μ + ε<sub>t</sub>
  * Variance: σ<sub>t</sub><sup>2</sup> = ω + Σα<sub>i</sub>ε<sub>t-i</sub><sup>2</sup> + Σβ<sub>j</sub>σ<sub>t-j</sub><sup>2</sup>
  * ε<sub>t</sub> = σ<sub>t</sub>z<sub>t</sub>, z<sub>t</sub> ~ N(0,1)

* **Estimation Methods:**
  * Maximum likelihood estimation
  * Quasi-maximum likelihood
  * Bayesian approaches
  * Method of moments

* **Financial Applications:**
  * Volatility forecasting
  * Option pricing
  * Risk measurement (VaR, ES)
  * Volatility-based trading strategies

### EGARCH (Exponential GARCH)

* **Asymmetric Volatility:**
  * Captures leverage effect
  * Log transformation ensures positive variance
  * Different response to positive and negative shocks

* **Model Formulation:**
  * log(σ<sub>t</sub><sup>2</sup>) = ω + Σα<sub>i</sub>g(z<sub>t-i</sub>) + Σβ<sub>j</sub>log(σ<sub>t-j</sub><sup>2</sup>)
  * g(z<sub>t</sub>) = θz<sub>t</sub> + γ[|z<sub>t</sub>| - E|z<sub>t</sub>|]

* **Financial Applications:**
  * Asymmetric risk modeling
  * Stress testing
  * Downside risk forecasting
  * Equity market volatility

### GJR-GARCH (Glosten-Jagannathan-Runkle GARCH)

* **Threshold Effects:**
  * Alternative asymmetric volatility model
  * Uses indicator function for negative returns
  * Captures leverage effect through additive terms

* **Model Formulation:**
  * σ<sub>t</sub><sup>2</sup> = ω + Σα<sub>i</sub>ε<sub>t-i</sub><sup>2</sup> + Σγ<sub>i</sub>I<sub>t-i</sub>ε<sub>t-i</sub><sup>2</sup> + Σβ<sub>j</sub>σ<sub>t-j</sub><sup>2</sup>
  * I<sub>t-i</sub> = 1 if ε<sub>t-i</sub> < 0, 0 otherwise

* **Financial Applications:**
  * Market crash risk assessment
  * Equity risk premium modeling
  * Risk parity portfolio construction
  * Hedging strategy development

### Multivariate GARCH (MGARCH)

* **Key Variants:**
  * BEKK: Ensures positive definiteness of covariance matrix
  * DCC: Dynamic conditional correlation
  * CCC: Constant conditional correlation
  * GO-GARCH: Generalized orthogonal GARCH

* **Common Features:**
  * Model time-varying covariances/correlations
  * Dimensionality challenges for large portfolios
  * Computational complexity
  * Estimation challenges

* **Financial Applications:**
  * Portfolio risk management
  * Asset allocation
  * Correlation forecasting
  * Contagion and spillover analysis

## State Space Models

### Exponential Smoothing

* **Simple Exponential Smoothing:**
  * Forecast is weighted average of observation and previous forecast
  * Single smoothing parameter α
  * Level component only

* **Holt's Linear Method:**
  * Adds trend component
  * Two smoothing parameters (α, β)
  * Linear trend extrapolation

* **Holt-Winters Seasonal Method:**
  * Adds seasonal component
  * Three smoothing parameters (α, β, γ)
  * Additive or multiplicative seasonality

* **Financial Applications:**
  * Short-term price trend forecasting
  * Seasonal sales prediction
  * Volatility smoothing
  * Trading signal generation

### Structural Time Series Models

* **Components:**
  * Level component
  * Trend component
  * Seasonal component
  * Cycle component
  * Irregular component

* **State Space Representation:**
  * Observation equation
  * State transition equation
  * Stochastic components
  * Time-varying parameters

* **Financial Applications:**
  * Trend-cycle decomposition
  * Business cycle analysis
  * Seasonal adjustment
  * Underlying growth estimation

### Kalman Filtering

* **State Estimation:**
  * Recursive algorithm for state estimation
  * Prediction step and update step
  * Optimal for linear Gaussian systems
  * Extended and unscented variants for nonlinear systems

* **Applications in Time Series:**
  * Parameter estimation
  * Signal extraction
  * Missing data interpolation
  * Time-varying coefficient models

* **Financial Applications:**
  * Factor model estimation
  * Time-varying beta estimation
  * Pairs trading
  * Yield curve modeling

### Dynamic Linear Models (DLM)

* **Bayesian Approach:**
  * Prior distributions for parameters
  * Sequential updating via Bayes rule
  * Forward filtering, backward sampling
  * Parameter learning

* **Key Features:**
  * Handles missing data naturally
  * Incorporates parameter uncertainty
  * Allows for model averaging
  * Produces full predictive distributions

* **Financial Applications:**
  * Regime-switching models
  * Time-varying parameter models
  * Portfolio allocation under uncertainty
  * Economic forecasting

## Implementation Considerations

### Model Selection Criteria

* **Statistical Criteria:**
  * Akaike Information Criterion (AIC)
  * Bayesian Information Criterion (BIC)
  * Hannan-Quinn Information Criterion (HQIC)
  * Likelihood ratio tests

* **Forecast Accuracy Metrics:**
  * Mean Absolute Error (MAE)
  * Root Mean Squared Error (RMSE)
  * Mean Absolute Percentage Error (MAPE)
  * Directional accuracy

* **Residual Diagnostics:**
  * Independence tests (Ljung-Box)
  * Normality tests
  * Heteroskedasticity tests
  * Structural break tests

### Parameter Estimation Challenges

* **Non-stationarity:**
  * Unit root testing
  * Appropriate differencing
  * Trend-stationary vs. difference-stationary

* **Outliers and Structural Breaks:**
  * Intervention analysis
  * Robust estimation
  * Rolling window approaches
  * Regime-switching extensions

* **Numerical Optimization:**
  * Initial value sensitivity
  * Local optima
  * Constraint handling
  * Convergence issues

### Software Implementation

* **Popular Packages:**
  * R: forecast, tseries, fGarch, rugarch
  * Python: statsmodels, arch, pmdarima
  * MATLAB: Econometrics Toolbox
  * Julia: TimeSeriesAnalysis.jl

* **Performance Considerations:**
  * Vectorization for efficiency
  * Parallel processing for multiple series
  * GPU acceleration for large-scale applications
  * Memory management for large datasets

## Getting Started

To begin implementing classical time series methods:

1. Conduct thorough exploratory analysis (refer to [Time Series Fundamentals](./ts-fundamentals.md))
2. Start with simpler models before increasing complexity
3. Validate model assumptions through diagnostic testing
4. Compare multiple models using appropriate criteria
5. Consider ensemble approaches for improved robustness

## Related Documentation

* [Time Series Fundamentals](./ts-fundamentals.md)
* [Machine Learning Approaches](./ts-ml-approaches.md)
* [Implementation Guide](./ts-implementation.md)
* [Evaluation Framework](./ts-evaluation.md)
* [Time Series Forecasting Guide](../time-series-forecasting.md)

---

*Last Updated: 2025-05-29*