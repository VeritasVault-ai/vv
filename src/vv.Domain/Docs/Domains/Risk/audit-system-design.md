---
document_type: audit-system-design
classification: internal
status: draft
version: 1.0.0
last_updated: 2025-05-30
applies_to: [risk-compliance-audit]
dependencies: [../../SECURITY.md, ../risk-architecture.md]
reviewers: [secops-lead, risk-lead, compliance-officer, lead-architect]
next_review: 2025-07-15
priority: p0
---

# VeritasVault Risk Domain – Audit System Design  

> Complements the unified security standard defined in [`../../SECURITY.md`](../../SECURITY.md).  
> **Goal:** deliver an immutable, real-time, and cryptographically verifiable audit layer that satisfies institutional, regulatory, and forensic requirements.

---

## 1. Architecture Overview  

```mermaid
flowchart TD
    A[Domain Services<br/>(Risk, Compliance)] -->|Event| B(Audit Logger)
    B --> C{Signing &<br/>Hash-Chaining}
    C --> D[Event Bus<br/>(MessageBus)]
    D --> E[Hot Store<br/>(LSM DB)]
    D --> F[Cold Store<br/>IPFS / S3 Glacier]
    E -->|Stream| G[SIEM /<br/>Realtime Monitors]
    E --> H[Compliance<br/>Reporter]
    H --> I[Regulators / Auditors]
```

* **Audit Logger** – domain-local component that transforms in-memory events into canonical log records.  
* **Signing & Hash-Chaining** – attaches EIP-712 signature + previous-hash to every record.  
* **Event Bus** – MessageBus channel `audit.*` for fan-out to storage, SIEM, and monitors.  
* **Hot Store** – high-throughput LSM DB (e.g., RocksDB / Cosmos DB) for 30-day query SLA.  
* **Cold Store** – chunked, content-addressable objects (IPFS/S3) retained ≥ 5 years.  
* **Compliance Reporter** – scheduled job generating SOC2, ISO 27001, MiCA artefacts.

---

## 2. Audit Log Structure & Format  

| Field                | Type      | Description                                      |
|----------------------|-----------|--------------------------------------------------|
| `recordId`           | `bytes32` | SHA-256(record) – unique identifier             |
| `prevHash`           | `bytes32` | Chain link to previous audit record              |
| `timestamp`          | `uint64`  | Unix epoch (ms)                                  |
| `domain`             | `string`  | e.g. `risk`, `asset`, `core`                     |
| `actor`              | `bytes20` | EVM address or DID of caller                     |
| `action`             | `string`  | Verb e.g. `RiskModel.UpdateFactor`               |
| `payloadHash`        | `bytes32` | Keccak256 of opaque payload                      |
| `sig`                | `bytes`   | EIP-712/BLS signature of all fields              |
| `meta`               | `map`     | Arbitrary key/value (<32 bytes each)             |

JSON-Lines is the wire format; Protobuf used on-chain/off-chain internal bus.

---

## 3. Cryptographic Signing & Verification  

1. **EIP-712 Domain Separator** includes chainId, contract address, log schema version.  
2. **Ledger-backed Keys** (HSM) sign at logger runtime.  
3. **Verification Pipeline**:  
   * Verify signature → Validate prevHash continuity → Persist.  
   * Any failure raises `SecurityIncidentDetected` (severity P2).  
4. **Merkle Root Anchoring** – every 10 min a Merkle root of the last N records is anchored to Core Infrastructure via `AuditRootAnchored` event, enabling lightweight proofs.

---

## 4. Tamper-Proof Storage Design  

| Layer      | Technique                               | RPO / RTO  |
|------------|-----------------------------------------|------------|
| Hot Store  | WORM tables, append-only, immutable TTL | RPO ≤ 5 s  |
| Cold Store | IPFS CIDs pinned + glacier copies       | RPO ≤ 15 m |
| Anchor     | On-chain Merkle roots (Core)            | Immutable  |

Immutable storage policies enforced by `StorageController` smart contract.  Any mutation triggers circuit breaker.

---

## 5. Real-Time Audit Event Processing  

* **Streaming** – Kafka/AMQP channel `audit.*` replicated to SIEM (Elastic/Splunk).  
* **Alert Rules** – severity mapping (P0-P5) fed into SIEM; P0/P1 auto-page.  
* **Back-Pressure** – Audit Logger buffers ≤ 50 k events; overflow pauses domain service (circuit breaker).  
* **Latency SLO** – ≤ 2 s from emit to hot-store persistence.

---

## 6. Compliance Reporting  

* **Scheduled Jobs** produce:  
  * SOC 2 / ISO 27001 control evidence bundles (JSON + CSV).  
  * MiCA on-chain operation logs (XBRL).  
  * Internal quarterly attestation PDFs with SHA-256 checksum.  
* **On-Demand** – auditors can call `GET /audit/reports?range=...` to generate custom extracts.

---

## 7. Integration Points  

| Consuming Domain  | Purpose                                   |
|-------------------|-------------------------------------------|
| **Core Infra**    | Anchors Merkle roots; consumes incident events |
| **Governance**    | Uses audit proofs for proposal validation |
| **Integration**   | Exposes audit query APIs to external partners |
| **Cross-Cutting** | SIEM dashboards, incident response flows |
| **AI / ML**       | Feeds anomaly-detection training data     |

---

## 8. Performance & Scalability  

* Target **25 k events/s** burst, 5 k sustained.  
* Horizontal shard-per-tenant model for hot store.  
* Async compression + chunking for cold archive (Zstd, 128 MiB chunks).  
* Index on `(timestamp, actor, action)` with Bloom filters for rapid selectivity.  
* Benchmarks run nightly; SLA alerts if p95 write latency > 50 ms.

---

## 9. Event Sourcing Patterns  

* Domain services emit **immutable events**; audit system is **secondary projection**.  
* `EventStore` = authoritative for aggregates; AuditLog = compliance view.  
* Replayable: rebuilding AuditLog from EventStore must be deterministic.

---

## 10. Audit Query APIs  

`/audit/logs`  
* `GET /audit/logs?from=ts&to=ts&actor=0x..&action=RiskModel.UpdateFactor` – filtered fetch  
* `GET /audit/logs/{recordId}/proof` – returns Merkle proof & signature chain  
* `POST /audit/reports` – generate ad-hoc compliance report (async job)  

All endpoints require `ROLE_AUDITOR` + OAuth2, emit `AuditQueryExecuted` event.

---

## 11. Security Reference  

For overarching threat models, circuit-breaker design, and access-control norms refer to [`../../SECURITY.md`](../../SECURITY.md).  
This document inherits **all** mandatory controls and review procedures from that central standard.

---

## 12. Change Log  

| Version | Date | Author            | Notes                     |
|---------|------|-------------------|---------------------------|
| 1.0.0   | 2025-05-30 | Factory Assistant | Initial draft – complements SECURITY.md |
