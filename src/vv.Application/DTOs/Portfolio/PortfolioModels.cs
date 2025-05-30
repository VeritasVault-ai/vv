using System;
using System.Collections.Generic;

namespace vv.Application.DTOs.Portfolio.BlackLitterman
{
    public class AIGeneratedViewDto
    {
        public string ViewId { get; set; }
        public string ModelId { get; set; } // Which AI model generated this view
        public string ModelVersion { get; set; }
        public DateTime GeneratedAt { get; set; }
        public List<ViewAssetWeightDto> AssetWeights { get; set; } = new();
        public decimal ExpectedReturn { get; set; }
        public decimal ModelConfidence { get; set; } // 0-1, AI's own assessment of confidence
        public string RationaleText { get; set; }
        public Dictionary<string, decimal> FactorContributions { get; set; } = new(); // Factors affecting this view
        public List<string> DataSourcesUsed { get; set; } = new();
        public Dictionary<string, object> ModelParameters { get; set; } = new();
        public decimal BacktestAccuracy { get; set; } // Historical accuracy of similar predictions
        public List<AlternativeViewDto> AlternativeViews { get; set; } = new();
        public string BlockchainVerificationHash { get; set; }
        public string IpfsContentId { get; set; } // For storing detailed model output
    }

    public class AlternativeViewDto
    {
        public string AlternativeId { get; set; }
        public List<ViewAssetWeightDto> AssetWeights { get; set; } = new();
        public decimal ExpectedReturn { get; set; }
        public decimal Probability { get; set; } // How likely this alternative is
        public string Scenario { get; set; } // Description of market conditions for this view
    }

    public class BlockchainModelVerificationDto
    {
        public string VerificationId { get; set; }
        public string ModelRunId { get; set; }
        public DateTime ExecutedAt { get; set; }
        public string InputDataHash { get; set; } // Hash of all input parameters
        public string OutputDataHash { get; set; } // Hash of results
        public string BlockchainNetworkUsed { get; set; }
        public string TransactionHash { get; set; }
        public int BlockNumber { get; set; }
        public string BlockExplorerUrl { get; set; }
        public string SmartContractAddress { get; set; }
        public string VerifierAddress { get; set; }
        public string ExecutorAddress { get; set; }
        public string VerificationStatus { get; set; } // Pending, Verified, Failed
        public DateTime VerifiedAt { get; set; }
        public List<ModelInputReferenceDto> InputReferences { get; set; } = new();
        public string ProofOfExecutionHash { get; set; }
    }

    public class ModelInputReferenceDto
    {
        public string ReferenceId { get; set; }
        public string DataType { get; set; } // Market Data, Views, Parameters, etc.
        public string StorageType { get; set; } // IPFS, Blockchain, Database
        public string StorageReference { get; set; } // IPFS hash, tx hash, or database ID
        public string DataDescription { get; set; }
        public DateTime StoredAt { get; set; }
        public bool IsPubliclyAccessible { get; set; }
        public string AccessMethod { get; set; } // API endpoint, query, etc.
    }

    public class ModelExecutionDto
    {
        public string ExecutionId { get; set; }
        public string ModelType { get; set; } // "BlackLitterman", "RiskParity", etc.
        public string ExecutedBy { get; set; } // User or system ID
        public DateTime StartedAt { get; set; }
        public DateTime CompletedAt { get; set; }
        public string Status { get; set; } // Running, Completed, Failed
        public string ErrorMessage { get; set; }
        public Dictionary<string, string> InputParameterHashes { get; set; } = new();
        public string ResultHash { get; set; }
        public string ExecutionEnvironment { get; set; } // Cloud, Local, etc.
        public Dictionary<string, object> PerformanceMetrics { get; set; } = new(); // Execution time, resource usage
        public string ModelVersion { get; set; }
        public string CodeRepositoryReference { get; set; } // Git commit hash
        public BlockchainModelVerificationDto Verification { get; set; }
    }

    public class ModelRegistryDto
    {
        public string ModelId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Type { get; set; } // BlackLitterman, ML Forecasting, etc.
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string Status { get; set; } // Draft, Testing, Production, Deprecated
        public List<string> SupportedAssetClasses { get; set; } = new();
        public Dictionary<string, object> DefaultParameters { get; set; } = new();
        public decimal BacktestSharpeRatio { get; set; }
        public decimal BacktestAlpha { get; set; }
        public string ValidationMethodology { get; set; }
        public List<string> ApprovedForClientTypes { get; set; } = new(); // Retail, Institutional, etc.
        public string BlockchainRegistryAddress { get; set; }
        public string RegistrationTransactionHash { get; set; }
        public string SourceCodeReference { get; set; } // IPFS hash or GitHub reference
    }
}