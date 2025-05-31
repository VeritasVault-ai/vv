---
document_type: navigation-index
classification: internal
status: draft
version: 0.1.0
last_updated: "2025-05-31"
applies_to: [platform-wide]
reviewers: [documentation-lead, lead-architect]
next_review: "2026-05-31"
priority: p2
---

# 📚 VeritasVault Documentation – Living Index  

> One-stop map of every Markdown document in `src/vv.Domain/Docs`

---

## 1. Documentation Structure Overview
```
vv.Domain/Docs
 ├─ ARCHITECTURE.md            (🏛️  Platform Overview)
 ├─ SECURITY.md                (🛡️  Unified Security & Audit Standard)
 ├─ NAVIGATION.md              (📚 This file)
 ├─ standards/                 (📏 Platform-wide standards)
 ├─ templates/                 (📝 Master template)
 └─ Domains/
     ├─ Core/ …
     ├─ Risk/ …
     ├─ Asset/ …
     ├─ Integration/ …
     ├─ Governance/ …
     ├─ AI/ …
     ├─ Gateway/ …
     └─ Crosscutting/ …
```
All domain folders contain:
* `README.md` (domain-overview)
* Detailed specs / guides
* `Design.md` or `domain-model.md` (architecture/specification)

---

## 2. Quick Navigation by Document Type

| Type | Path Prefix | Examples |
|------|-------------|----------|
| **Architecture Docs** | `*/Design.md`, `*/domain-model.md`, `ARCHITECTURE.md` | Core `domain-model.md`, AI `ai-architecture.md` |
| **Specifications** | `*/specification*`, `standards/`, dedicated `.md` files | `settlement-protocol.md`, `api-standards.md` |
| **Guides & Runbooks** | `*/guide*`, `*/runbook*`, `implementation-guidance/` | Crosscutting `audit.md`, Integration `monitoring/` |
| **Standards & Policies** | `standards/`, `SECURITY.md` | `api-standards.md`, `master-template.md` |
| **Templates** | `templates/` | `master-template.md` |

---

## 3. Domain-by-Domain Index

Legend: ✅ **approved** · 🟡 **in-review** · 📝 **draft**

### 3.1 Core Infrastructure
| Doc | Status |
|-----|--------|
| [README](Domains/Core/README.md) | 🟡 |
| [Domain Model](Domains/Core/domain-model.md) | 📝 |
| [Solidity Interfaces](Domains/Core/solidity-interfaces.md) | 📝 |

### 3.2 Risk, Compliance & Audit
| Doc | Status |
|-----|--------|
| [README](Domains/Risk/README.md) | 🟡 |
| [Audit System Design](Domains/Risk/audit-system-design.md) | 📝 |

### 3.3 Asset, Trading & Settlement
| Doc | Status |
|-----|--------|
| [README](Domains/Asset/README.md) | 🟡 |
| [Settlement Protocol](Domains/Asset/settlement-protocol.md) | 📝 |
| [Portfolio Optimisation](Domains/Asset/portfolio-optimization.md) | 📝 |

### 3.4 Integration, Analytics & Access
| Doc | Status |
|-----|--------|
| [README](Domains/Integration/README.md) | 🟡 |

### 3.5 Governance, Ops & Custody
| Doc | Status |
|-----|--------|
| [README](Domains/Governance/README.md) | 🟡 |

### 3.6 AI / ML Core
| Doc | Status |
|-----|--------|
| [README](Domains/AI/README.md) | 🟡 |
| [AI Architecture](Domains/AI/ai-architecture.md) | 🟡 |

### 3.7 Integration Gateway
| Doc | Status |
|-----|--------|
| [README](Domains/Gateway/README.md) | 🟡 |
| [API Standards (legacy)](Domains/Gateway/implementation-guidance/api-standards.md) | ⚠️ _deprecated_ – see unified standards |

### 3.8 Cross-Cutting Concerns
| Doc | Status |
|-----|--------|
| [Design](Domains/Crosscutting/Design.md) | 🟡 |

---

## 4. Platform Standards & Templates
| Doc | Purpose |
|-----|---------|
| [ARCHITECTURE.md](ARCHITECTURE.md) | High-level platform overview |
| [SECURITY.md](SECURITY.md) | Unified security & audit standard |
| [standards/api-standards.md](standards/api-standards.md) | Single source API standards |
| [templates/master-template.md](templates/master-template.md) | Canonical doc template |

---

## 5. Cross-References & Integration Points
* **Security** – All domains reference [`SECURITY.md`](SECURITY.md) in their Security & Compliance sections.
* **Architecture** – All domain READMEs link to [`ARCHITECTURE.md`](ARCHITECTURE.md) for context.
* **API Standards** – Gateway & Integration docs must reference [`standards/api-standards.md`](standards/api-standards.md).
* **Domain Events** – Refer to Core `domain-model.md` for canonical event definitions.

---

## 6. Status Legend & Maintenance

| Emoji | Meaning | Review Cadence |
|-------|---------|----------------|
| ✅ | Approved – production ready | 6 m |
| 🟡 | In review – pending approvals | 3 m |
| 📝 | Draft – work-in-progress | 1 m |
| ⚠️ | Deprecated – to be removed | n/a |

**Update Process**
1. Edit document → bump `version` & `last_updated`.
2. Update status indicator here.
3. Submit PR; obtain required reviewers (see `master-template.md` §5).
4. Merge & schedule next review.

> _Automated link checking & status badge generation will be added to CI in a forthcoming task (issue #P2-CI)._  

---
