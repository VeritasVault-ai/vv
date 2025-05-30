# Time Series Training Pipeline

> Guide for building effective training pipelines for time series models

---

## Pipeline Architecture

### Key Components

* **Data Ingestion Layer**:
  * Historical data sources
  * Point-in-time reconstruction
  * Data versioning
  * Lineage tracking

* **Preprocessing Layer**:
  * Temporal alignment
  * Missing value handling
  * Outlier treatment
  * Feature computation

* **Training Orchestration**:
  * Experiment tracking
  * Hyperparameter optimization
  * Cross-validation framework
  * Model artifact management

* **Evaluation Framework**:
  * Metric calculation
  * Benchmark comparison
  * Performance analysis
  * Model diagnostics

### Design Principles

* **Reproducibility**:
  * Deterministic pipeline steps
  * Fixed random seeds
  * Environment isolation
  * Complete parameter logging

* **Scalability**:
  * Parallel feature computation
  * Distributed training
  * Incremental processing capability
  * Resource optimization

* **Governance**:
  * Data access controls
  * Model documentation
  * Approval workflows
  * Compliance checks

## Data Preparation

### Historical Data Management

* **Point-in-Time Architecture**:
  * As-of date snapshots
  * Timeline consistency
  * Survivorship bias prevention
  * Look-ahead bias prevention

* **Data Quality Processes**:
  * Automated validation rules
  * Schema enforcement
  * Consistency checks
  * Source comparison

* **Data Versioning**:
  * Training dataset snapshots
  * Changelog tracking
  * Feature dataset versions
  * Reference data versioning

### Temporal Structures

* **Time Alignment**:
  * Regular vs. irregular timestamps
  * Calendar adjustments
  * Time zone normalization
  * Business day conventions

* **Window Creation**:
  * Feature window definition
  * Prediction horizon setting
  * Gap handling
  * Overlapping vs. non-overlapping

* **Temporal Splits**:
  * Train-validation-test boundaries
  * Forward-looking validation
  * Multiple test periods
  * Out-of-time validation

## Feature Engineering

### Feature Types

* **Temporal Features**:
  * Lags and differences
  * Rolling statistics
  * Temporal indicators
  * Seasonality encodings

* **Domain-Specific Features**:
  * Technical indicators (finance)
  * Cross-sectional statistics
  * External factors integration
  * Calendar effects

* **Advanced Features**:
  * Frequency domain features
  * Shape descriptors
  * Pattern detectors
  * Interaction features

### Feature Pipeline Design

* **Pipeline Structure**:
  * Modular transformers
  * Sequential processing
  * Transformation graphs
  * Feature groups

* **Time-Aware Processing**:
  * Temporal dependencies
  * Forward-only transformations
  * Time-windowed operations
  * Seasonal adjustments

* **Feature Selection**:
  * Stability analysis
  * Importance ranking
  * Redundancy elimination
  * Domain validation

## Model Training Framework

### Training Loop Design

* **Core Components**:
  * Data loader with temporal awareness
  * Validation strategy
  * Loss function definition
  * Optimization method

* **Training Variations**:
  * Single-step vs. multi-step forecasting
  * Direct vs. recursive forecasting
  * Global vs. local models
  * Univariate vs. multivariate

* **Regularization Approaches**:
  * Time-based regularization
  * Parameter constraints
  * Dropout variations
  * Early stopping criteria

### Hyperparameter Optimization

* **Search Strategies**:
  * Grid search with time limits
  * Bayesian optimization
  * Multi-objective optimization
  * Population-based training

* **Time Series Considerations**:
  * Time-based validation impact
  * Regime-specific performance
  * Stability across time periods
  * Computational constraints

* **Optimization Metrics**:
  * Primary business metric
  * Multiple forecast horizons
  * Weighted error combination
  * Consistency measures

### Ensemble Methods

* **Temporal Ensembles**:
  * Multi-horizon specialists
  * Regime-specific models
  * Seasonal experts
  * Hierarchical combination

* **Model Diversity**:
  * Algorithm diversity
  * Feature subset diversity
  * Parameter diversity
  * Random initialization

* **Combination Techniques**:
  * Simple averaging
  * Error-based weighting
  * Stacked generalization
  * Dynamic weighting

## Validation Framework

### Time Series Cross-Validation

* **Forward Validation**:
  * Expanding window approach
  * Sliding window approach
  * Multiple origin points
  * Blocked validation

* **Multi-Horizon Validation**:
  * Horizon-specific metrics
  * Forecast decay analysis
  * Cumulative error assessment
  * Distribution of errors

* **Robustness Testing**:
  * Stress period performance
  * Regime transition handling
  * Extreme value prediction
  * Noise sensitivity

### Performance Analysis

* **Error Decomposition**:
  * Bias vs. variance analysis
  * Systematic vs. random errors
  * Temporal error patterns
  * Feature-conditional errors

* **Diagnostic Visualizations**:
  * Actual vs. predicted plots
  * Residual analysis
  * Error autocorrelation
  * Prediction intervals

* **Business Impact Metrics**:
  * Decision quality metrics
  * Financial performance
  * Operational metrics
  * Risk-adjusted performance

## Implementation Example

Here's a simplified implementation of a time series training pipeline:

```python
import pandas as pd
import numpy as np
from sklearn.preprocessing import StandardScaler
from sklearn.pipeline import Pipeline
from sklearn.model_selection import TimeSeriesSplit
from sklearn.metrics import mean_squared_error, mean_absolute_error
import joblib
import json
import os
from datetime import datetime

class TimeSeriesTrainingPipeline:
    """End-to-end pipeline for time series model training"""
    
    def __init__(self, config):
        """
        Initialize training pipeline with configuration
        
        Parameters:
        -----------
        config : dict
            Configuration parameters for pipeline
            - data_params: Data loading and preparation settings
            - feature_params: Feature engineering settings
            - model_params: Model training parameters
            - validation_params: Validation strategy
            - output_params: Output and artifact settings
        """
        self.config = config
        self.artifacts = {}
        self.metrics = {}
        self.output_dir = config['output_params']['artifact_dir']
        os.makedirs(self.output_dir, exist_ok=True)
        
    def prepare_data(self, data_source):
        """
        Load and prepare time series data
        
        Parameters:
        -----------
        data_source : str or DataFrame
            Data source path or DataFrame
            
        Returns:
        --------
        DataFrame : Prepared data
        """
        # Load data
        if isinstance(data_source, str):
            data = pd.read_csv(data_source, parse_dates=[self.config['data_params']['date_column']])
        else:
            data = data_source.copy()
            
        # Set date as index if not already
        if not isinstance(data.index, pd.DatetimeIndex):
            data = data.set_index(self.config['data_params']['date_column'])
            
        # Sort by time
        data = data.sort_index()
        
        # Handle missing values based on config
        missing_strategy = self.config['data_params'].get('missing_strategy', 'ffill')
        if missing_strategy == 'ffill':
            data = data.ffill()
        elif missing_strategy == 'interpolate':
            data = data.interpolate(method='time')
            
        # Store prepared data
        self.artifacts['prepared_data'] = data
        
        return data
    
    def create_features(self, data):
        """
        Create time series features
        
        Parameters:
        -----------
        data : DataFrame
            Input data
            
        Returns:
        --------
        DataFrame : Features and targets
        """
        feature_config = self.config['feature_params']
        target_col = feature_config['target_column']
        
        # Initialize with base features
        features = pd.DataFrame(index=data.index)
        
        # Add lag features
        for lag in feature_config.get('lags', [1, 7, 14, 28]):
            features[f'{target_col}_lag_{lag}'] = data[target_col].shift(lag)
            
        # Add rolling window features
        for window in feature_config.get('windows', [7, 14, 30]):
            features[f'{target_col}_rolling_mean_{window}'] = data[target_col].rolling(window).mean().shift(1)
            features[f'{target_col}_rolling_std_{window}'] = data[target_col].rolling(window).std().shift(1)
            
        # Add date-based features
        if feature_config.get('use_date_features', True):
            features['dayofweek'] = features.index.dayofweek
            features['month'] = features.index.month
            features['quarter'] = features.index.quarter
            features['year'] = features.index.year
            
        # Add target
        features['target'] = data[target_col]
        
        # Remove rows with NaN due to lag creation
        features = features.dropna()
        
        # Store feature data
        self.artifacts['feature_data'] = features
        
        return features
    
    def split_data(self, features):
        """
        Split data into train and validation sets
        
        Parameters:
        -----------
        features : DataFrame
            Feature data
            
        Returns:
        --------
        dict : Train and validation splits
        """
        split_config = self.config['validation_params']['split']
        
        if split_config['method'] == 'fixed_date':
            # Split at specific date
            split_date = pd.Timestamp(split_config['split_date'])
            train = features[:split_date]
            valid = features[split_date:]
            
        elif split_config['method'] == 'ratio':
            # Split by ratio
            split_idx = int(len(features) * split_config['train_ratio'])
            train = features.iloc[:split_idx]
            valid = features.iloc[split_idx:]
            
        # Create X, y pairs
        X_train = train.drop('target', axis=1)
        y_train = train['target']
        X_valid = valid.drop('target', axis=1)
        y_valid = valid['target']
        
        # Store splits
        splits = {
            'X_train': X_train,
            'y_train': y_train,
            'X_valid': X_valid,
            'y_valid': y_valid
        }
        
        self.artifacts['data_splits'] = {
            'train_start': X_train.index[0],
            'train_end': X_train.index[-1],
            'valid_start': X_valid.index[0],
            'valid_end': X_valid.index[-1],
            'train_size': len(X_train),
            'valid_size': len(X_valid)
        }
        
        return splits
    
    def create_preprocessing_pipeline(self):
        """
        Create preprocessing pipeline
        
        Returns:
        --------
        Pipeline : Scikit-learn preprocessing pipeline
        """
        preprocessing_steps = []
        
        # Add feature scaling if configured
        if self.config['feature_params'].get('apply_scaling', True):
            scaler = StandardScaler()
            preprocessing_steps.append(('scaler', scaler))
        
        # Create and return pipeline
        preprocessing_pipeline = Pipeline(preprocessing_steps)
        
        return preprocessing_pipeline
    
    def train_model(self, splits):
        """
        Train time series model
        
        Parameters:
        -----------
        splits : dict
            Data splits from split_data()
            
        Returns:
        --------
        tuple : (trained_model, preprocessing_pipeline)
        """
        model_config = self.config['model_params']
        model_type = model_config['model_type']
        
        # Create preprocessing pipeline
        preprocessing = self.create_preprocessing_pipeline()
        
        # Fit preprocessing on training data
        X_train_processed = preprocessing.fit_transform(splits['X_train'])
        
        # Initialize model based on type
        if model_type == 'xgboost':
            import xgboost as xgb
            model = xgb.XGBRegressor(**model_config.get('hyperparams', {}))
            
            # Train model
            model.fit(
                X_train_processed, 
                splits['y_train'],
                eval_set=[(preprocessing.transform(splits['X_valid']), splits['y_valid'])],
                early_stopping_rounds=model_config.get('early_stopping', 50),
                verbose=model_config.get('verbose', True)
            )
            
        elif model_type == 'lightgbm':
            import lightgbm as lgb
            model = lgb.LGBMRegressor(**model_config.get('hyperparams', {}))
            
            # Train model
            model.fit(
                X_train_processed, 
                splits['y_train'],
                eval_set=[(preprocessing.transform(splits['X_valid']), splits['y_valid'])],
                early_stopping_rounds=model_config.get('early_stopping', 50),
                verbose=model_config.get('verbose', True)
            )
            
        else:
            raise ValueError(f"Unsupported model type: {model_type}")
            
        # Store model and preprocessing
        self.artifacts['model'] = model
        self.artifacts['preprocessing'] = preprocessing
        
        return model, preprocessing
    
    def evaluate_model(self, model, preprocessing, splits):
        """
        Evaluate model performance
        
        Parameters:
        -----------
        model : trained model
            The trained forecasting model
        preprocessing : Pipeline
            Preprocessing pipeline
        splits : dict
            Data splits from split_data()
            
        Returns:
        --------
        dict : Performance metrics
        """
        # Generate predictions
        X_valid_processed = preprocessing.transform(splits['X_valid'])
        predictions = model.predict(X_valid_processed)
        
        # Calculate metrics
        metrics = {}
        metrics['rmse'] = np.sqrt(mean_squared_error(splits['y_valid'], predictions))
        metrics['mae'] = mean_absolute_error(splits['y_valid'], predictions)
        
        # Calculate MAPE, handling zero values
        y_true = splits['y_valid'].values
        mask = y_true != 0
        if mask.sum() > 0:
            metrics['mape'] = np.mean(np.abs((y_true[mask] - predictions[mask]) / y_true[mask])) * 100
        else:
            metrics['mape'] = np.nan
        
        # Directional accuracy
        if len(y_true) > 1:
            actual_dirs = np.sign(np.diff(y_true))
            pred_dirs = np.sign(np.diff(predictions))
            metrics['dir_accuracy'] = np.mean(actual_dirs == pred_dirs)
        
        # Store metrics and predictions
        self.metrics = metrics
        self.artifacts['predictions'] = {
            'dates': splits['X_valid'].index,
            'actuals': splits['y_valid'].values,
            'predictions': predictions
        }
        
        return metrics
    
    def save_artifacts(self):
        """
        Save model artifacts and metadata
        
        Returns:
        --------
        str : Path to saved artifacts
        """
        timestamp = datetime.now().strftime("%Y%m%d_%H%M%S")
        model_dir = os.path.join(self.output_dir, f"model_{timestamp}")
        os.makedirs(model_dir, exist_ok=True)
        
        # Save model
        model_path = os.path.join(model_dir, "model.joblib")
        joblib.dump(self.artifacts['model'], model_path)
        
        # Save preprocessing pipeline
        preprocessing_path = os.path.join(model_dir, "preprocessing.joblib")
        joblib.dump(self.artifacts['preprocessing'], preprocessing_path)
        
        # Save metrics
        metrics_path = os.path.join(model_dir, "metrics.json")
        with open(metrics_path, 'w') as f:
            json.dump(self.metrics, f, indent=2)
        
        # Save data split info
        splits_path = os.path.join(model_dir, "data_splits.json")
        with open(splits_path, 'w') as f:
            json.dump({
                k: str(v) if isinstance(v, (pd.Timestamp, datetime)) else v
                for k, v in self.artifacts['data_splits'].items()
            }, f, indent=2)
        
        # Save feature importance if available
        if hasattr(self.artifacts['model'], 'feature_importances_'):
            feature_names = self.artifacts['data_splits']['X_train'].columns
            importance = self.artifacts['model'].feature_importances_
            
            feature_importance = {
                name: float(imp) for name, imp in zip(feature_names, importance)
            }
            
            importance_path = os.path.join(model_dir, "feature_importance.json")
            with open(importance_path, 'w') as f:
                json.dump(feature_importance, f, indent=2)
        
        # Save config
        config_path = os.path.join(model_dir, "config.json")
        with open(config_path, 'w') as f:
            # Convert any non-serializable objects to strings
            config_safe = json.loads(json.dumps(self.config, default=str))
            json.dump(config_safe, f, indent=2)
            
        # Create model card with summary
        card = {
            "model_version": timestamp,
            "created_at": datetime.now().isoformat(),
            "model_type": self.config['model_params']['model_type'],
            "target_variable": self.config['feature_params']['target_column'],
            "training_period": f"{self.artifacts['data_splits']['train_start']} to {self.artifacts['data_splits']['train_end']}",
            "validation_period": f"{self.artifacts['data_splits']['valid_start']} to {self.artifacts['data_splits']['valid_end']}",
            "performance_metrics": self.metrics,
            "artifacts": {
                "model": "model.joblib",
                "preprocessing": "preprocessing.joblib",
                "metrics": "metrics.json",
                "data_splits": "data_splits.json",
                "config": "config.json"
            }
        }
        
        card_path = os.path.join(model_dir, "model_card.json")
        with open(card_path, 'w') as f:
            json.dump(card, f, indent=2)
            
        return model_dir
    
    def run_pipeline(self, data_source):
        """
        Run full training pipeline
        
        Parameters:
        -----------
        data_source : str or DataFrame
            Data source path or DataFrame
            
        Returns:
        --------
        dict : Pipeline results
        """
        # Prepare data
        data = self.prepare_data(data_source)
        
        # Create features
        features = self.create_features(data)
        
        # Split data
        splits = self.split_data(features)
        
        # Train model
        model, preprocessing = self.train_model(splits)
        
        # Evaluate model
        metrics = self.evaluate_model(model, preprocessing, splits)
        
        # Save artifacts
        artifact_path = self.save_artifacts()
        
        return {
            "metrics": metrics,
            "artifact_path": artifact_path,
            "model": model,
            "preprocessing": preprocessing
        }