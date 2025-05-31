---
document_type: interface-spec
classification: internal
status: draft
version: 0.1.0
last_updated: "2025-05-31"
applies_to: [core-infrastructure]
dependencies: [Domains/Core/domain-model.md, Domains/Crosscutting/Design.md]
reviewers: [lead-solidity-dev, secops-lead, infra-lead]
next_review: "2026-05-31"
priority: p0
---

# Core Infrastructure – On-Chain Solidity Interfaces

> Canonical specification of all smart-contract interfaces underpinning VeritasVault’s Core Infrastructure domain.

---

## 1. Overview

The Core Infrastructure layer exposes eight primary contracts that enforce consensus, indexing, randomness, gas policy, security controls, rate-limiting, multi-chain abstraction, and fork management.  
Each interface adheres to **SOLID**, **OpenZeppelin** best-practices, and emits events required for immutable audit.

| Interface | Aggregate Root (DDD) | Purpose |
|-----------|---------------------|---------|
| `IConsensusManager` | ConsensusManager | Track finality, validate blocks, handle reorgs |
| `IChainIndexer` | ChainIndexer | Persist chain history, stream events |
| `IRandomnessOracle` | RandomnessOracle | Provide VRF-based randomness proofs |
| `IGasController` | GasController | Maintain dynamic gas/fee policy |
| `ISecurityController` | SecurityController | Detect threats, trigger circuit breaker |
| `IRateLimiter` | RateLimiter | Throttle abusive actors & resources |
| `IChainAdapter` | ChainAdapter | Uniform multi-chain operations |
| `IForkManager` | ForkManager | Detect forks, coordinate safe upgrades |

All interfaces are **upgrade-safe** (UUPS compatible) and **storage-layout-agnostic**.

---

## 2. Shared Data Structures & Events

```solidity
pragma solidity ^0.8.24;

/// @dev Block header used across interfaces
struct BlockHeader {
    bytes32 parentHash;
    uint64  number;
    uint64  timestamp;
    bytes32 stateRoot;
    bytes32 txRoot;
    bytes   validatorSig;      // BLS or ECDSA aggregate
}

/// @dev Finality proof wrapper
struct FinalityProof {
    bytes32 blockHash;
    bytes   signature;         // BFT or light-client proof
}

/// @dev Verifiable Random Function proof
struct VRFProof {
    bytes32 output;
    bytes   proof;
    uint64  blockNumber;
}

/// @dev Gas policy parameters (packed for gas efficiency)
struct GasPolicy {
    uint128 minGasPrice;       // wei
    uint128 maxGasPrice;       // wei
    uint32  baseFee;           // gwei
    uint32  surgeMultiplier;   // ×1e4 e.g. 15000 = 1.5x
}

/// @dev Rate-limit config per resource
struct RateLimit {
    uint128 maxRequests;
    uint64  timeWindow;        // seconds
}

/// @dev Standardised security incident report
struct SecurityIncident {
    bytes32 id;
    address reporter;
    uint8   severity;          // 0-5 (P5 critical)
    bytes32 cause;             // keccak256(code)
    uint64  timestamp;
}
```

### Cross-Cutting Events

```solidity
event BlockFinalized(uint64 indexed number, bytes32 blockHash);
event ChainReorg(uint64 indexed depth, bytes32 newHead, bytes32 oldHead);
event RandomnessDelivered(bytes32 indexed requestId, bytes32 output);
event GasPolicyUpdated(GasPolicy newPolicy);
event RateLimitBreached(bytes32 indexed resource, address indexed actor);
event SecurityIncidentDetected(bytes32 indexed id, uint8 severity);
event ForkDetected(bytes32 indexed forkId, uint64 height);
```

---

## 3. Interface Definitions

### 3.1 `IConsensusManager`

```solidity
interface IConsensusManager {
    /// @notice Submit header + finality proof; returns true if accepted
    function submitBlock(BlockHeader calldata header, FinalityProof calldata proof) external returns (bool);

    /// @notice Returns whether block has reached finality
    function isFinalized(uint64 blockNumber) external view returns (bool);

    /// @notice Current canonical head
    function latestBlock() external view returns (BlockHeader memory);

    /// Events inherited from global section
}
```

### 3.2 `IChainIndexer`

```solidity
interface IChainIndexer {
    /// @notice Persist full transaction list for a block
    function indexTransactions(uint64 blockNumber, bytes calldata rlpEncodedTxs) external;

    /// @notice Stream events starting at `fromBlock` (pull pattern)
    function streamEvents(uint64 fromBlock) external view returns (bytes memory);

    /// @notice Get raw transaction RLP for block
    function getTransactions(uint64 blockNumber) external view returns (bytes memory);
}
```

### 3.3 `IRandomnessOracle`

```solidity
interface IRandomnessOracle {
    /// @notice Request VRF based on seed; returns requestId
    function requestRandom(bytes32 seed) external returns (bytes32);

    /// @notice Callback to deliver randomness; only callable by oracle nodes
    function deliverRandom(bytes32 requestId, VRFProof calldata proof) external;

    /// @notice Retrieve randomness result (reverts if not fulfilled)
    function getRandom(bytes32 requestId) external view returns (bytes32);
}
```

### 3.4 `IGasController`

```solidity
interface IGasController {
    /// @notice Update gas policy (onlyDAO)
    function setGasPolicy(GasPolicy calldata policy) external;

    /// @notice Fetch current policy
    function getGasPolicy() external view returns (GasPolicy memory);

    /// @notice Compute fee for calldata size
    function estimateFee(uint256 calldataSize) external view returns (uint256);
}
```

### 3.5 `ISecurityController`

```solidity
interface ISecurityController {
    /// @notice Report incident; emits SecurityIncidentDetected
    function reportIncident(SecurityIncident calldata incident) external;

    /// @notice Emergency pause protocol (circuit breaker)
    function pause() external;

    /// @notice Resume protocol after incident resolved
    function unpause() external;

    /// @notice Check paused state
    function isPaused() external view returns (bool);
}
```

### 3.6 `IRateLimiter`

```solidity
interface IRateLimiter {
    /// @notice Consume quota; returns remaining
    function consume(bytes32 resourceId, address actor, uint256 amount) external returns (uint256);

    /// @notice Configure limit (onlyGovernance)
    function setLimit(bytes32 resourceId, RateLimit calldata limit) external;

    /// @notice Get remaining quota
    function remaining(bytes32 resourceId, address actor) external view returns (uint256);
}
```

### 3.7 `IChainAdapter`

```solidity
interface IChainAdapter {
    /// @notice Submit message to target chain
    function sendMessage(uint256 targetChainId, bytes calldata payload) external returns (bytes32);

    /// @notice Verify incoming message proof
    function verifyMessage(bytes calldata proof) external view returns (bool);

    /// @notice Execute verified message
    function executeMessage(bytes calldata proof) external returns (bool);
}
```

### 3.8 `IForkManager`

```solidity
interface IForkManager {
    /// @notice Register fork candidate
    function registerFork(bytes32 forkId, uint64 height, bytes32 newHead) external;

    /// @notice Resolve fork, choose canonical head
    function resolveFork(bytes32 forkId, bytes32 chosenHead) external;

    /// @notice Get active forks
    function activeForks() external view returns (bytes32[] memory);
}
```

---

## 4. Access Control Patterns

1. **Role-Based** (`AccessControl`):  
   * `ROLE_DAO` – governance proposals (gas policy, rate limits)  
   * `ROLE_ORACLE` – `deliverRandom` privileged call  
   * `ROLE_MONITOR` – `reportIncident`, `registerFork`  

2. **Ownable / UUPS** for upgradeability (EIP-1967 slots).  
3. **EIP-712 Signatures** for gas-less actor interactions (submitBlock meta-tx).

---

## 5. Gas Optimisation Considerations

* **Packed Structs** (`GasPolicy`, `RateLimit`) – ≤ 32-byte alignment.  
* **Unchecked Increment** inside tight for-loops (`indexTransactions`).  
* **Custom Errors** instead of `require` strings for 35-40 % refund.  
* **Bitmap Permissioning** for frequent role checks.  
* **Immutable Arguments** (e.g., chainId) for proxies using EIP-1167 clones.

---

## 6. Security Patterns & Guards

| Threat | Mitigation |
|--------|------------|
| Re-entrancy | `nonReentrant` modifiers where external calls occur |
| DoS w/ block spam | `RateLimiter` + `GasController` surge multiplier |
| Randomness manipulation | VRF proofs anchored to finalized block hashes |
| Governance hijack | Time-lock + multi-sig for `ROLE_DAO` |
| Fork chaos | `IForkManager` halts trading until resolution |
| Oracle spoofing | Stake-slashing and consensus among `ROLE_ORACLE` nodes |

Circuit-breaker architecture ensures **fail-closed**: once `ISecurityController.pause()` is triggered, `whenNotPaused` modifiers across all critical contracts freeze state-changing logic.

---

## 7. Integration Examples

### 7.1 Cross-Domain Randomness

```solidity
contract AMMPool {
    IRandomnessOracle public oracle;
    mapping(bytes32 => bool) private pending;

    function requestCurveSeed() external {
        bytes32 id = oracle.requestRandom(keccak256(abi.encodePacked(block.number, address(this))));
        pending[id] = true;
    }

    function onRandomness(bytes32 requestId, bytes32 output) external {
        require(msg.sender == address(oracle));
        require(pending[requestId], "unknown id");
        pending[requestId] = false;
        _applyRandomCurve(uint256(output));
    }
}
```

### 7.2 Chain Adapter Usage

```solidity
contract BridgeProxy {
    IChainAdapter public adapter;
    bytes32 public constant ROLE_RELAYER = keccak256("ROLE_RELAYER");

    function bridge(bytes calldata payload, uint256 dstChain) external {
        adapter.sendMessage(dstChain, payload);
    }

    function onMessage(bytes calldata proof) external {
        require(adapter.verifyMessage(proof), "invalid proof");
        adapter.executeMessage(proof); // may mint wrapped asset, etc.
    }
}
```

---

## 8. Reference Implementation

A reference implementation adhering to these interfaces lives under  
`src/vv.Infrastructure/Contracts/Core/*`. All changes **MUST** preserve interface
signatures to maintain storage-layout and ABI compatibility.

---

## 9. Change Log

| Version | Date | Author | Notes |
|---------|------|--------|-------|
| 0.1.0 | 2025-05-30 | Factory Assistant | Initial interface specification |

---
