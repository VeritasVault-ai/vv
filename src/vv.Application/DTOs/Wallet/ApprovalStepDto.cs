using System;

namespace vv.Application.DTOs.Wallet
{
    public class ApprovalStepDto
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string ApproverRole { get; set; } = string.Empty;

        public string ApprovedBy { get; set; } = string.Empty;

        public DateTime? ApprovedAt { get; set; }

        public int Order { get; set; }

        public string Notes { get; set; } = string.Empty;
    }
}
