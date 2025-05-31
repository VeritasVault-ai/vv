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

# Communication Templates

> Standardized formats for incident-related communications

---

## 1. Overview

This document provides standardized templates for all incident-related communications within the VeritasVault platform. Consistent communication formats ensure clarity, completeness, and appropriate tone for different stakeholders and incident phases.

## 2. Communication Principles

* **Clarity** - Use clear, concise language without technical jargon
* **Transparency** - Be honest about what's known and unknown
* **Timeliness** - Communicate early and provide regular updates
* **Audience-Appropriate** - Tailor message to the audience's technical level
* **Action-Oriented** - Include next steps and expectations
* **Consistency** - Use consistent terminology and formatting

## 3. Initial Incident Notification

### Purpose
To inform stakeholders of a new incident and set expectations for updates.

### Template

```
INCIDENT ALERT: [Severity] - [Short Description]

Time Detected: [Timestamp]
Affected Systems: [Component List]
Current Impact: [Service/User Impact]

Initial Assessment: [Brief analysis]
Actions Underway: [Current response measures]

Next Update Expected: [Timestamp]
Incident Coordinator: [Name/Contact]
```

### Example

```
INCIDENT ALERT: P1-CRITICAL - Bridge Transaction Failure

Time Detected: 2025-05-29 08:42 UTC
Affected Systems: Cross-Chain Bridge, Transaction Processor, Oracle Service
Current Impact: All cross-chain transactions failing with timeout errors

Initial Assessment: Bridge validator consensus failure due to network partition
Actions Underway: Restarting validator nodes, implementing backup consensus path

Next Update Expected: 2025-05-29 09:15 UTC
Incident Coordinator: Jane Smith (j.smith@veritasvault.com, #incident-123)
```

## 4. Status Update Template

### Purpose
To provide regular updates during an ongoing incident.

### Template

```
INCIDENT UPDATE: [Incident ID] - [Timestamp]

Current Status: [Investigating/Mitigating/Resolving]
Impact Changes: [Any change in affected services]

Progress: [Actions completed since last update]
Challenges: [Ongoing issues or blockers]
Next Steps: [Planned actions]

Estimated Resolution: [Timeframe if known]
Next Update Expected: [Timestamp]
```

### Example

```
INCIDENT UPDATE: INC-2025-042 - 2025-05-29 09:15 UTC

Current Status: Mitigating
Impact Changes: Partial service restoration - high value transactions now processing

Progress: 
- Identified network partition between US and EU validator nodes
- Restored connectivity for 4 of 7 validator nodes
- Implemented reduced consensus threshold temporarily

Challenges:
- 3 validators still unreachable
- Backlog of pending transactions growing

Next Steps:
- Deploying additional validator nodes
- Implementing transaction prioritization for backlog

Estimated Resolution: 2025-05-29 11:00 UTC
Next Update Expected: 2025-05-29 09:45 UTC
```

## 5. Resolution Notification

### Purpose
To inform stakeholders that an incident has been resolved.

### Template

```
INCIDENT RESOLVED: [Incident ID] - [Short Description]

Resolution Time: [Timestamp]
Total Duration: [Time from detection to resolution]
Final Impact Assessment: [Service/User impact summary]

Root Cause: [Brief explanation]
Resolution Actions: [How the issue was fixed]

Follow-up: [Post-incident review timing]
Preventive Measures: [Immediate safeguards implemented]

Incident Owner: [Name/Contact for questions]
```

### Example

```
INCIDENT RESOLVED: INC-2025-042 - Bridge Transaction Failure

Resolution Time: 2025-05-29 10:37 UTC
Total Duration: 1 hour 55 minutes
Final Impact Assessment: 
- 4,327 cross-chain transactions delayed
- No data or financial loss occurred
- All transactions now processed successfully

Root Cause: Network ACL configuration error after deployment blocked validator intercommunication

Resolution Actions:
- Corrected network ACL rules
- Restored all validator nodes
- Processed transaction backlog with prioritization
- Verified end-to-end transaction flow

Follow-up: Post-incident review scheduled for 2025-05-30 14:00 UTC
Preventive Measures:
- Added pre-deployment ACL verification check
- Implemented validator connectivity monitoring
- Updated deployment runbook with verification steps

Incident Owner: Jane Smith (j.smith@veritasvault.com)
```

## 6. Stakeholder-Specific Templates

### Executive Update

```
EXECUTIVE SUMMARY: [Incident ID]

Business Impact: [Brief non-technical impact description]
Status: [Active/Resolved], [Duration if resolved]
User Facing: [Yes/No/Limited]

Key Points:
- [1-3 bullet points on business impact]
- [Risk assessment]
- [Timeline for resolution]

Business Decisions Required:
- [Any immediate decisions needed]
```

### Customer Communication

```
SERVICE STATUS UPDATE

Current Status: [Service operational/degraded/unavailable]
Issue Start Time: [When users might have first noticed]
Expected Resolution: [Timeframe in user-friendly terms]

Details:
[Non-technical description of the issue]

Recommendation:
[What users should do, if anything]

Next Update: [When to expect more information]
```

### Technical Team Handoff

```
TECHNICAL HANDOFF: [Incident ID]

Current System State:
- [Component status]
- [Metrics/thresholds to watch]
- [Recent changes made]

Known Issues:
- [Ongoing problems]
- [Temporary workarounds in place]

Pending Actions:
- [Required follow-ups]
- [Scheduled activities]

Reference Information:
- [Links to dashboards]
- [Logs location]
- [Command history]
```

## 7. Communication Channels

| Channel | Audience | When to Use | Template |
|---------|----------|-------------|----------|
| PagerDuty | On-call team | Initial alert | Initial Notification (abbreviated) |
| Email | Internal stakeholders | All updates | Full templates |
| Slack | Response team | Real-time coordination | Custom format |
| Status Page | External users | Public impact | Customer Communication |
| Phone | Executives | Critical incidents | Executive Summary + verbal |
| Knowledge Base | All teams | Post-resolution | Resolution + learnings |

## 8. References

* [Incident Response Overview](../incident-response.md)
* [Roles & Responsibilities](./roles-responsibilities.md)
* [Incident Lifecycle](./incident-lifecycle.md)

---

## 9. Document Control

* **Owner:** Communications Lead
* **Last Updated:** 2025-05-29
* **Status:** Draft