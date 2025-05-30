# VeritasVault Master Documentation Template  
_Canonical reference for **all** Markdown documents in `vv.Domain/Docs`_

---

## 0. About This Template
Use this file as the **single source of truth** for creating new documentation across the repository.  
It **replaces** the older `DocTemplate-pt1.md`, `DocTemplate-pt2.md`, and `DocTemplate-styleGuide.md`.

---

## 1. YAML Metadata Header

Every document **MUST** begin with a fenced YAML block:

```yaml
---
document_type: <architecture|domain-overview|specification|runbook|guide|policy>
classification: <internal|public|confidential>
status: <draft|review|approved|archived>
version: 1.0.0
last_updated: YYYY-MM-DD
applies_to: [comma,separated,domains]
dependencies: [relative/paths/to/related/docs]
reviewers: [role|github_handle]
next_review: YYYY-MM-DD
priority: <p0|p1|p2|p3>
---
```

Guidelines  
* Increment `version` for any material change.  
* `classification` drives publication rules (internal vs OSS).  
* `next_review` ensures living-docs; set ≤ 6 months in the future.

---

## 2. Standard Document Structure

Section | Purpose | Mandatory?
------- | ------- | ----------
1 **Executive Summary** | Business & technical impact in ≤ 200 words. | ✅
2 **Domain/Module Overview** | Scope, responsibilities, boundaries. | ✅
3 **Responsibilities & Boundaries** | In-scope / out-of-scope lists. | ✅
4 **Detailed Design / Specification** | Models, flows, diagrams, algorithms. | ✅
5 **Implementation & Operations** | Phases, deployment, runtime, runbooks. | ✅  
6 **Security & Compliance** | Reference `../../SECURITY.md`, list domain specifics. | ⚠️ if relevant  
7 **Integration Points** | Inter-domain or external interfaces. | ⚠️  
8 **References** | RFCs, papers, external URLs. | ⚠️  
9 **Change Log** | Table of versions & edits (see §5). | ✅

> Omit optional sections only if they genuinely don’t apply.

---

## 3. Style Guide (Concise Rules)

* **Headings**: Title Case; use `#` for doc title, `##` for top sections.  
* **Tone**: Direct, objective, no marketing fluff.  
* **Lists**: `-` for unordered, `1.` for ordered; keep grammar parallel.  
* **Tables**: Always include header row; align pipes.  
* **Code Blocks**: Triple back-ticks with language tag. Keep ≤ 80 chars/line.  
* **Diagrams**: Mermaid or C4 inside code fences; add caption above.  
* **Links**: Relative paths for repo docs; descriptive text for external URLs.  
* **Line Length**: Soft-wrap ≤ 100 chars for readability.  
* **Metadata**: Update `last_updated` & `version` on each merge to main.  

---

## 4. Formatting Standards

Aspect | Rule
------ | ----
Spacing | Blank line **before & after** each `#`/`##` header.
Horizontal Rules | Use `---` to visually separate major sections.
Bold / Italic | Use sparingly for emphasis **not** decoration.
Images | Prefer diagrams in code fences; avoid embedded base64.
Accessibility | Avoid colour-only cues; ensure high contrast.
File Names | **kebab-case.md** – no spaces, no CamelCase.
Folder Depth | Max depth = 3 for discoverability.

---

## 5. Change Management Requirements

Every document must end with a **Change Log** table:

| Version | Date | Author | Changes | Reviewers |
|---------|------|--------|---------|-----------|
| 1.0.0 | 2025-05-30 | alice | Initial draft | @lead-architect |

Process  
1. Open PR referencing doc path and Jira/issue.  
2. At least **one domain lead + one security reviewer** must approve.  
3. Merge → bump `version`, update `last_updated`, append row to log.  
4. Archive superseded docs in `/archive/` folder.

---

## 6. Template Usage Examples

### 6.1 Architecture Document (excerpt)

```yaml
---
document_type: architecture
classification: internal
status: draft
version: 0.1.0
last_updated: 2025-05-30
applies_to: [core-infrastructure]
reviewers: [lead-architect, secops-lead]
next_review: 2025-08-01
priority: p0
---
```

# Core Infrastructure – Layer Architecture  
*(Executive Summary …)*

### Aggregate Roots  
| Name | Responsibility |
|------|----------------|
| ConsensusManager | Tracks finality, resolves forks |

*(continue with sections 2-9)*

---

### 6.2 Domain-Overview Document (excerpt)

```yaml
---
document_type: domain-overview
classification: internal
status: approved
version: 1.2.0
last_updated: 2025-04-15
applies_to: [ai-ml-core]
reviewers: [ai-lead, compliance-officer]
next_review: 2025-10-15
priority: p1
---
```

# AI / ML Core – Domain Overview  
*(Executive Summary …)*

*(sections as per structure)*

---

### 6.3 Specification Document (excerpt)

```yaml
---
document_type: specification
classification: internal
status: review
version: 0.4.0
last_updated: 2025-05-15
applies_to: [asset-trading-settlement]
reviewers: [trading-lead, secops-lead]
next_review: 2025-06-30
priority: p1
---
```

# Settlement Protocol Specification  
*(Detailed Design / Algorithms …)*

---

_End of Template_  
