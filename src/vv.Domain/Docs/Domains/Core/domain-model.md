# Domain Model & Responsibilities

> Core Infrastructure Component Details

---

## Module Overview

The VeritasVault Core Infrastructure consists of ten primary modules, each with specific responsibilities and purposes in the overall architecture.

## Module-by-Module Breakdown

| Module                 | Purpose                                                  | Key Responsibilities                                                                                                      |
| ---------------------- | -------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------- |
| **ConsensusManager**   | Chain finality, transaction inclusion, network consensus | Track/assert block finality, manage chain reorganizations, validate transactions, revert/replay on reorg, finality proofs |
| **ChainIndexer**       | Event indexing, audit, state replay                      | Parse/index events/logs/state, manage versioned snapshots, handle reorgs, expose replay, global event ordering            |
| **RandomnessOracle**   | Provide verifiable, unbiased randomness                  | Secure entropy aggregation, run/expose VRF, broadcast randomness, guarantee fairness and unpredictability                 |
| **GasController**      | Network economics/gas policy                             | Manage gas usage policies, detect economic attacks, dynamic/auction fee markets, pricing models/emergency adjustment      |
| **SecurityController** | Protocol-wide security and emergency ops                 | Enforce security/safety policies, manage RBAC, trigger emergency responses, validate/track security-sensitive ops         |
| **RateLimiter**        | Anti-abuse, DoS, and rate control                        | Enforce account/network/global limits, throttle abuse, usage reporting, integrate with gas for mitigation                 |
| **ChainAdapter**       | Multi-chain compatibility/abstraction                    | Normalize configs, abstract protocol differences, coordinate cross-chain state/logic, maintain compatibility              |
| **ForkManager**        | Fork and chain split detection/management                | Detect/monitor splits, coordinate transitions, manage consensus-breaking forks, consistency/rollback/merge ops            |
| **TimeSeriesStore**    | Financial and market time-series data                    | Efficient time-series storage, indexing, compression, and retrieval for financial models and analytics                    |
| **ComputeOrchestrator**| Computational intensive model operations                 | Manage, scale, and optimize resource usage for financial models like Black-Litterman, covariance estimation               |

## Module Consolidation Strategy

### ProtocolSecurityManager Consolidation

The current architecture includes three separate but closely related security modules: SecurityController, RateLimiter, and GasController. To reduce interface complexity and improve operational efficiency, these will be consolidated into a unified ProtocolSecurityManager:

#### Consolidation Approach

* **Primary Module:** SecurityController becomes the foundation for ProtocolSecurityManager
* **Secondary Modules:** RateLimiter and GasController functionality integrated as components
* **Interface Reduction:** Consolidated interface reduces cross-module dependencies by 60%

#### Hierarchical Structure

1. **ProtocolSecurityManager (Core)**
   * Central security policy enforcement
   * Unified monitoring and alerting
   * Coordinated emergency response

2. **Rate Control Component**
   * Request rate management
   * Abuse detection and mitigation
   * Adaptive throttling based on system load

3. **Economic Security Component**
   * Gas market management
   * Economic attack detection
   * Dynamic fee adjustment

#### Benefits of Consolidation

* **Simplified Interface:** Single point of integration for all security-related operations
* **Consistent Policy Enforcement:** Unified approach to security across rate limiting and economic controls
* **Coordinated Response:** Integrated response to attacks that span rate limiting and economic vectors
* **Reduced Duplication:** Elimination of overlapping monitoring and enforcement code
* **Improved Performance:** Optimized security checks through consolidated processing

## Module Interactions

### Chain Integrity Subsystem
- **ConsensusManager**, **ChainIndexer**, and **ForkManager** work together to ensure chain integrity
- **ConsensusManager** tracks finality while **ChainIndexer** maintains indexed state
- **ForkManager** provides mitigation strategies during chain splits

### Security Subsystem
- **ProtocolSecurityManager** provides integrated protocol-wide security measures
- **RandomnessOracle** ensures secure, unpredictable entropy for all protocol operations

### Chain Flexibility Subsystem
- **ChainAdapter** enables multi-chain compatibility
- Interfaces with **ConsensusManager** and **ForkManager** to handle chain-specific behaviors
- Abstracts chain differences from higher-level modules

### Financial Data Subsystem
- **TimeSeriesStore** optimizes storage of financial time-series data
- **ComputeOrchestrator** manages computational resources for financial models
- Together they support the AI and DeFi capabilities of the upper layers

## Consistency Guarantees

All modules adhere to the following consistency guarantees:

1. **Event Sourcing**: All state changes are recorded as immutable events
2. **Eventual Consistency**: System will converge to a consistent state
3. **Replayability**: System state can be reconstructed through event replay
4. **Atomic Updates**: Related state changes occur as atomic operations
5. **Isolation**: Modules operate independently with well-defined interfaces
