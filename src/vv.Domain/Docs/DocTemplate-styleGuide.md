# VeritasVault Markdown Documentation Style Guide

---

## 1. Purpose

Establish consistent, readable, and audit-ready Markdown documentation for all VeritasVault architecture and domain artifacts.

---

## 2. Structure & Sectioning

* **Use Level-1 (`#`) and Level-2 (`##`) headers** for major sections.
* **Always begin with a Level-1 title**: `# [Document Title]`.
* **Separate logical sections with horizontal rules (`---`)** for clarity.
* **Number sections if order is important; otherwise, use descriptive titles.**
* **Group related items with sub-headers (Level-3 `###`), especially for lists, rules, or technical details.**

---

## 3. Headings & Titles

* Use **Title Case** for all section and sub-section headers (capitalize principal words).
* **Avoid sentence-case or all caps** in section titles.
* Keep section titles concise but descriptive ("Business Impact", not "The Impact On Our Business").

---

## 4. Lists & Bullets

* **Use `-` for unordered lists.**
* **Use `1.`, `2.`, etc. for ordered lists** only if sequence matters.
* **Keep list items parallel in grammar** (all noun phrases, or all imperative statements).
* **Indent nested lists by two spaces** for clarity.

---

## 5. Tables

* **Use Markdown tables** for matrices, responsibilities, mapping, etc.
* **Keep column widths consistent**; break up wide tables if they become unreadable.
* **Always include header rows and horizontal separators** (with `|---|`).
* **Use short, unambiguous column titles.**

---

## 6. Code & Pseudocode

* **Use fenced code blocks (triple backticks)** for any code, interface, or configuration example.
* **Specify the language** (`typescript`, `yaml`, `solidity`, etc.) if applicable for syntax highlighting.
* **Keep code blocks under 80 characters wide** for readability.
* **Always provide a brief description above the code block** explaining its intent or context.

---

## 7. Text & Tone

* **Be direct, concise, and objective.**
* **Avoid filler, marketing, or speculative language.**
* **Prefer active voice.**
* **Define acronyms and domain terms on first use, or provide a glossary if document-specific.**
* **Use consistent terminology across documents** (see shared glossary or domain lexicon).

---

## 8. Formatting & Spacing

* **Leave one blank line before and after each header or major section.**
* **Never indent the first line of a paragraph.**
* **Do not use tab characters for alignment; use spaces if needed.**
* **Avoid excessive bold, italics, or blockquotes—use them only for emphasis or true citations.**

---

## 9. References & Linking

* **Internal links:** Use relative paths to other docs within the repository.
* **External links:** Use full URLs and provide a clear link title, not just the URL.
* **Reference sections** should always be at the end of the document.

---

## 10. Metadata & Versioning

* **Every document starts with a YAML metadata block** (see template).
* **Update `last_updated`, `version`, and `reviewers`** fields with each major revision.
* **Summarize significant changes in a change log table if included.**

---

## 11. Diagrams

* **Use Mermaid or C4 notation in fenced code blocks** for diagrams.
* **Label all diagram components and provide a brief caption or context above each diagram.**
* **Do not embed base64 images or external links for diagrams.**

---

## 12. Best Practices

* **Proofread for grammar, mechanics, and clarity before submitting.**
* **Enforce the use of the latest approved templates for each doc type.**
* **Run Markdown linters before PRs or merges (markdownlint recommended).**
* **Add a pre-submission checklist at the end of each document if required by the template.**

---

## 13. Accessibility & Readability

* **Keep line length under 80 characters where practical.**
* **Avoid color or formatting that reduces contrast or legibility.**
* **Favor bullet points, tables, and diagrams over dense paragraphs.**
* **Ensure all links, diagrams, and references are accessible to team members.**

---

## 14. Change Management

* **Increment the document version for any material change.**
* **Record significant edits and their rationale in the change log or commit message.**
* **Archive superseded documents according to policy.**

---
