---
document_type: specification
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

# VeritasVault Price Oracle DSL Specification v1.0

## 1. Introduction

This document defines the formal Domain-Specific Language (DSL) for price oracles within the VeritasVault platform. Price oracles provide authoritative price data for assets, supporting real-time valuation, risk assessment, and settlement processes.

## 2. Oracle Definition

### 2.1 Base Structure

Every price oracle is represented as a JSON object with the following structure:

```json
{
  "id": OracleIdentifier,
  "type": OracleType,
  "sources": [DataSource],
  "config": ConfigurationObject,
  "meta": MetadataObject
}
```

### 2.2 Oracle Identifier Format

```
OracleIdentifier := "oracle" "-" SourceType "-" InstrumentCode
  
Where:
- SourceType: Lowercase identifier of data source category (e.g., "001")
- InstrumentCode: Uppercase instrument code (e.g., "EURUSD")
```

## 3. Oracle Types

The `type` field specifies the oracle's implementation model:

| Type | Description |
|------|-------------|
| `aggregated` | Combines and validates data from multiple sources |
| `direct` | Publishes data from a single authoritative source |
| `consensus` | Requires validator consensus before publishing |
| `chainlink` | Integration with Chainlink oracle network |
| `hybrid` | Combines on-chain and off-chain data sources |
| `internal` | VeritasVault internal pricing model |

## 4. Data Sources

The `sources` field contains an array of data provider configurations:

```json
"sources": [
  {
    "id": String,              // Unique source identifier
    "weight": Number,          // Weight in aggregation (0.0-1.0)
    "endpoint": String,        // API endpoint or contract address
    "updateFrequency": Number, // Updates per minute
    "timeout": Number,         // Timeout in milliseconds
    "credentials": String      // Reference to secure credentials
  }
]
```

## 5. Configuration Schema

### 5.1 Common Configuration Fields

All oracles must include:

```json
"config": {
  "aggregationMethod": String,   // "median", "mean", "twap", "vwap"
  "heartbeatInterval": Number,   // Seconds between health checks
  "stalePriceThreshold": Number, // Seconds until price considered stale
  "anomalyThreshold": Number,    // % deviation to flag as anomaly
  "fallbackStrategy": String,    // Action on failure
  "minSources": Number,          // Minimum sources for valid price
  "precision": Number            // Decimal precision for price
}
```

### 5.2 Aggregated Oracle Configuration

```json
"config": {
  // Common fields +
  "outlierDetection": String,    // Method to detect outliers
  "confidenceInterval": Number,  // Required statistical confidence
  "sourceTimeout": Number,       // Milliseconds to wait for sources
  "updateStrategy": String       // Push or pull update mechanism
}
```

### 5.3 Consensus Oracle Configuration

```json
"config": {
  // Common fields +
  "consensusThreshold": Number,  // % of validators required
  "validatorSet": [String],      // List of validator identifiers
  "validationMethod": String,    // Cryptographic validation method
  "rewardStructure": Object      // Validator incentive configuration
}
```

## 6. Metadata Schema

```json
"meta": {
  "owner": String,               // Oracle operator/owner
  "created": ISO8601DateTime,    // Creation timestamp
  "version": String,             // Oracle implementation version
  "marketCategory": String,      // Market classification
  "regulatoryApprovals": [       // Regulatory certifications
    {
      "authority": String,
      "certification": String,
      "expiration": ISO8601DateTime
    }
  ],
  "documentation": String,       // URI to detailed documentation
  "sla": {                       // Service level agreement
    "uptime": Number,            // % uptime guarantee
    "latency": Number,           // Max latency in milliseconds
    "resolution": Number         // Dispute resolution time in hours
  }
}
```

## 7. Example

```json
{
  "id": "oracle-001-eurusd",
  "type": "aggregated",
  "sources": [
    {
      "id": "refinitiv-fx-1",
      "weight": 0.4,
      "endpoint": "https://api.refinitiv.com/fx/v1/eurusd",
      "updateFrequency": 60,
      "timeout": 500,
      "credentials": "credentials://refinitiv-api-key"
    },
    {
      "id": "bloomberg-fx-1",
      "weight": 0.4,
      "endpoint": "https://api.bloomberg.com/market-data/fx/eurusd",
      "updateFrequency": 60,
      "timeout": 500,
      "credentials": "credentials://bloomberg-api-key"
    },
    {
      "id": "internal-model-fx",
      "weight": 0.2,
      "endpoint": "model://fx-forecasting/eurusd",
      "updateFrequency": 30,
      "timeout": 200,
      "credentials": null
    }
  ],
  "config": {
    "aggregationMethod": "median",
    "heartbeatInterval": 15,
    "stalePriceThreshold": 300,
    "anomalyThreshold": 0.5,
    "fallbackStrategy": "last_valid_price",
    "minSources": 2,
    "precision": 6,
    "outlierDetection": "z-score",
    "confidenceInterval": 0.95,
    "sourceTimeout": 2000,
    "updateStrategy": "push"
  },
  "meta": {
    "owner": "VeritasVault Market Data Team",
    "created": "2025-01-15T08:00:00Z",
    "version": "1.2.0",
    "marketCategory": "Forex",
    "regulatoryApprovals": [
      {
        "authority": "Financial Conduct Authority",
        "certification": "Authorized Price Provider",
        "expiration": "2026-01-15T23:59:59Z"
      }
    ],
    "documentation": "https://docs.veritasvault.com/oracles/fx/eurusd",
    "sla": {
      "uptime": 99.95,
      "latency": 250,
      "resolution": 4
    }
  }
}
```

## 8. Price Data Structure

When an oracle publishes price data, it follows this structure:

```json
{
  "oracleId": String,            // Oracle identifier
  "assetId": String,             // Asset identifier
  "price": Number,               // Current price value
  "timestamp": ISO8601DateTime,  // Price timestamp
  "confidence": Number,          // Confidence score (0.0-1.0)
  "sources": Number,             // Number of sources used
  "signature": String,           // Cryptographic signature
  "metadata": {                  // Optional extended information
    "volume": Number,            // Trading volume
    "volatility": Number,        // Recent price volatility
    "spreadBps": Number          // Spread in basis points
  }
}
```

## 9. Validation Rules

1. Oracle identifiers must follow the specified format and be unique
2. Oracle type must be from the predefined vocabulary
3. At least one data source must be defined for each oracle
4. Source weights must sum to 1.0 for aggregated oracles
5. Confidence scores must be between 0.0 and 1.0
6. Heartbeat and staleness thresholds must align with SLA requirements
7. All timestamps must be valid ISO-8601 format
8. Regulatory approvals must include valid expiration dates
9. Oracle price data must be cryptographically signed

## 10. Cross-Domain Applications

Price oracles are critical infrastructure used by:
- Asset valuation services
- Trading and matching engines
- Settlement and clearing systems
- Risk management frameworks
- Compliance and reporting systems
- Analytics and monitoring services

Oracle data integrity is ensured through:
- Cryptographic signatures on all price updates
- Anomaly detection and circuit breakers
- Heartbeat monitoring and health checks
- Multi-source validation and consensus
- Audit logging of all price submissions