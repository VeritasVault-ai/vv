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

# Asset Specification

> Comprehensive Data Model for Digital Assets in VeritasVault

---

## Overview

This document specifies the canonical representation of assets within the VeritasVault platform, including data structures, metadata requirements, lifecycle states, and behavioral interfaces.

## Asset Data Model

### Core Asset Structure

```typescript
interface Asset {
  id: AssetId;                   // Unique system identifier
  externalIds: ExternalId[];     // External reference identifiers (ISIN, CUSIP, etc.)
  assetType: AssetType;          // Classification of asset
  metadata: AssetMetadata;       // Descriptive information
  state: AssetState;             // Current lifecycle state
  issuance: IssuanceDetails;     // Creation and distribution information
  behaviors: AssetBehavior[];    // Associated runtime behaviors
  permissions: AssetPermission[]; // Access and operation rights
}
```

### Asset Identifier System

Each asset has a unique internal identifier and may have multiple external identifiers:

```typescript
type AssetId = string;  // UUID format

interface ExternalId {
  type: ExternalIdType;  // ISIN, CUSIP, TICKER, etc.
  value: string;         // The actual identifier
  issuer: string;        // Authority that issued the identifier
  validationStatus: ValidationStatus;  // Verified, Pending, Failed
}

enum ExternalIdType {
  ISIN,
  CUSIP,
  TICKER,
  SEDOL,
  LEI,
  FIGI,
  CUSTOM
}
```

### Asset Types and Classification

Assets are categorized into types and subtypes with associated classification data:

```typescript
enum AssetType {
  EQUITY,
  FIXED_INCOME,
  DERIVATIVE,
  DIGITAL_ASSET,
  ALTERNATIVE,
  COMPOSITE
}

interface AssetClassification {
  assetType: AssetType;
  assetSubtype: string;  // More specific categorization
  sector: string;        // Industry sector
  geography: string;     // Geographic classification
  riskClass: RiskClass;  // Risk categorization
  customTags: string[];  // Additional classification tags
}
```

### Asset Metadata

Comprehensive descriptive information about the asset:

```typescript
interface AssetMetadata {
  name: string;           // Human-readable name
  description: string;    // Detailed description
  classification: AssetClassification;  // Type classification
  documentation: DocumentReference[];   // Links to supporting docs
  legalTerms: LegalTerms;               // Legal information
  technicalDetails: TechnicalDetails;   // Implementation details
  displayInfo: DisplayInformation;      // UI representation
  customAttributes: {[key: string]: any};  // Extensible attributes
}

interface DocumentReference {
  title: string;         // Document title
  description: string;   // Brief description
  documentType: string;  // Type of document
  location: URI;         // Location of document
  hash: string;          // Content hash for verification
  version: string;       // Document version
}
```

### Asset State and Lifecycle

Assets transition through defined lifecycle states:

```typescript
enum AssetState {
  DRAFT,         // Initial creation, incomplete
  PENDING,       // Awaiting approval/activation
  ACTIVE,        // Fully operational
  SUSPENDED,     // Temporarily unavailable
  TERMINATED,    // No longer active
  ARCHIVED       // Historical record only
}

interface StateTransition {
  fromState: AssetState;
  toState: AssetState;
  conditions: TransitionCondition[];
  requiredApprovals: ApprovalRequirement[];
  postTransitionActions: ActionReference[];
}
```

### Issuance Details

Information about the asset's creation and distribution:

```typescript
interface IssuanceDetails {
  issuer: EntityReference;         // Issuing entity
  issuanceDate: Date;              // Date of issuance
  initialSupply: Quantity;         // Initial amount issued
  currentSupply: Quantity;         // Current outstanding amount
  maxSupply: Quantity | null;      // Maximum possible supply (null if unlimited)
  denomination: Denomination;      // Smallest divisible unit
  issuanceTerms: IssuanceTerms;    // Terms of the issuance
  distributionMethod: DistributionMethod;  // How the asset was distributed
}
```

### Asset Behaviors

Runtime behaviors associated with different asset types:

```typescript
interface AssetBehavior {
  behaviorType: BehaviorType;  // Type of behavior
  implementation: URI;         // Location of implementation
  parameters: {[key: string]: any};  // Configuration parameters
  enabled: boolean;            // Whether behavior is active
}

enum BehaviorType {
  DIVIDEND_DISTRIBUTION,
  INTEREST_PAYMENT,
  MATURITY_PROCESSING,
  CORPORATE_ACTION,
  OPTION_EXERCISE,
  PRICE_ORACLE,
  TRANSFER_RESTRICTION,
  COMPLIANCE_CHECK,
  CUSTOM
}
```

## Asset Type-Specific Extensions

### Equity-Specific Properties

```typescript
interface EquityProperties {
  shareClass: string;         // Class of shares
  votingRights: boolean;      // Whether shares have voting rights
  dividendPolicy: string;     // Dividend distribution policy
  marketCap: Money;           // Market capitalization
  outstandingShares: number;  // Number of shares in circulation
  parValue: Money | null;     // Nominal value per share
}
```

### Fixed Income Properties

```typescript
interface FixedIncomeProperties {
  principal: Money;               // Face value
  couponRate: Percentage;         // Interest rate
  couponType: CouponType;         // Fixed, Floating, Zero
  couponFrequency: Frequency;     // Payment frequency
  maturityDate: Date;             // Date of maturity
  yieldToMaturity: Percentage;    // Current YTM
  duration: number;               // Modified duration
  creditRating: CreditRating;     // Bond rating
  callableDetails: CallableDetails | null;  // Call provisions
}
```

### Derivative Properties

```typescript
interface DerivativeProperties {
  derivativeType: DerivativeType;    // Option, Future, Swap, etc.
  underlyingAssets: AssetReference[];  // Reference assets
  strikePrice: Money | null;         // For options/certain derivatives
  expirationDate: Date | null;       // Expiration date if applicable
  settlementType: SettlementType;    // Cash or Physical
  contractSize: number;              // Size of one contract
  marginRequirements: MarginRequirements | null;  // If applicable
}
```

## Validation and Integrity Rules

### Asset Validation Rules

Assets must satisfy these integrity constraints:

1. Every asset must have a unique AssetId
2. Asset name and description must not be empty
3. Asset must have at least one ExternalId for production use
4. IssuanceDate must be in the past
5. If maxSupply is specified, currentSupply must not exceed it
6. All referenced entities must exist and be valid
7. All DocumentReferences must have valid hashes
8. Asset state transitions must follow the defined state machine
9. Type-specific properties must be present for the declared AssetType

### Metadata Validation

Asset metadata must satisfy these requirements:

1. Required fields based on assetType must be present
2. DocumentReferences must have valid URIs
3. Custom attributes must follow the defined schema
4. DisplayInformation must include all required fields for UI rendering
5. Classification must be consistent with assetType

## Asset Lifecycle Events

### Core Lifecycle Events

```typescript
enum AssetLifecycleEvent {
  CREATED,                // Initial creation
  METADATA_UPDATED,       // Information changed
  STATE_CHANGED,          // Lifecycle state transition
  SUPPLY_CHANGED,         // Outstanding amount changed
  BEHAVIOR_ADDED,         // New behavior attached
  BEHAVIOR_REMOVED,       // Behavior detached
  BEHAVIOR_UPDATED,       // Behavior configuration changed
  PERMISSION_CHANGED,     // Access rights modified
  EXTERNAL_ID_ADDED,      // New identifier added
  EXTERNAL_ID_REMOVED,    // Identifier removed
  EXTERNAL_ID_VALIDATED,  // Identifier verified
  DOCUMENTATION_ADDED,    // New documentation linked
  DOCUMENTATION_REMOVED   // Documentation unlinked
}
```

### Event Handling Requirements

1. All asset changes must generate appropriate lifecycle events
2. Events must be cryptographically signed
3. Events must be chronologically ordered
4. Events must be immutably stored
5. Events must capture all relevant change information
6. Events must identify the actor that triggered the change
7. Events must be available for audit and replay

## Implementation Guidelines

### Security Requirements

1. All asset operations must be permission-controlled
2. Sensitive metadata must be encrypted
3. Asset state transitions must require appropriate authorization
4. Supply changes must follow defined issuance rules
5. External ID validation must use secure verification mechanisms

### Extension Framework

The asset model can be extended through:

1. Custom asset types via the type registry
2. Custom behaviors via the behavior framework
3. Custom metadata via the extensible attributes system
4. Custom validation rules via the validation framework
5. Custom lifecycle events via the event system

### Performance Considerations

1. Asset lookup by ID should be O(1)
2. Asset query by properties should be optimized for common patterns
3. Asset metadata should support lazy loading for large documents
4. State transitions should be atomic
5. Event generation should be asynchronous where possible

## API Integration

Assets expose the following core interfaces:

```solidity
interface IAsset {
    // Basic information retrieval
    function getId() external view returns (bytes32);
    function getMetadata() external view returns (AssetMetadata memory);
    function getState() external view returns (AssetState);
    
    // Lifecycle management
    function transitionState(AssetState newState, bytes calldata authProof) external returns (bool);
    function updateMetadata(AssetMetadata calldata metadata, bytes calldata authProof) external returns (bool);
    
    // Behavior interaction
    function performAction(bytes32 behaviorId, bytes calldata actionData) external returns (bytes memory);
    
    // Transfer validation
    function validateTransfer(address from, address to, uint256 amount) external view returns (bool, string memory);
}
```