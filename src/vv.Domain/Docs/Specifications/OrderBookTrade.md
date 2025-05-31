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

# VeritasVault OrderBook & Trade DSL Specification v1.0

## 1. Introduction

This document defines the formal Domain-Specific Language (DSL) for order books and trading activities within the VeritasVault platform. It specifies the structure, states, and behaviors of orders, matching processes, and executed trades.

## 2. Order Definition

### 2.1 Base Structure

Every order is represented as a JSON object with the following structure:

```json
{
  "id": OrderIdentifier,
  "type": OrderType,
  "side": OrderSide,
  "status": OrderStatus,
  "asset": AssetIdentifier,
  "quantity": QuantityObject,
  "price": PriceObject,
  "constraints": ConstraintObject,
  "meta": MetadataObject
}
```

### 2.2 Order Identifier Format

```
OrderIdentifier := "order" "-" WalletId "-" Timestamp "-" Sequence

Where:
- WalletId: Alphanumeric identifier of originating wallet (truncated)
- Timestamp: ISO-8601 UTC timestamp (compact format)
- Sequence: Numeric sequence for collision avoidance
```

## 3. Order Types & Sides

### 3.1 Order Types

| Type | Description |
|------|-------------|
| `limit` | Execute at specified price or better |
| `market` | Execute at best available price |
| `stop` | Becomes market when price reaches trigger |
| `stopLimit` | Becomes limit when price reaches trigger |
| `trailingStop` | Stop that adjusts with market movement |
| `iceberg` | Shows only portion of total quantity |
| `fok` | Fill or Kill - must fill entire order or cancel |
| `ioc` | Immediate or Cancel - fill what's possible immediately |
| `twap` | Time-Weighted Average Price algorithm |
| `vwap` | Volume-Weighted Average Price algorithm |

### 3.2 Order Sides

| Side | Description |
|------|-------------|
| `buy` | Buy order (bid) |
| `sell` | Sell order (ask) |

### 3.3 Order Status

| Status | Description |
|--------|-------------|
| `pending` | Order received but not yet active |
| `active` | Order placed in order book |
| `partiallyFilled` | Order partially matched |
| `filled` | Order completely matched |
| `cancelled` | Order cancelled before completion |
| `rejected` | Order rejected due to validation |
| `expired` | Order expired due to time constraints |
| `triggered` | Stop/conditional order has triggered |
| `suspended` | Order temporarily suspended |

## 4. Order Components

### 4.1 Quantity Object

```json
"quantity": {
  "original": Number,           // Original order quantity
  "displayed": Number,          // Visible quantity (for iceberg)
  "filled": Number,             // Quantity already filled
  "remaining": Number,          // Quantity still to fill
  "minimumFill": Number         // Minimum fill quantity
}
```

### 4.2 Price Object

```json
"price": {
  "limit": Number,              // Limit price (null for market)
  "stop": Number,               // Stop/trigger price (if applicable)
  "trailingOffset": Number,     // Trailing stop offset (if applicable)
  "averageFilled": Number,      // Average fill price of executions
  "slippageTolerance": Number   // Maximum allowed slippage percentage
}
```

### 4.3 Constraint Object

```json
"constraints": {
  "timeInForce": String,        // "GTC", "GTD", "IOC", "FOK"
  "expireTime": ISO8601DateTime, // Expiration time (for GTD)
  "selfTradePrevention": String, // "cancel-newest", "cancel-oldest", "decrement"
  "postOnly": Boolean,          // Must be a maker, not taker
  "allowedVenues": [String],    // Permitted trading venues
  "regionConstraints": [String], // Region restrictions (if any)
  "timeConstraints": {          // Time-based constraints
    "timezone": String,
    "allowedWindows": [
      {
        "start": TimeString,
        "end": TimeString,
        "days": [String]
      }
    ]
  }
}
```

### 4.4 Metadata Object

```json
"meta": {
  "created": ISO8601DateTime,    // Order creation time
  "updated": ISO8601DateTime,    // Last order update time
  "source": String,              // Order origin (API, UI, etc.)
  "clientOrderId": String,       // Client-assigned order ID
  "userId": String,              // User identifier
  "walletId": String,            // Wallet identifier
  "strategyId": String,          // Trading strategy reference
  "complianceChecks": [          // Compliance verifications
    {
      "type": String,
      "status": String,
      "timestamp": ISO8601DateTime
    }
  ],
  "riskChecks": [                // Risk control checks
    {
      "type": String,
      "result": String,
      "timestamp": ISO8601DateTime
    }
  ],
  "tags": [String]               // Custom order tags
}
```

## 5. Order Book Structure

```json
{
  "assetId": AssetIdentifier,
  "bids": [OrderSummary],
  "asks": [OrderSummary],
  "lastUpdated": ISO8601DateTime,
  "sequence": Number,
  "stats": {
    "bestBid": Number,
    "bestAsk": Number,
    "midPrice": Number,
    "lastTradePrice": Number,
    "lastTradeSize": Number,
    "volume24h": Number,
    "priceChange24h": Number,
    "lowPrice24h": Number,
    "highPrice24h": Number,
    "numOrders": Number,
    "depth": Number
  }
}
```

### 5.1 Order Summary

```json
{
  "price": Number,
  "quantity": Number,
  "numOrders": Number,
  "timestamp": ISO8601DateTime
}
```

## 6. Trade Definition

### 6.1 Base Structure

Every executed trade is represented as:

```json
{
  "id": TradeIdentifier,
  "asset": AssetIdentifier,
  "buyOrderId": OrderIdentifier,
  "sellOrderId": OrderIdentifier,
  "price": Number,
  "quantity": Number,
  "timestamp": ISO8601DateTime,
  "settlementStatus": SettlementStatus,
  "proofs": [ProofObject],
  "meta": MetadataObject
}
```

### 6.2 Trade Identifier Format

```
TradeIdentifier := "trade" "-" AssetCode "-" Timestamp "-" Sequence

Where:
- AssetCode: Short code for the traded asset 
- Timestamp: ISO-8601 UTC timestamp (compact format)
- Sequence: Numeric sequence for collision avoidance
```

### 6.3 Settlement Status

| Status | Description |
|--------|-------------|
| `pending` | Trade executed but not yet settled |
| `settling` | Settlement in progress |
| `settled` | Trade fully settled |
| `failed` | Settlement failed |
| `disputed` | Settlement under dispute |

### 6.4 Proof Object

```json
{
  "type": String,               // "execution", "settlement", "compliance"
  "hash": String,               // Cryptographic hash/signature
  "timestamp": ISO8601DateTime, // When proof was generated
  "verifierIds": [String]       // Entities verifying the proof
}
```

### 6.5 Trade Metadata

```json
"meta": {
  "venue": String,              // Trading venue identifier
  "fees": [                     // Fee breakdown
    {
      "type": String,
      "amount": Number,
      "currency": String,
      "recipient": String
    }
  ],
  "makerOrderId": OrderIdentifier, // Which order was the maker
  "takerOrderId": OrderIdentifier, // Which order was the taker
  "isInterbook": Boolean,       // Cross-book trade
  "settlementRef": String,      // Reference to settlement record
  "eventChain": [String],       // Linked event identifiers
  "complianceStatus": {
    "status": String,           // Compliance verification status
    "verifications": [          // Compliance checks performed
      {
        "type": String,
        "status": String,
        "timestamp": ISO8601DateTime
      }
    ]
  }
}
```

## 7. Example: Limit Order

```json
{
  "id": "order-xf9b2c5-20250525T101502Z-1234",
  "type": "limit",
  "side": "buy",
  "status": "active",
  "asset": "fxspot-EURUSD-2025-05-25T10:15Z-EU",
  "quantity": {
    "original": 10000,
    "displayed": 10000,
    "filled": 0,
    "remaining": 10000,
    "minimumFill": 1000
  },
  "price": {
    "limit": 1.1205,
    "stop": null,
    "trailingOffset": null,
    "averageFilled": null,
    "slippageTolerance": 0.001
  },
  "constraints": {
    "timeInForce": "GTD",
    "expireTime": "2025-05-25T16:00:00Z",
    "selfTradePrevention": "cancel-newest",
    "postOnly": true,
    "allowedVenues": ["primary", "internal"],
    "regionConstraints": ["EU"],
    "timeConstraints": {
      "timezone": "CET",
      "allowedWindows": [
        {
          "start": "08:00:00",
          "end": "17:00:00",
          "days": ["MON", "TUE", "WED", "THU", "FRI"]
        }
      ]
    }
  },
  "meta": {
    "created": "2025-05-25T10:15:02Z",
    "updated": "2025-05-25T10:15:02Z",
    "source": "api",
    "clientOrderId": "client-order-ref-123456",
    "userId": "user-42789ad5e",
    "walletId": "wallet-f9b2c5e7d",
    "strategyId": "strat-eur-trend-1",
    "complianceChecks": [
      {
        "type": "pre-trade",
        "status": "passed",
        "timestamp": "2025-05-25T10:15:01Z"
      }
    ],
    "riskChecks": [
      {
        "type": "position-limit",
        "result": "approved",
        "timestamp": "2025-05-25T10:15:01Z"
      },
      {
        "type": "exposure",
        "result": "approved",
        "timestamp": "2025-05-25T10:15:01Z"
      }
    ],
    "tags": ["api-client", "institutional"]
  }
}
```

## 8. Example: Executed Trade

```json
{
  "id": "trade-EURUSD-20250525T101530Z-5678",
  "asset": "fxspot-EURUSD-2025-05-25T10:15Z-EU",
  "buyOrderId": "order-xf9b2c5-20250525T101502Z-1234",
  "sellOrderId": "order-a7d8e3-20250525T101450Z-5678",
  "price": 1.1204,
  "quantity": 5000,
  "timestamp": "2025-05-25T10:15:30Z",
  "settlementStatus": "settled",
  "proofs": [
    {
      "type": "execution",
      "hash": "0x8f41b8934b8c9b33c8d5c39c297389284af8d929c178b5987d802f9a7f34b87c",
      "timestamp": "2025-05-25T10:15:30Z",
      "verifierIds": ["matching-engine-1", "audit-service"]
    },
    {
      "type": "settlement",
      "hash": "0x3a7d8c98f6e5d4c2b1a9f8e7d6c5b4a3f2e1d0c9b8a7d6e5f4c3b2a1d0c9b8a7",
      "timestamp": "2025-05-25T10:15:35Z",
      "verifierIds": ["settlement-service", "audit-service"]
    }
  ],
  "meta": {
    "venue": "primary",
    "fees": [
      {
        "type": "maker",
        "amount": 0.05,
        "currency": "USD",
        "recipient": "exchange"
      },
      {
        "type": "taker",
        "amount": 0.15,
        "currency": "USD",
        "recipient": "exchange"
      }
    ],
    "makerOrderId": "order-xf9b2c5-20250525T101502Z-1234",
    "takerOrderId": "order-a7d8e3-20250525T101450Z-5678",
    "isInterbook": false,
    "settlementRef": "settlement-20250525-f9e8d7c6",
    "eventChain": [
      "event-order-received-a7d8e3",
      "event-order-received-xf9b2c5",
      "event-orders-matched-5678",
      "event-trade-executed-5678",
      "event-settlement-completed-5678"
    ],
    "complianceStatus": {
      "status": "verified",
      "verifications": [
        {
          "type": "trade-surveillance",
          "status": "passed",
          "timestamp": "2025-05-25T10:15:31Z"
        },
        {
          "type": "position-monitoring",
          "status": "passed",
          "timestamp": "2025-05-25T10:15:32Z"
        }
      ]
    }
  }
}
```

## 9. Validation Rules

1. Order identifiers must follow the specified format and be unique
2. Order type, side, and status must be from the predefined vocabularies
3. Asset identifier must reference a valid asset in the system
4. Quantities must be positive and conform to asset minimum/maximum trade sizes
5. Prices must be within valid ranges and tick size constraints for the asset
6. Time constraints must align with asset trading window restrictions
7. Orders with region constraints must respect asset region permissions
8. Trade executions must reference valid buy and sell orders
9. Settlement status transitions must follow the defined workflow
10. Proofs must include valid cryptographic signatures and timestamps

## 10. Cross-Domain Applications

The OrderBook & Trade DSL interfaces with:
- Asset registry for instrument validation
- Price oracles for market data and pricing
- Settlement engine for trade clearing
- Risk management for pre/post-trade checks
- Compliance framework for regulatory verification
- Audit system for cryptographic proofs
- Analytics engine for market data and insights

Circuit breakers and safety mechanisms include:
- Pre-trade compliance and risk validation
- Price band monitoring and protection
- Order size and frequency limitations
- Market surveillance and anomaly detection
- Cryptographic verification of all trades