using System;
using System.Collections.Generic;

namespace vv.Application.DTOs.Compliance
{
    public class ComplianceReportDto
    {
        public DateTime GeneratedAt { get; set; }
        public string PoolId { get; set; }
        public List<RegulatoryCheckDto> RegulatoryChecks { get; set; } = new();
        public List<AMLAlertDto> AmlAlerts { get; set; } = new();
        public List<TransactionMonitoringDto> FlaggedTransactions { get; set; } = new();
        public Dictionary<string, bool> JurisdictionalCompliance { get; set; } = new(); // Region → compliant?
        public List<LicenseDto> ActiveLicenses { get; set; } = new();
        public DateTime NextAuditDue { get; set; }
        public string ComplianceOfficer { get; set; }
        public bool IsCompliant { get; set; }
    }

    public class RegulatoryCheckDto
    {
        public string CheckId { get; set; }
        public string Regulation { get; set; } // GDPR, AML, KYC, etc.
        public string Jurisdiction { get; set; }
        public string Status { get; set; } // Passed, Failed, Pending
        public DateTime CheckedAt { get; set; }
        public string Description { get; set; }
        public List<string> EvidenceUrls { get; set; } = new();
        public string ResponsiblePerson { get; set; }
        public DateTime NextCheckDue { get; set; }
    }

    public class AMLAlertDto
    {
        public string AlertId { get; set; }
        public string Type { get; set; } // Suspicious Transaction, High Risk, etc.
        public string ClientId { get; set; }
        public string TransactionId { get; set; }
        public DateTime DetectedAt { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Asset { get; set; }
        public string Status { get; set; } // Open, Investigating, Closed, Reported
        public string AssignedTo { get; set; }
        public bool ReportedToAuthorities { get; set; }
    }

    public class TransactionMonitoringDto
    {
        public string MonitoringId { get; set; }
        public string TransactionId { get; set; }
        public string Flag { get; set; } // Size, Frequency, Pattern, etc.
        public string Description { get; set; }
        public decimal RiskScore { get; set; }
        public bool RequiresReview { get; set; }
        public string ReviewedBy { get; set; }
        public DateTime ReviewedAt { get; set; }
        public string Resolution { get; set; }
    }

    public class LicenseDto
    {
        public string LicenseId { get; set; }
        public string Type { get; set; } // MSB, Virtual Asset Provider, etc.
        public string Jurisdiction { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; } // Active, Pending, Expired
        public string IssuingAuthority { get; set; }
        public List<string> CoveredActivities { get; set; } = new();
        public string DocumentUrl { get; set; }
    }

    public class KYCStatusDto
    {
        public string DocumentType { get; set; }
        public string Status { get; set; } // Verified, Pending, Rejected
        public DateTime SubmissionDate { get; set; }
        public DateTime VerificationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string VerifiedBy { get; set; }
        public List<string> Notes { get; set; } = new();
    }

    public class TransactionScreeningDto
    {
        public string ScreeningId { get; set; }
        public string TransactionId { get; set; }
        public DateTime ScreenedAt { get; set; }
        public bool IsClean { get; set; }
        public List<SanctionHitDto> SanctionHits { get; set; } = new();
        public List<RiskIndicatorDto> RiskIndicators { get; set; } = new();
        public string BlockchainAnalysisProvider { get; set; }
        public string ScreeningResult { get; set; }
        public decimal RiskScore { get; set; }
        public string Action { get; set; } // Allowed, Blocked, Pending Review
    }

    public class SanctionHitDto
    {
        public string SanctionListName { get; set; }
        public string EntityName { get; set; }
        public string EntityType { get; set; } // Person, Organization, Address
        public string MatchType { get; set; } // Exact, Partial, Fuzzy
        public decimal MatchScore { get; set; } // 0-100
        public string SanctioningBody { get; set; } // OFAC, UN, EU, etc.
        public DateTime ListUpdatedDate { get; set; }
    }

    public class RiskIndicatorDto
    {
        public string IndicatorType { get; set; }
        public string Description { get; set; }
        public decimal Severity { get; set; } // 1-10
        public string DataSource { get; set; }
        public DateTime DetectedAt { get; set; }
        public List<string> RelatedEntities { get; set; } = new();
    }
}