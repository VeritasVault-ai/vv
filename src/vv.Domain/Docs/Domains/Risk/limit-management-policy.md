---
document_type: policy
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

# Limit Management Policy

> Framework for establishing, monitoring, and enforcing risk limits across the platform

---

## 1. Overview

The Limit Management Policy establishes the framework for defining, implementing, monitoring, and enforcing risk limits across the VeritasVault platform. This document outlines the principles, processes, and responsibilities that ensure risk exposures remain within the organization's risk appetite.

## 2. Limit Framework Principles

### Core Principles

* **Alignment with Risk Appetite:**
  * Limits directly reflect approved risk appetite
  * Clear linkage to risk capacity
  * Consistent with strategic objectives
  * Reconcilable to capital and liquidity resources
  * Regular recalibration to current appetite

* **Comprehensive Coverage:**
  * All material risks addressed
  * All business activities included
  * Multiple limit dimensions for complex risks
  * Granular and aggregated perspectives
  * Supplementary monitoring for emerging risks

* **Effective Constraint:**
  * Binding on business activities
  * Meaningful thresholds
  * Forward-looking perspective
  * Responsive to changing conditions
  * Enforceable in systems and processes

* **Transparent and Understandable:**
  * Clear definition and calculation
  * Straightforward interpretation
  * Accessible to all stakeholders
  * Consistent application
  * Documented rationale

### Limit Types

* **Enterprise Limits:**
  * Board-approved boundaries
  * Aggregate risk constraints
  * Capital-based limits
  * Earnings-at-risk caps
  * Strategic risk parameters

* **Risk Type Limits:**
  * Market risk constraints
  * Credit exposure caps
  * Liquidity minimums
  * Operational risk thresholds
  * Model risk boundaries

* **Business/Activity Limits:**
  * Business unit allocations
  * Product type constraints
  * Geographic restrictions
  * Client segment parameters
  * Activity-specific caps

* **Transaction Limits:**
  * Individual trade size
  * Cumulative transaction volume
  * Trading frequency
  * Position aging limits
  * Approval thresholds

### Limit Hierarchy

![Limit Hierarchy](../Architecture/diagrams/limit-hierarchy.svg)

* **Level 1: Enterprise Limits**
  * Set by Board of Directors
  * Monitored by Risk Committee
  * Reviewed annually
  * Require Board approval for changes
  * Reported quarterly to Board

* **Level 2: Risk Type Limits**
  * Set by Risk Management Committee
  * Monitored by CRO
  * Reviewed semi-annually
  * Require CEO/CRO approval for changes
  * Reported monthly to Executive Committee

* **Level 3: Business Unit Limits**
  * Set by Business and Risk Leadership
  * Monitored by Business Risk Managers
  * Reviewed quarterly
  * Require Business Head and Risk approval
  * Reported weekly to Business Management

* **Level 4: Desk/Individual Limits**
  * Set by Business Unit Management
  * Monitored by Supervisors
  * Reviewed monthly
  * Require Unit Head approval for changes
  * Reported daily to Business Unit Management

## 3. Limit Design and Calibration

### Limit Design Methodology

1. **Risk Identification:**
   * Identify relevant risk types
   * Determine appropriate metrics
   * Define measurement methodology
   * Establish monitoring frequency
   * Determine necessary granularity

2. **Quantification Approach:**
   * Historical analysis of exposures
   * Stress testing scenarios
   * Peer benchmarking
   * Regulatory considerations
   * Expert judgment

3. **Limit Structure Definition:**
   * Hard limits vs. soft thresholds
   * Absolute vs. relative measures
   * Static vs. dynamic limits
   * Single-point vs. tiered structure
   * Temporary vs. permanent limits

4. **Calibration Process:**
   * Alignment to risk appetite
   * Business volume consideration
   * Market condition assessment
   * Capacity analysis
   * Scenario impact testing

### Calibration Factors

* **Risk Appetite Alignment:**
  * Board-approved risk tolerance
  * Strategic risk positioning
  * Risk-return objectives
  * Competitive considerations
  * Stakeholder expectations

* **Business Requirements:**
  * Strategic business plans
  * Expected trading volumes
  * Client activity forecasts
  * Product growth projections
  * Seasonal patterns

* **Environmental Factors:**
  * Market conditions
  * Regulatory environment
  * Competitive landscape
  * Macroeconomic outlook
  * Systemic risk indicators

* **Operational Capabilities:**
  * Monitoring capabilities
  * System constraints
  * Staff expertise
  * Process maturity
  * Control effectiveness

### Limit Review and Recalibration

* **Regular Review Cycle:**
  * Annual comprehensive review
  * Quarterly assessment
  * Monthly utilization analysis
  * Ad-hoc reviews for material changes
  * Post-incident evaluation

* **Recalibration Triggers:**
  * Persistent high utilization (>85%)
  * Significant utilization volatility
  * Business strategy changes
  * Market condition shifts
  * Risk appetite adjustments
  * Regulatory changes

* **Recalibration Process:**
  * Analysis of utilization patterns
  * Business needs assessment
  * Risk impact evaluation
  * Approval at appropriate level
  * Documentation of rationale
  * Communication and implementation

## 4. Limit Monitoring and Control

### Monitoring Framework

* **Monitoring Frequency:**
  * Real-time: Trading limits, critical thresholds
  * Intraday: Market risk, position limits
  * Daily: Credit exposures, liquidity metrics
  * Weekly: Concentration risks, trend analysis
  * Monthly: Strategic limits, complex aggregations

* **Utilization Measurement:**
  * Current utilization percentage
  * Peak utilization tracking
  * Trend analysis
  * Volatility assessment
  * Forward-looking projection

* **Alert Thresholds:**
  * Early warning: 70% utilization
  * Elevated attention: 85% utilization
  * Pre-breach action: 95% utilization
  * Breach notification: >100% utilization
  * Severe breach: >120% utilization

### Control Mechanisms

* **Preventative Controls:**
  * Pre-trade limit verification
  * Automated approval workflows
  * System-enforced hard limits
  * Training and awareness
  * Clear policies and procedures

* **Detective Controls:**
  * Automated limit monitoring
  * Utilization reporting
  * Trend analysis
  * Independent verification
  * Reconciliation processes

* **Corrective Controls:**
  * Breach response procedures
  * Automated trading restrictions
  * Position reduction requirements
  * Enhanced approval processes
  * Escalation protocols

### Escalation Procedures

* **Level 1: Awareness**
  * Trigger: 85% utilization
  * Notification to desk/business head
  * Documentation of awareness
  * Increased monitoring frequency
  * Preliminary action plan if sustained

* **Level 2: Action Required**
  * Trigger: 95% utilization or temporary breach
  * Notification to business and risk management
  * Required action plan
  * Daily monitoring and reporting
  * Preventative measures implementation

* **Level 3: Formal Breach**
  * Trigger: Limit breach beyond tolerance
  * Formal notification to senior management
  * Required remediation plan with timeline
  * Potential temporary activity restrictions
  * Root cause analysis

* **Level 4: Severe Breach**
  * Trigger: Significant breach or prolonged breach
  * Executive committee notification
  * Immediate risk reduction required
  * Potential business activity suspension
  * Comprehensive review and control enhancement

## 5. Limit Breach Management

### Breach Classification

* **Technical Breach:**
  * Temporary, system-related
  * Data or timing issues
  * No actual risk increase
  * Self-correcting
  * Documentation required

* **Active Breach:**
  * Actual risk increase
  * Operational or judgment error
  * Requires active management
  * Time-bound remediation
  * Root cause analysis required

* **Passive Breach:**
  * Market movement driven
  * No active position increase
  * External factor driven
  * Management plan required
  * Monitoring until resolution

* **Authorized Breach:**
  * Pre-approved temporary excess
  * Specific circumstances
  * Time-bound authorization
  * Enhanced monitoring
  * Senior approval documented

### Breach Response Process

1. **Identification and Notification:**
   * Breach detection
   * Initial classification
   * Required notifications
   * Preliminary assessment
   * Immediate safeguards

2. **Assessment and Documentation:**
   * Breach magnitude determination
   * Root cause analysis
   * Impact evaluation
   * Documentation of circumstances
   * Classification finalization

3. **Remediation Planning:**
   * Risk mitigation options
   * Timeframe establishment
   * Resource requirements
   * Approval of approach
   * Communication plan

4. **Implementation and Monitoring:**
   * Remediation execution
   * Progress tracking
   * Regular reporting
   * Effectiveness assessment
   * Plan adjustment if needed

5. **Resolution and Review:**
   * Breach closure verification
   * Lessons learned documentation
   * Control enhancement recommendations
   * Limit structure review
   * Prevent recurrence measures

### Temporary Limit Adjustments

* **Temporary Increase Process:**
  * Business justification requirement
  * Risk assessment
  * Time limitation (typically <30 days)
  * Approval requirements based on:
    * Magnitude of increase
    * Duration of adjustment
    * Risk type and significance
    * Recent breach history
  * Enhanced monitoring requirement

* **Emergency Protocol:**
  * Defined emergency situations
  * Rapid approval process
  * Immediate notification requirements
  * Limited duration (typically <5 days)
  * Post-approval documentation
  * Formal review within 24 hours

* **Documentation Requirements:**
  * Business justification
  * Risk assessment
  * Approval evidence
  * Time limitation
  * Monitoring plan
  * Resolution approach

## 6. Governance and Oversight

### Roles and Responsibilities

* **Board of Directors:**
  * Approve enterprise risk appetite
  * Review material limit breaches
  * Oversee limit framework effectiveness
  * Challenge limit structure and calibration
  * Approve Level 1 limits

* **Risk Committee:**
  * Review and recommend risk appetite
  * Approve Level 2 limits
  * Monitor aggregate limit utilization
  * Review significant limit breaches
  * Approve material limit framework changes

* **Chief Risk Officer:**
  * Develop and maintain limit framework
  * Propose limit structures and levels
  * Oversee limit monitoring process
  * Report on limit utilization and breaches
  * Approve Level 3 limits

* **Business Management:**
  * Operate within approved limits
  * Request appropriate limits
  * Manage limit utilization
  * Address limit breaches
  * Propose limit adjustments

* **Risk Management Function:**
  * Design limit structures
  * Monitor limit utilization
  * Analyze breach patterns
  * Report limit status
  * Recommend framework improvements

* **Internal Audit:**
  * Independently assess framework
  * Verify limit implementation
  * Validate monitoring processes
  * Review breach management
  * Evaluate overall effectiveness

### Approval Authorities

| Limit Type | New Limit | Increase | Decrease | Temporary Adjustment |
|------------|-----------|----------|----------|----------------------|
| Level 1 | Board | Board | Risk Committee | CEO & CRO |
| Level 2 | Risk Committee | Risk Committee | CRO | CRO |
| Level 3 | CRO | CRO | Risk Management | Business Head & Risk Management |
| Level 4 | Business Head & Risk Management | Business Head & Risk Management | Business Management | Business Management |

### Reporting Requirements

* **Daily Reporting:**
  * Limit utilization dashboard
  * Threshold breaches
  * New limit breaches
  * Remediation status updates
  * Significant utilization changes

* **Weekly Reporting:**
  * Utilization trends
  * Breach summary
  * Remediation progress
  * Temporary adjustments
  * Emerging concerns

* **Monthly Reporting:**
  * Comprehensive limit status
  * Breach pattern analysis
  * Remediation effectiveness
  * Limit adjustment recommendations
  * Stress testing against limits

* **Quarterly Reporting:**
  * Limit framework effectiveness
  * Structural recommendations
  * Utilization pattern analysis
  * Strategic alignment assessment
  * Framework enhancement proposals

## 7. Limit Documentation and Systems

### Documentation Standards

* **Limit Definition Documentation:**
  * Precise metric definition
  * Calculation methodology
  * Data sources and quality requirements
  * Monitoring frequency
  * Reporting requirements
  * Breach management process

* **Limit Register:**
  * Comprehensive inventory of all limits
  * Hierarchical organization
  * Current approved values
  * Approval history
  * Related limits mapping
  * Owner and monitor assignment

* **Approval Documentation:**
  * Approval request details
  * Risk assessment summary
  * Business justification
  * Approval evidence
  * Implementation verification
  * Review schedule

### Systems and Tools

* **Limit Management System:**
  * Centralized limit repository
  * Automated monitoring
  * Alert generation
  * Workflow management
  * Reporting capabilities
  * Audit trail

* **Integration Points:**
  * Trading systems
  * Risk calculation engines
  * Position management systems
  * Market data services
  * Reporting platforms
  * Approval workflows

* **System Capabilities:**
  * Real-time monitoring
  * Customizable dashboards
  * Flexible reporting
  * Automated notifications
  * Trend analysis
  * What-if scenario analysis

## 8. Implementation Guidelines

### Implementation Priorities

1. **Foundation:**
   * Core market risk limits
   * Essential credit limits
   * Basic liquidity thresholds
   * Critical operational limits
   * Fundamental monitoring capabilities

2. **Enhancement:**
   * Comprehensive limit structure
   * Advanced monitoring tools
   * Integrated reporting
   * Automated workflows
   * Breach management automation

3. **Optimization:**
   * Dynamic limit adjustments
   * Predictive utilization analytics
   * ML-enhanced monitoring
   * Strategic limit optimization
   * Advanced visualization

### Best Practices

* **Limit Design:**
  * Keep limit structures simple and understandable
  * Ensure direct connection to risk appetite
  * Focus on material risks
  * Balance constraint and business enablement
  * Design for extreme conditions

* **Monitoring and Control:**
  * Automate routine monitoring
  * Establish clear escalation paths
  * Focus attention on exceptions
  * Track historical patterns
  * Ensure system redundancy

* **Governance:**
  * Maintain clear documentation
  * Establish unambiguous accountability
  * Review regularly for relevance
  * Learn from breach experience
  * Validate effectiveness periodically

## 9. References

* [Risk Management Overview](./README.md)
* [Risk Architecture](./risk-architecture.md)
* [Portfolio Risk Monitoring](./portfolio-risk-monitoring.md)
* [Stress Testing Guidelines](./stress-testing-guidelines.md)
* [Compliance Framework](./compliance-framework.md)
* [System Architecture](../Architecture/system-architecture.md)

---

## 10. Document Control

* **Owner:** Head of Risk Controls
* **Last Updated:** 2025-05-29
* **Status:** Draft

* **Change Log:**

  | Version | Date | Author | Changes | Reviewers |
  |---------|------|--------|---------|-----------|
  | 1.0.0 | 2025-05-29 | Head of Risk Controls | Initial document creation | CRO, Compliance, Business Heads |

* **Review Schedule:** Quarterly or with significant framework changes