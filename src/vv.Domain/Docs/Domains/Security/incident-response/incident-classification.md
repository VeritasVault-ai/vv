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

# Incident Classification

> Severity levels and impact dimensions for operational incidents

---

## 1. Overview

This document defines the standardized classification system for operational incidents within the VeritasVault platform. Proper classification ensures appropriate resource allocation, response prioritization, and communication cadence for all incidents.

## 2. Severity Levels

### Critical (P1)

* **Definition:** Complete service unavailability or data integrity issues
* **Impact:** Financial impact or significant user disruption
* **Response:** Requires immediate, all-hands response
* **Examples:** 
  * Bridge failure preventing all cross-chain transactions
  * Security breach with active exploitation
  * Data corruption affecting user balances
  * Complete API gateway outage

### High (P2)

* **Definition:** Partial service degradation or major component failure
* **Impact:** Significant performance issues affecting users
* **Response:** Requires urgent response within business hours
* **Examples:**
  * API errors affecting subset of endpoints
  * Message delivery failures with significant delay
  * Oracle delays affecting pricing data
  * Authentication service degradation

### Medium (P3)

* **Definition:** Minor service degradation or non-critical failures
* **Impact:** Limited user impact or with workarounds available
* **Response:** Requires attention but not immediate response
* **Examples:**
  * Increased latency within acceptable thresholds
  * Non-critical errors with automatic retry
  * Capacity warnings without immediate risk
  * Secondary feature unavailability

### Low (P4)

* **Definition:** Cosmetic issues or minor bugs
* **Impact:** No user impact but requires tracking
* **Response:** Scheduled fix in normal release cycle
* **Examples:**
  * UI glitches not affecting functionality
  * Optimization opportunities identified
  * Technical debt items
  * Documentation inconsistencies

## 3. Impact Dimensions

### User Impact

* **Measurement:** Number/percentage of affected users
* **Categories:**
  * Critical: >25% of users affected
  * High: 5-25% of users affected
  * Medium: 1-5% of users affected
  * Low: <1% of users affected

### Functionality Impact

* **Measurement:** Criticality of affected functionality
* **Categories:**
  * Critical: Core transaction functionality affected
  * High: Important but non-core functionality affected
  * Medium: Secondary functionality affected
  * Low: Optional or cosmetic functionality affected

### Data Impact

* **Measurement:** Potential for data loss or corruption
* **Categories:**
  * Critical: Confirmed data loss or corruption
  * High: Potential for data loss or temporary inconsistency
  * Medium: Data delays or temporary unavailability
  * Low: No data integrity impact

### Reputational Impact

* **Measurement:** Public visibility and perception
* **Categories:**
  * Critical: Widespread external awareness, media coverage
  * High: Limited external awareness, social media mentions
  * Medium: Internal visibility only
  * Low: Minimal awareness, limited to technical team

### Financial Impact

* **Measurement:** Direct or indirect financial consequences
* **Categories:**
  * Critical: >$100,000 potential impact
  * High: $10,000-$100,000 potential impact
  * Medium: $1,000-$10,000 potential impact
  * Low: <$1,000 potential impact

### Security Impact

* **Measurement:** Potential security implications
* **Categories:**
  * Critical: Confirmed breach or exploit
  * High: Vulnerability with potential for exploitation
  * Medium: Security anomaly requiring investigation
  * Low: Security improvement opportunity

## 4. Classification Matrix

| Dimension | Critical (P1) | High (P2) | Medium (P3) | Low (P4) |
|-----------|--------------|-----------|-------------|----------|
| User Impact | >25% affected | 5-25% affected | 1-5% affected | <1% affected |
| Functionality | Core functionality | Important functionality | Secondary functionality | Optional functionality |
| Data | Confirmed loss/corruption | Potential loss | Delays/unavailability | No impact |
| Reputational | External media coverage | Social media mentions | Internal visibility | Technical team only |
| Financial | >$100K impact | $10K-$100K impact | $1K-$10K impact | <$1K impact |
| Security | Confirmed breach | Potential exploitation | Anomaly for investigation | Improvement opportunity |

## 5. Classification Process

1. **Initial Assessment:**
   * First responder evaluates against classification criteria
   * Default to higher severity when uncertain
   * Document initial assessment rationale

2. **Severity Confirmation:**
   * Incident Commander validates classification
   * May adjust based on additional information
   * Documents any classification changes

3. **Dynamic Reclassification:**
   * Severity level may change during incident lifecycle
   * Changes must be documented and communicated
   * Requires Incident Commander approval

4. **Multiple Dimension Handling:**
   * Overall classification based on highest dimension
   * Secondary dimensions noted in incident record
   * Response tailored to address all dimensions

## 6. References

* [Incident Response Overview](../../ExternalInterface/benchmarks/incident-response.md)
* [Incident Lifecycle](./incident-lifecycle.md)
* [Communication Templates](./communication-templates.md)

---

## 7. Document Control

* **Owner:** Incident Response Lead
* **Last Updated:** 2025-05-29
* **Status:** Draft