---
document_type: api-standards
classification: internal
status: draft
version: 1.0.0
last_updated: 2025-05-30
applies_to: [integration-gateway, integration-analytics-access, core-infrastructure]
dependencies: [../SECURITY.md, ../ARCHITECTURE.md]
reviewers: [gateway-lead, integration-lead, secops-lead, compliance-officer]
next_review: 2025-08-01
priority: p0
---

# VeritasVault Unified API Standards  
_Single source of truth for all HTTP/gRPC APIs across the platform_

---

## 1. Performance & Availability SLAs

| API Class                | p95 Latency (response time) | Availability (rolling 30 d) | Notes                          |
|--------------------------|-----------------------------|-----------------------------|--------------------------------|
| **Critical-Path** (trading, settlement, auth) | **< 200 ms** | **99.99 %** | Must degrade gracefully under load |
| **Standard** (portfolio, data queries) | < 400 ms | 99.9 % |                             |
| **Analytics / Reporting** | < 1 500 ms | 99.5 % | Async export endpoints may stream |
| **Batch / Bulk**         | SLA documented per endpoint | 99.0 % | Use async tasks + callbacks  |

Latency measured **at API Gateway edge**. Breach of SLA must generate `APIPerformanceDegraded` alert.

---

## 2. Security Standards  
_All controls inherit from the [Unified Security & Audit Standard](../SECURITY.md)._ Key API‐specific mandates:

1. **TLS 1.3** minimum; HSTS pre-load headers everywhere.  
2. **OAuth 2.1 + JWT** for user/apps, **mutual-TLS** for service-to-service.  
3. **Scope-based RBAC** embedded in JWT (`scp` claim) checked in middleware.  
4. All privileged routes require **multi-factor auth** for human actors.  
5. Request + response bodies >1 KiB are **SHA-256 hashed** and logged.  
6. Gateway injects **Correlation-ID** header; value propagated end-to-end.

---

## 3. Versioning & Compatibility

* URI scheme: `/api/v{MAJOR}/...` (e.g. `/api/v1/portfolios`).  
* **MAJOR** increments on breaking change, **MINOR** via `Accept-Version` header.  
* Minimum two GA versions simultaneously supported (N and N-1).  
* Deprecations announced 90 days in advance via status `/api/status` and changelog RSS feed.

---

## 4. Request / Response Formats

* **JSON** default (`application/json; charset=utf-8`).  
* **gRPC** used for internal micro-services; external optional.  
* Timestamps in **RFC 3339 / ISO 8601 UTC**.  
* Money amounts as **strings** in smallest unit (e.g. `"100000000"` satoshi).  
* Use snake_case field names for payloads.  
* Standard envelope:

```json
{
  "data": { ... },
  "meta": {
    "correlation_id": "uuid",
    "timestamp": "2025-05-30T10:15:00Z"
  }
}
```

---

## 5. Error Handling

HTTP Status  | Meaning                         | Payload
------------ |-------------------------------- |------------------------------------------------
400          | Validation / malformed request  | `error.code`, `error.message`, `fields[]`
401 / 403    | AuthN / AuthZ failure           | `error.code`, `error.message`
404          | Resource not found              | `error.code`, `error.message`
409          | Optimistic concurrency conflict | Suggested retry policy
429          | Rate-limit exceeded             | `Retry-After` header seconds
5xx          | Server faults                   | Generic message + `incident_id`

Error envelope:

```json
{
  "error": {
    "code": "PORTFOLIO_NOT_FOUND",
    "message": "No portfolio found for id 123",
    "details": { ... }          // optional
  }
}
```

---

## 6. Rate Limiting Patterns

Algorithm: **Token Bucket** enforced at API Gateway.  
Headers returned on every request:

```
X-RateLimit-Limit: 1200
X-RateLimit-Remaining: 742
X-RateLimit-Reset: 1717076400  // Unix epoch seconds
```

Limits tiered by plan:

| Plan           | Burst | Refill / min |
|----------------|-------|--------------|
| Public         |  60   | 60           |
| Authenticated  | 600   | 600          |
| Institutional  | 1200  | 1200         |

Abuse triggers `RateLimitBreached` event (severity P3) and exponential back-off.

---

## 7. Authentication & Authorization

| Actor Type | Flow                 | Token Lifetime | Extra Claims              |
|------------|----------------------|----------------|---------------------------|
| User (UI)  | OAuth 2.1 PKCE       | 30 min         | `tenant_id`, `roles`      |
| API Client | Client-Credentials   | 60 min         | `app_id`, `scopes`        |
| Service    | mTLS + SPIFFE SVID   | 24 h rolling   | `service`, `env`, `build` |

* Mandatory **scopes** per endpoint documented in OpenAPI.  
* Fine-grained ABAC via gateway policy engine (OPA) – evaluated before route.

---

## 8. Monitoring & Observability

Metric                       | Target
-----------------------------|----------------------------
p95 latency                  | See SLA table §1
5xx error rate               | < 0.1 %
Auth failure / total         | < 2 %
Rate-limit hits / total      | < 5 %
Open connection count        | Auto-scale before 70 % max
Request trace sampled        | 100 % critical, 10 % standard

Instrumentation:

* **OpenTelemetry** traces emitted with Correlation-ID.  
* Structured JSON logs shipped to central SIEM (ELK).  
* Prometheus metrics `/metrics` on internal port; gateway aggregates.  
* Real-time SLO dashboards; alerts into OpsGenie.

---

## 9. Implementation Compliance Checklist

- [ ] URI contains `/api/v{MAJOR}` prefix  
- [ ] OpenAPI 3.1 spec with explicit `operationId`, `tags`, and example payloads  
- [ ] All 2xx responses < payload 500 KB, gzip enabled  
- [ ] Error envelope follows §5 format  
- [ ] JWT validation + scope enforcement tested  
- [ ] Rate-limiting headers present and correct  
- [ ] Gateway + service emit OpenTelemetry spans  
- [ ] Endpoint latency & error metrics on dashboards  
- [ ] Contract tests run in CI against stubbed server  
- [ ] Security review checklist signed off (see SECURITY.md)  

---

## 10. References

* OWASP API Security Top 10  
* RFC 9110 – HTTP Semantics  
* OpenAPI Specification 3.1  
* Google API Improvement Proposals (AIPs)  

---

_This document supersedes previous API standards in `Gateway/implementation-guidance/api-standards*`. All domains **MUST** reference this file._  
