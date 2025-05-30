---
document_type: architecture
classification: internal
status: draft
version: 1.0.0
last_updated: 2025-05-30
applies_to: [core-infrastructure]
dependencies: []
reviewers: [lead-architect, secops-lead, infra-lead]
next_review: 2025-07-15
priority: p0
---

# Core Infrastructure – Domain Model

> Canonical source for all business rules, contracts, and events governing VeritasVault’s foundational security and consensus layer.

---

## 1. Executive Summary

The Core Infrastructure domain provides **chain-level security, consensus finality, event indexing, gas-economics, abuse control, and randomness** for every other VeritasVault artifact.  
It embodies zero-trust principles, immutable audit, and deterministic behaviour, ensuring that higher-level business logic remains reliable even under adversarial conditions.

---

## 2. Domain Overview

### Responsibilities
* Maintain canonical view of on-chain state and finality (`ConsensusManager`, `ChainIndexer`)
* Provide verifiable randomness (`RandomnessOracle`)
* Enforce gas / fee policy and DoS protection (`GasController`, `RateLimiter`)
* Detect, classify, and halt on security incidents (`SecurityController`)
* Abstract multi-chain interactions (`ChainAdapter`)
* Manage forks and upgrades (`ForkManager`)

### Boundaries
* **In-Scope:** Consensus, indexing, randomness proofs, gas policy, incident handling, rate-limiting, fork detection.
* **Out-of-Scope:** Business-level asset logic, UI, analytics; these consume Core events through adapters.

---

## 3. Domain Model Structure (DDD)

### 3.1 Aggregate Roots

| Aggregate Root | Purpose |
|----------------|---------|
| **ConsensusManager** | Tracks finality, validates blocks, orchestrates fork resolution |
| **ChainIndexer** | Persists full chain history & emits `ChainEvent`s |
| **RandomnessOracle** | Supplies VRF-based randomness and proofs |
| **GasController** | Calculates & updates `GasPolicy` rules |
| **SecurityController** | Detects threats, triggers circuit-breakers |
| **RateLimiter** | Maintains `RateLimitConfig`, counts actions per resource |
| **ChainAdapter** | Uniform interface to multi-chain operations |
| **ForkManager** | Monitors forks, coordinates safe network upgrades |

### 3.2 Entities

| Entity | Description |
|--------|-------------|
| **Block** | Immutable record of transactions and metadata |
| **ChainEvent** | Abstract superclass for all chain-level occurrences |
| **SecurityIncident** | Detailed incident report inc. severity, proof, responder |
| **ForkEvent** | Fork start / resolution descriptor |

### 3.3 Value Objects

| Value Object | Attributes |
|--------------|------------|
| **BlockHeader** | parentHash, number, timestamp, stateRoot, validatorSig |
| **GasPolicy** | minGasPrice, maxGasPrice, baseFee, surgeMultiplier |
| **ChainConfig** | chainId, consensusAlgo, finalityDepth |
| **VRFProof** | output, proof, blockNumber |
| **RateLimitConfig** | resourceId, maxRequests, timeWindowSecs |

### 3.4 Domain Events

| Event | Trigger |
|-------|---------|
| **BlockFinalized** | Finality threshold reached for `Block` |
| **ChainReorg** | Competing longer chain detected within `finalityDepth` |
| **RandomnessRequested** | Module requests VRF through `RandomnessOracle` |
| **RandomnessDelivered** | VRFProof validated and delivered |
| **GasPolicyUpdated** | Governance-approved policy change applied |
| **RateLimitBreached** | `RateLimiter` flags exceeding actor/resource |
| **SecurityIncidentDetected** | Threat identified; `SecurityController` engaged |
| **ForkDetected** | Divergent fork identified by `ForkManager` |

### 3.5 Repository Contracts (Interfaces)

```csharp
public interface IConsensusRepository
{
    Task<Block?> GetBlockAsync(long number);
    Task AppendBlockAsync(Block block);
    Task<bool> IsFinalizedAsync(long number);
}

public interface IChainIndexerRepository
{
    IAsyncEnumerable<ChainEvent> StreamEvents(long fromBlock);
}

public interface IRandomnessRepository
{
    Task<VRFProof?> RequestRandomAsync(byte[] seed);
}

public interface ISecurityIncidentRepository
{
    Task LogIncidentAsync(SecurityIncident incident);
    Task<IEnumerable<SecurityIncident>> QueryAsync(DateTime from, DateTime to);
}

public interface IRateLimitRepository
{
    Task<bool> IncrementAsync(string resourceId, string actorId);
    Task<RateLimitConfig> GetConfigAsync(string resourceId);
}

public interface IForkRepository
{
    Task RecordForkAsync(ForkEvent fork);
    Task<IEnumerable<ForkEvent>> GetActiveForksAsync();
}
```

### 3.6 Business Rules & Invariants

| # | Rule |
|---|------|
| BR-1 | `Block.number` **must** be sequential and parentHash verified before persistence. |
| BR-2 | Re-org depth **must not** exceed `ChainConfig.finalityDepth`; otherwise trigger `SecurityIncidentDetected`. |
| BR-3 | `VRFProof` **must** be verifiable against block hash; otherwise reject randomness. |
| BR-4 | `GasPolicy` updates require multi-sig governance and emit `GasPolicyUpdated`. |
| BR-5 | `RateLimiter` denies requests once actor exceeds `maxRequests` within `timeWindowSecs`. |
| BR-6 | Critical `SecurityIncident` severity ≥ **P1** triggers protocol circuit-breaker (halts trading). |

---

## 4. Integration Points

| Domain | Interaction |
|--------|-------------|
| **Risk & Compliance** | Consumes `SecurityIncidentDetected`, `GasPolicyUpdated`, `RateLimitBreached` |
| **Asset & Trading** | Waits for `BlockFinalized` to settle trades; uses `RandomnessOracle` for AMM curves |
| **Integration Gateway** | Relies on `ChainAdapter` for cross-chain proofs |
| **Governance & Ops** | Approves `GasPolicy` changes, triggers upgrades via `ForkManager` |
| **AI / ML** | Feeds on `ChainEvent` stream for anomaly detection models |

---

## 5. Security Considerations

* **Zero-Trust Enforcement** – All external calls pass through `ChainAdapter` with signature verification.  
* **Defense-in-Depth** – Rate limiting, gas throttling, circuit breakers layered to mitigate DoS.  
* **Immutable Audit** – All aggregates emit tamper-proof events; stored by Risk & Audit domain.  
* **Fork Handling** – `ForkManager` locks critical operations until consensus on longest chain.  
* **Randomness Abuse Resistance** – VRF proofs anchored to finalized blocks, preventing prediction.

---

## 6. Implementation Phases

| Phase | Scope | Key Deliverables |
|-------|-------|------------------|
| **1 – Baseline (by 07 Jun 2025)** | ConsensusManager, ChainIndexer, basic RateLimiter | Block storage, event stream, DoS throttle |
| **2 – Security & Randomness (by 24 Jun 2025)** | SecurityController, RandomnessOracle | VRF integration, incident logging, circuit breakers |
| **3 – Multi-Chain & Forks (by 10 Jul 2025)** | ChainAdapter, ForkManager | Cross-chain abstraction, upgrade orchestration |
| **Post-MVP** | Advanced gas economics, adaptive rate limits, chaos-testing hooks | Dynamic GasController, self-healing |

---

## 7. References

* `ARCHITECTURE.md` – platform-level overview (single source of truth)  
* [Cross-Cutting Design](../../Crosscutting/Design.md) – uniform security patterns  
* Ethereum Yellow Paper – consensus fundamentals  
* RFC 9380 VRF – randomness proof standard  

---

## 8. Change Log

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0.0 | 2025-05-30 | Factory Assistant | Initial Core Infrastructure domain model |

---
