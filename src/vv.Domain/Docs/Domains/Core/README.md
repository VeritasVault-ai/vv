---
document_type: domain-overview
classification: internal
status: draft
version: 1.0.0
last_updated: "2025-05-31"
applies_to: [core-infrastructure]
dependencies: []
reviewers: [lead-architect, secops-lead, infra-lead]
next_review: "2026-05-31"
priority: p0
---

# VeritasVault Core Infrastructure

> Foundation Layer – Blockchain & Protocol Security

---

## Overview

The Core Infrastructure domain supplies VeritasVault’s **consensus, finality, randomness, gas-economics, abuse-control, and multi-chain abstraction** capabilities.

For the platform-wide architectural context see the **[High-Level Architecture Overview](../../ARCHITECTURE.md)**.

---

## Table of Contents

1. [Domain Model](./domain-model.md)  
2. [Solidity Interfaces](./solidity-interfaces.md)  
3. [Security & Compliance](#security--compliance)  

---

## Domain Scope (at a glance)

| Capability | Primary Component |
|------------|------------------|
| Consensus & Finality | `ConsensusManager` |
| Chain Event Indexing | `ChainIndexer` |
| Verifiable Randomness | `RandomnessOracle` |
| Gas / Fee Policy | `GasController` |
| Threat Detection & Circuit Breakers | `SecurityController` |
| DoS Mitigation | `RateLimiter` |
| Multi-Chain Adapters | `ChainAdapter` |
| Fork Detection & Upgrade Orchestration | `ForkManager` |

Detailed class and contract specifications live in the **Domain Model** and **Solidity Interfaces** documents linked above.

---

## Security & Compliance

All Core Infrastructure controls adhere to the **[VeritasVault Unified Security & Audit Standard](../../SECURITY.md)**.  
Key obligations:

* Circuit-breaker hooks (`pause()`, `unpause()`) must conform to the global pattern.  
* Every state-changing function emits a signed audit event consumed by the central Audit system.  
* Upgrade operations require the multi-sig & time-lock thresholds defined in the security standard.  
* Rate-limiting and gas-surge policies are configured through governance and logged per the audit requirements.

---

## Integration Summary

Other VeritasVault domains consume Core Infrastructure events and services via the interfaces documented in **Solidity Interfaces**. No business logic outside this domain may bypass these contracts.

---

*Last updated: 2025-05-30*  
