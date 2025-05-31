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

# Alerting Framework

> Alert definition, classification, and management system

---

## 1. Overview

The Alerting Framework provides a structured approach to defining, triggering, and managing alerts across the VeritasVault platform. This document outlines the architecture, classification system, and implementation guidelines for effective alerting.

## 2. Alert Classification

### Severity Levels

* **Critical (P1):**
  * Immediate response required (SLA: 15 minutes)
  * Service-wide impact or data integrity issue
  * Examples: Bridge failure, system-wide outage, security breach

* **High (P2):**
  * Urgent response needed (SLA: 1 hour)
  * Partial service impact or major component failure
  * Examples: API degradation, failed oracle updates, message delivery delays

* **Medium (P3):**
  * Scheduled response (SLA: 8 hours)
  * Degraded performance or non-critical component issue
  * Examples: Increased latency, minor capacity concerns, non-critical errors

* **Low (P4):**
  * Routine investigation (SLA: 24 hours)
  * No immediate impact, but requires attention
  * Examples: Warning thresholds, resource optimization opportunities

* **Info:**
  * Awareness only, no action required
  * Monitoring of normal operations and state changes
  * Examples: Deployments, maintenance, scaling events

### Alert Categories

* **Availability:** Service or component accessibility
* **Performance:** Speed, throughput, or capacity issues
* **Error:** Failure conditions or exception rates
* **Security:** Potential security incidents
* **Compliance:** Regulatory or policy violations
* **Capacity:** Resource utilization concerns
* **Anomaly:** Deviation from normal patterns

## 3. Alert Definition

### Alert Configuration

```typescript
interface AlertDefinition {
  id: string;
  name: string;
  description: string;
  severity: 'critical' | 'high' | 'medium' | 'low' | 'info';
  category: AlertCategory;
  condition: {
    metric: string;
    operator: 'gt' | 'lt' | 'eq' | 'neq' | 'gte' | 'lte';
    threshold: number;
    evaluationPeriod: number; // seconds
    evaluationCycles: number; // consecutive periods
  };
  notificationChannels: NotificationChannel[];
  runbook: string; // URL to response procedure
  autoRemediation?: AutoRemediationAction;
  silenceConditions?: SilenceCondition[];
}
```

### Alert Correlation

* **Grouping Mechanism:**
  * Component-based correlation
  * Time-based clustering
  * Causal relationship mapping
  * Symptom vs. root cause identification

* **Noise Reduction:**
  * Alert de-duplication
  * Alert suppression during maintenance
  * Flapping detection and dampening
  * Dependent alert filtering

## 4. Notification & Escalation

### Notification Channels

* **Standard Channels:**
  * Email notifications
  * SMS messages
  * Push notifications to mobile devices
  * Integration with chat platforms (Slack, Teams)

* **Escalation Channels:**
  * Phone calls for critical issues
  * Paging systems (PagerDuty, OpsGenie)
  * Escalation to management
  * Emergency broadcast system

### Escalation Policies

* **Time-Based Escalation:**
  * Initial notification to primary on-call
  * Escalation if unacknowledged within timeframe
  * Secondary responder notification
  * Management escalation for extended incidents

* **Severity-Based Routing:**
  * Critical alerts to senior engineers
  * Multiple responders for high-severity issues
  * Appropriate team based on component
  * Follow-the-sun handoff for global coverage

## 5. Alert Management

### Alert Lifecycle

* **Detection:** Condition evaluation against thresholds
* **Triggering:** Alert created and initial notification sent
* **Acknowledgment:** Responder acknowledges receipt
* **Investigation:** Problem diagnosis and troubleshooting
* **Resolution:** Problem fixed and systems recovered
* **Closure:** Alert closed with resolution notes
* **Review:** Post-mortem analysis for improvement

### Alert Tools & Interfaces

* **Alert Console:**
  * Real-time view of active alerts
  * Filtering by severity, component, status
  * Grouping and correlation visualization
  * Action buttons for common responses

* **Mobile Interface:**
  * Push notifications with critical details
  * Quick acknowledgment capability
  * Basic investigation tools
  * Escalation request functionality

## 6. Implementation Guidelines

### Alert Design Principles

* **Signal vs. Noise:**
  * Alert on symptoms that matter to users
  * Avoid alerting on non-actionable conditions
  * Consolidate related alerts to reduce noise
  * Tune thresholds to minimize false positives

* **Actionability:**
  * Every alert must have a clear action path
  * Link directly to relevant runbooks
  * Include sufficient context for diagnosis
  * Provide tools for common resolution steps

* **Prioritization:**
  * Clear distinction between severity levels
  * Focus attention on highest-impact issues
  * Time-to-acknowledge proportional to severity
  * Balance response time with responder well-being

### Common Alert Definitions

* **Bridge Health:**
  * Message success rate < 99.9% over 5 minutes
  * Message processing time > 30 seconds
  * Failed validator consensus > 3 consecutive attempts
  * Pending message queue > 100 for 15 minutes

* **Oracle Performance:**
  * Price feed delay > 5 minutes
  * Price deviation > 5% from consensus
  * Failed updates > 3 consecutive attempts
  * Source availability < 80%

* **System Performance:**
  * API latency > 500ms (95th percentile)
  * Error rate > 1% over 5 minutes
  * CPU utilization > 85% for 10 minutes
  * Memory usage > 90% for 5 minutes

## 7. References & Resources

### Internal Documentation

* [Incident Response](./incident-response.md)
* [Operational Runbooks](./operational-runbooks.md)
* [Monitoring Architecture](./monitoring-architecture.md)

### External References

* [Google SRE Alerting Philosophy](https://sre.google/sre-book/monitoring-distributed-systems/)
* [Prometheus Alerting Best Practices](https://prometheus.io/docs/practices/alerting/)
* [PagerDuty Incident Response](https://response.pagerduty.com/)

---

## 8. Document Control

* **Owner:** Alerting Systems Lead
* **Last Updated:** 2025-05-29
* **Status:** Draft