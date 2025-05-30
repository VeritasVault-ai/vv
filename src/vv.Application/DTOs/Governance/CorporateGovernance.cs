using System;
using System.Collections.Generic;

namespace vv.Application.DTOs.Governance.Corporate
{
    public class CorporateProposalWorkflowDto
    {
        public string WorkflowId { get; set; }
        public string ClientId { get; set; }
        public string ProposalId { get; set; }
        public string Status { get; set; } // Draft, PendingApproval, Approved, Rejected, Submitted, Withdrawn
        public List<ApprovalStepDto> ApprovalSteps { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public string CurrentApprover { get; set; }
        public int CurrentStep { get; set; }
        public DateTime? CompletedAt { get; set; }
        public List<string> Comments { get; set; } = new();
        public List<DocumentDto> SupportingDocuments { get; set; } = new();
        public bool IsExpedited { get; set; }
    }

    public class ApprovalStepDto
    {
        public int StepNumber { get; set; }
        public string ApproverRole { get; set; }
        public string ApproverUserId { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected, Skipped
        public DateTime? ActionDate { get; set; }
        public string Comments { get; set; }
        public bool IsRequired { get; set; }
        public int RequiredSignatureCount { get; set; } // For multi-sig approval
        public List<SignatureDto> Signatures { get; set; } = new();
    }

    public class SignatureDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public DateTime SignedAt { get; set; }
        public string IpAddress { get; set; }
        public string SignatureHash { get; set; } // Digital signature verification
    }

    public class DocumentDto
    {
        public string DocumentId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileSizeBytes { get; set; }
        public DateTime UploadedAt { get; set; }
        public string UploadedBy { get; set; }
        public string StoragePath { get; set; }
        public string DocumentHash { get; set; } // For integrity checking
        public bool IsConfidential { get; set; }
    }

    public class VoteAutomationRuleDto
    {
        public string RuleId { get; set; }
        public string ClientId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<VoteTriggerDto> Triggers { get; set; } = new();
        public VoteActionDto Action { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastTriggeredAt { get; set; }
        public int TriggerCount { get; set; }
        public string LastResult { get; set; }
    }

    public class VoteTriggerDto
    {
        public string TriggerId { get; set; }
        public string Type { get; set; } // ProposalTag, ProposerAddress, TokenThreshold, etc.
        public string Condition { get; set; } // Contains, Equals, GreaterThan, etc.
        public string Value { get; set; }
        public bool IsRequired { get; set; }
    }

    public class VoteActionDto
    {
        public string ActionType { get; set; } // Vote, Notify, Delegate, Abstain
        public string VoteDecision { get; set; } // For, Against, Abstain
        public decimal VotingPowerPercentage { get; set; } // % of available power to use
        public string NotifyUserIds { get; set; } // Comma-separated user IDs to notify
        public string DelegateToAddress { get; set; } // Address to delegate to
        public string CustomMessage { get; set; } // For notifications
        public bool RequiresReview { get; set; }
    }

    public class GovernanceReportDto
    {
        public string ReportId { get; set; }
        public string ClientId { get; set; }
        public string ReportType { get; set; } // Monthly, Quarterly, Annual, Ad-hoc
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public DateTime GeneratedAt { get; set; }
        public int ProposalsParticipatedIn { get; set; }
        public int ProposalsCreated { get; set; }
        public decimal AverageParticipationRate { get; set; }
        public Dictionary<string, int> VoteBreakdown { get; set; } = new(); // For/Against/Abstain count
        public List<string> SignificantProposals { get; set; } = new();
        public Dictionary<string, decimal> GovernanceTokensHeld { get; set; } = new();
        public decimal GovernanceInfluenceScore { get; set; } // 0-100
        public List<GovernanceActionDto> RecommendedActions { get; set; } = new();
        public string ReportUrl { get; set; }
    }

    public class GovernanceActionDto
    {
        public string ActionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; } // High, Medium, Low
        public string Category { get; set; }
        public string Benefit { get; set; }
        public decimal EstimatedImpact { get; set; } // 1-10
        public List<string> PrerequisiteActions { get; set; } = new();
    }
}