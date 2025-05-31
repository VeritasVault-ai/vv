---
document_type: guide
classification: internal
status: draft
version: 0.1.0
last_updated: '2025-05-31'
applies_to:
- AI
reviewers:
- '@tech-lead'
priority: p2
next_review: '2026-05-31'
---

## 1. Layout, Coding, and Style Guidelines

### Architectural Principles

* **Hexagonal/Onion Architecture:** Inbound dependencies only; clear separation of domain, infrastructure, and interfaces.
* **CQRS Everywhere:** Split reads and writes—no mixing business logic with queries.
* **Explicit Contracts:** All interfaces are versioned and signed; no silent upgrades.
* **Idempotency by Default:** Every controller/action can be safely retried.

### Coding Standards

* **Strong Typing:** Strict TypeScript/solidity typing; fail fast, fail loud.
* **Defensive Programming:** Null checks, input validation, and early error returns everywhere.
* **No Magic:** No reflection, global state, or hidden dependencies.
* **Open/Closed Principle:** Extend, don’t modify; build composable, not brittle.
* **DRY, but not Overabstracted:** Reuse, but keep the codebase navigable and human-readable.
* **Consistent Naming:** Domain-driven names for entities, aggregates, and events.

### Styling and Layout

* **Minimalist UI/UX:** Only what’s necessary, no distraction. Admin and ops views default to dark mode, info-dense.
* **Tailwind/utility CSS:** Use utility-first styling for all web UIs.
* **Component Isolation:** No monolith views—compose small, tested UI atoms.
* **ThemeContext:** Global theming; avoid hardcoded colors and font sizes.

---

## 2. Documentation & Auditability

* **Comprehensive API/Controller Docs:** Every endpoint, input, output, and event is documented.
* **Design Artifacts:** Every module change requires updated C4/sequence/DDD diagrams.
* **Versioned Docs:** Documentation is versioned alongside code; docs changes required in PR templates.
* **Change Logs:** All artifacts, models, and events have machine-readable change logs.
* **Audit Trails:** All business decisions and controller actions are signed, timestamped, and hashed.

---

## 3. Security, Testing, & Observability

* **Unit/Integration/System Tests:** 80%+ coverage; fail pipeline if below threshold.
* **Mutation Testing:** Catch what traditional unit tests miss.
* **Continuous Pen-Testing:** Automated threat modeling, simulated attacks each sprint.
* **Zero Trust:** Every action must be authenticated/authorized, even internal calls.
* **Secrets Management:** All keys and credentials rotated and never hardcoded.
* **Observability First:** Structured logging, metrics, distributed tracing, and alerting on all critical paths.
* **Error Boundaries:** All UI and backend layers must gracefully handle and report errors, not hide them.

---

## 4. Model, Data, and Pipeline Practices

* **Atomic Versioning:** No partial updates; rollback as first-class.
* **Dependency Validation:** Validate dependency graphs on every build/deploy; no cycles.
* **Schema Evolution:** All changes backwards-compatible unless major, with automated migration scripts.
* **Immutable Data:** All critical business and compliance data is append-only and versioned.
* **Fairness/Drift Monitoring:** Live metrics, intersectional bias checks, and rollback triggers.
* **Data Provenance:** All data sources, transformations, and model inputs/outputs are signed and traceable.

---

## 5. Layout and Deployment Best Practices

* **Incremental Rollouts:** Canary/Shadow deployments before production; every deployment is rollback tested in staging.
* **Feature Flags:** All experimental features gated and togglable per environment.
* **Automated Compliance Gates:** No deploy without passing compliance, fairness, and security checks.
* **Geo-Redundancy:** Operators and validators must span at least 5 regions.
* **Disaster Recovery:** All state and models backed up with automated restore tested each release.
* **No Side-Treatments:** Everything mission-critical—logging, data validation, error handling, theming—is architected in, not bolted on.

---

## 6. Crosscutting Concerns (Don’t Ignore These)

* **ThemeContext:** Global, not per-view. Test for color, font, and contrast accessibility.
* **Error Boundaries:** All UI and backend exceptions should be logged, reported, and user-meaningful.
* **Validation:** No layer assumes another validated inputs; validate at every boundary.
* **Logging:** Structured and contextual logs for every critical operation.
* **Access Control:** Role-based permissions enforced everywhere, even batch/ops scripts.
* **Resilience:** Circuit breakers, retries, and fallback paths for all critical integrations.

---

## 7. Pitfalls to Avoid

* **Don’t treat logging, validation, or error handling as afterthoughts.** They’re first-class.
* **Don’t skip regression, rollback, or audit testing, even for ‘minor’ changes.**
* **Don’t tolerate undocumented or untested interfaces, ever.**
* **Don’t allow untracked dependencies, shadow deployments, or ‘temporary’ bypasses to remain past the sprint.**
* **Don’t allow a single region, operator, or stakeholder to dominate the system.**

---

## 8. References & Implementation Resources

* [C4 Model Diagrams](./ARCHITECTURE.md)
* [Interface/Controller Contracts](./Controllers/)
* [Testing Strategy](./TESTING.md)
* [Security Playbooks](./SECURITY.md)
* [Deployment/CI Docs](./DEPLOYMENT.md)