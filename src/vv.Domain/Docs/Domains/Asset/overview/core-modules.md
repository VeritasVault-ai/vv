# Core Modules

> Overview of the main functional modules in the Asset Domain

---

## Overview

The Asset Domain is organized into a modular architecture where each module handles specific aspects of asset management. This document provides an overview of these core modules, their responsibilities, and how they interact within the system.

## Module Architecture

The Asset Domain follows a layered modular architecture:

```
┌─────────────────────────────────────────────────────────┐
│                  Application Services                   │
└───────────────────────────┬─────────────────────────────┘
                            │
┌───────────────────────────┼─────────────────────────────┐
│                           │                             │
│  ┌─────────────────┐  ┌───▼───────────┐  ┌────────────┐ │
│  │  Asset Registry │  │ Portfolio     │  │ Risk       │ │
│  │  Module         │◄─┤ Management    │─►│ Management │ │
│  │                 │  │ Module        │  │ Module     │ │
│  └─────────────────┘  └───┬───────────┘  └────┬───────┘ │
│           ▲                │                   │         │
│           │                │                   │         │
│           │                │                   │         │
│  ┌────────┴────────┐  ┌───▼───────────┐  ┌────▼───────┐ │
│  │ Factor Models   │  │ Settlement    │  │ Performance │ │
│  │ Module          │  │ Module        │  │ Module      │ │
│  │                 │  │               │  │             │ │
│  └─────────────────┘  └───────────────┘  └─────────────┘ │
│                                                          │
│                   Domain Services                        │
└─────────────────────────────────────────────────────────┘
```

## Module Overview

Due to the complexity of each module, detailed documentation is provided in separate files. Here is a summary of the core modules:

### [Asset Registry Module](./modules/asset-registry-module.md)

The central repository for all asset information:

* Asset data management and metadata
* Asset classification and categorization
* Reference data management
* Corporate action processing
* Asset lifecycle management

### [Portfolio Management Module](./modules/portfolio-management-module.md)

Responsible for portfolio construction and management:

* Portfolio modeling and construction
* Portfolio optimization
* Rebalancing strategies
* Constraint management
* What-if scenario analysis

### [Factor Models Module](./modules/factor-models-module.md)

Handles factor-based analysis and modeling:

* Factor definition and construction
* Factor exposure calculation
* Factor return estimation
* Multi-factor models
* Factor-based portfolio construction

### [Risk Management Module](./modules/risk-management-module.md)

Manages all aspects of investment risk:

* Risk measurement and analytics
* Risk decomposition
* Scenario analysis and stress testing
* Risk budgeting and allocation
* Risk monitoring and reporting

### [Performance Module](./modules/performance-module.md)

Calculates and analyzes investment performance:

* Return calculation
* Performance attribution
* Benchmark comparison
* Performance analytics
* Performance reporting

### [Settlement Module](./modules/settlement-module.md)

Handles the settlement of transactions:

* Settlement workflow management
* Settlement verification
* Atomic settlement operations
* Batch settlement processing
* Settlement finality assurance

## Cross-Module Integration

The modules interact through well-defined interfaces:

### Key Integration Points

* **Asset Registry ↔ Portfolio Management**: Asset data for portfolio construction
* **Portfolio Management ↔ Risk Management**: Portfolio analysis for risk assessment
* **Factor Models ↔ Performance**: Factor data for attribution analysis
* **Portfolio Management ↔ Settlement**: Transaction processing for portfolio changes
* **Risk Management ↔ Factor Models**: Factor data for risk decomposition

### Integration Patterns

* **Event-based Communication**: Asynchronous notifications between modules
* **Service Interfaces**: Well-defined APIs for module interactions
* **Shared Domain Models**: Common data structures between modules
* **Workflow Orchestration**: Process management across modules

## Extension Points

Each module provides extension mechanisms:

* **Plugin Architecture**: Custom components can be added
* **Strategy Pattern**: Interchangeable algorithms
* **Configuration Options**: Behavioral customization
* **Custom Providers**: Integration with external systems

## Data Flow

The general data flow through the Asset Domain:

1. Asset data enters through the Asset Registry
2. Portfolio Management constructs portfolios using assets
3. Factor Models provide factor exposures and returns
4. Risk Management calculates risk metrics
5. Performance Module measures and attributes performance
6. Settlement Module handles transaction execution

## Implementation Status

For the current implementation status of each module, see:
* [Implementation Phases](./implementation-phases.md)

## Related Documentation

For detailed documentation on each module, refer to:

* [Asset Registry Module](./modules/asset-registry-module.md)
* [Portfolio Management Module](./modules/portfolio-management-module.md)
* [Factor Models Module](./modules/factor-models-module.md)
* [Risk Management Module](./modules/risk-management-module.md)
* [Performance Module](./modules/performance-module.md)
* [Settlement Module](./modules/settlement-module.md)