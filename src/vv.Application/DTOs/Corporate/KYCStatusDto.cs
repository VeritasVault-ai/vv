using System;

namespace vv.Application.DTOs.Corporate
{
    public class KYCStatusDto
    {
        public string Id { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime LastUpdated { get; set; }

        public string UpdatedBy { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;
    }
}
