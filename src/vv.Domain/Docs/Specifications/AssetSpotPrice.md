# VeritasVault Asset DSL Specification v1.0

## 1. Introduction

This document defines the formal Domain-Specific Language (DSL) for representing financial assets within the VeritasVault platform, with emphasis on time and region-adjustable spot price assets.

## 2. Asset Definition

### 2.1 Base Structure

Every asset is represented as a JSON object with the following structure:

```json
{
  "id": AssetIdentifier,
  "traits": [Trait],
  "meta": MetadataObject
}
```

### 2.2 Asset Identifier Format

```
AssetIdentifier := InstrumentType "-" InstrumentCode "-" Timestamp "-" RegionCode

Where:
- InstrumentType: Lowercase identifier of asset category (e.g., "fxspot")
- InstrumentCode: Uppercase instrument code (e.g., "EURUSD")
- Timestamp: ISO-8601 UTC timestamp (e.g., "2025-05-25T10:15Z")
- RegionCode: Uppercase region code (e.g., "EU")
```

## 3. Asset Traits

Traits are predefined capability flags that enable specific platform behaviors:

| Trait | Description |
|-------|-------------|
| `onChain` | Asset is represented on distributed ledger |
| `divisible` | Asset can be fractionally owned or traded |
| `fungible` | Asset units are interchangeable |
| `priceFeedEligible` | Asset can receive external price feeds |
| `spotAsset` | Asset represents current market pricing |
| `fiatPair` | Asset is a currency exchange pair |
| `realtimeValuation` | Asset supports continuous price updates |
| `regionAdjustable` | Asset behavior varies by geographic region |
| `timeAdjustable` | Asset behavior varies by time parameters |

## 4. Metadata Schema

### 4.1 Common Fields

All assets must include:

```json
"meta": {
  "market": String,              // Market category
  "created": ISO8601DateTime,    // Creation timestamp
  "instrumentType": String       // Instrument classification
}
```

### 4.2 FX Spot Asset Fields

```json
"meta": {
  // Common fields +
  "baseCurrency": String,        // ISO 4217 base currency code
  "quoteCurrency": String,       // ISO 4217 quote currency code
  "precision": Number,           // Decimal places for price
  "priceOracleId": String,       // Oracle identifier
  "venue": String,               // Trading venue
  "provider": String,            // Data provider
  "regulatoryRegion": String,    // Primary regulatory jurisdiction
  "minTradeSize": Number,        // Minimum allowed trade size
  "maxTradeSize": Number,        // Maximum allowed trade size
  "liquidity": String,           // Liquidity descriptor
  "tickSize": Number,            // Minimum price movement
  "reportingStandard": String    // Applicable reporting standard
}
```

### 4.3 Region-Adjustable Fields

Assets with the `regionAdjustable` trait must include:

```json
"meta": {
  // Other fields +
  "region": String,              // Current operational region
  "allowedRegions": [            // List of permitted regions
    String
  ]
}
```

### 4.4 Time-Adjustable Fields

Assets with the `timeAdjustable` trait must include:

```json
"meta": {
  // Other fields +
  "timezone": String,            // Time zone identifier
  "allowedTimeWindows": [        // Permitted trading windows
    {
      "start": TimeString,       // Window start time (HH:MM:SS)
      "end": TimeString,         // Window end time (HH:MM:SS)
      "days": [String],          // Days of week (MON,TUE,WED,THU,FRI,SAT,SUN)
      "timezone": String         // Window-specific timezone
    }
  ],
  "dstAdjustment": Boolean       // Whether DST affects time windows
}
```

## 5. Example

```json
{
  "id": "fxspot-EURUSD-2025-05-25T10:15Z-EU",
  "traits": [
    "onChain",
    "divisible",
    "fungible",
    "priceFeedEligible",
    "spotAsset",
    "fiatPair",
    "realtimeValuation",
    "regionAdjustable",
    "timeAdjustable"
  ],
  "meta": {
    "baseCurrency": "EUR",
    "quoteCurrency": "USD",
    "precision": 6,
    "market": "Forex",
    "priceOracleId": "oracle-001-eurusd",
    "venue": "Aggregated",
    "provider": "Refinitiv",
    "regulatoryRegion": "EU",
    "instrumentType": "FX_SPOT",
    "minTradeSize": 1000,
    "maxTradeSize": 10000000,
    "liquidity": "high",
    "tickSize": 0.00001,
    "reportingStandard": "MiFID II",
    "created": "2025-05-25T10:00:00Z",
    "region": "EU",
    "timezone": "CET",
    "allowedRegions": ["EU", "UK", "APAC"],
    "allowedTimeWindows": [
      {
        "start": "08:00:00",
        "end": "17:00:00",
        "days": ["MON", "TUE", "WED", "THU", "FRI"],
        "timezone": "CET"
      }
    ],
    "dstAdjustment": true
  }
}
```

## 6. Validation Rules

1. Asset identifiers must follow the specified format and be unique
2. All traits must be from the predefined vocabulary
3. Required metadata fields must be present based on asset type and traits
4. String values must conform to their respective formats (e.g., ISO timestamps)
5. Numeric values must be within valid ranges for their fields
6. Time windows must not overlap within the same timezone and days

## 7. Cross-Domain Applications

This DSL enables consistent asset representation across:
- Trading systems
- Risk management platforms
- Compliance frameworks
- Analytics engines
- Disaster recovery procedures
- Access control systems

The `regionAdjustable` and `timeAdjustable` traits allow systems to:
- Dynamically adjust behavior based on current time and location
- Apply different trading rules by region
- Implement region-specific compliance requirements
- Control settlement timing across different markets
- Support multi-region disaster recovery