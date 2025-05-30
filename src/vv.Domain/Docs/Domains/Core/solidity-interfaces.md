# Solidity Interface Details

> Key Contract Specifications for Core Infrastructure

---

## Overview

This document specifies the Solidity interfaces for all key contracts in the VeritasVault Core Infrastructure. These interfaces define the primary interaction points for both internal system components and external integrations.

## Key Contracts

### ConsensusManager

```solidity
interface IConsensusManager {
    function finalizeBlock(uint256 blockNumber) external;
    function detectReorg(uint256 fromBlock) external returns (bool);
    function submitFinalityProof(uint256 blockNumber, bytes calldata proof) external;
}
```

The ConsensusManager handles block finality and chain reorganizations. It validates transaction inclusion and provides the foundation for consistent state management.

### ChainIndexer

```solidity
interface IChainIndexer {
    function indexEvent(uint256 blockNumber, bytes calldata eventData) external;
    function createSnapshot(uint256 blockNumber) external;
    function restoreSnapshot(uint256 snapshotId) external;
    function replayEvents(uint256 fromBlock, uint256 toBlock) external view returns (bytes[] memory);
}
```

The ChainIndexer provides event indexing, audit capabilities, and state replay functionality. It maintains versioned state snapshots and handles reorgs.

### RandomnessOracle

```solidity
interface IRandomnessOracle {
    function getRandom(bytes32 context) external view returns (bytes32);
    function submitVRFProof(bytes32 requestId, bytes calldata proof) external;
}
```

The RandomnessOracle provides verifiable, unbiased randomness through secure entropy aggregation and verifiable random functions (VRF).

### GasController

```solidity
interface IGasController {
    function setGasPolicy(bytes32 policyId, bytes calldata policyData) external;
    function getGasPrice() external view returns (uint256);
    function emergencyAdjust(uint256 newGasPrice) external;
}
```

The GasController manages network economics and gas policies, including dynamic fee markets and emergency adjustments to prevent economic attacks.

### SecurityController

```solidity
interface ISecurityController {
    function checkAccess(address account, bytes32 resource) external view returns (bool);
    function emergencyPause(bytes32 reason) external;
    function validateOperation(bytes32 opId) external view returns (bool);
}
```

The SecurityController enforces security policies, manages role-based access control, and can trigger emergency responses.

### RateLimiter

```solidity
interface IRateLimiter {
    function incrementUsage(address user, bytes32 resource) external;
    function checkLimit(address user, bytes32 resource) external view returns (bool);
    function getUsage(address user, bytes32 resource) external view returns (uint256);
}
```

The RateLimiter prevents abuse and denial-of-service attacks by enforcing usage limits at account, network, and global levels.

### ChainAdapter

```solidity
interface IChainAdapter {
    function isBlockFinal(uint256 blockNumber) external view returns (bool);
    function handleReorg(uint256 fromBlock) external;
    function getChainSpecificConfig() external view returns (bytes memory);
    function validateTransaction(bytes calldata txData) external view returns (bool);
}
```

The ChainAdapter provides multi-chain compatibility by abstracting protocol differences and coordinating cross-chain state.

### ForkManager

```solidity
interface IForkManager {
    function detectFork(uint256 blockNumber) external view returns (bool);
    function handleFork(uint256 fromBlock, uint256 toBlock) external;
}
```

The ForkManager detects and manages chain splits and fork transitions, ensuring consistent operations during consensus-breaking events.

### TimeSeriesStore

```solidity
interface ITimeSeriesStore {
    function storeTimeSeries(bytes32 identifier, uint256 timestamp, bytes calldata data) external;
    function queryTimeSeries(bytes32 identifier, uint256 startTime, uint256 endTime) external view returns (bytes[] memory);
    function getLatestDataPoint(bytes32 identifier) external view returns (uint256, bytes memory);
    function getAggregatedData(bytes32 identifier, uint256 startTime, uint256 endTime, bytes32 aggregationType) external view returns (bytes memory);
}
```

The TimeSeriesStore efficiently handles financial and market time-series data, with specialized storage, indexing, and retrieval capabilities.

### ComputeOrchestrator

```solidity
interface IComputeOrchestrator {
    function scheduleComputation(bytes32 modelId, bytes calldata parameters) external returns (bytes32 jobId);
    function getComputationResult(bytes32 jobId) external view returns (bytes memory result, bool completed);
    function cancelComputation(bytes32 jobId) external;
    function getResourceUsage(address user) external view returns (uint256 cpuUsage, uint256 memoryUsage);
}
```

The ComputeOrchestrator manages computational resources for intensive operations like financial models, providing scheduling, results retrieval, and resource monitoring.

## Interface Extensions

Each interface may be extended with additional methods in their implementation contracts. The interfaces defined here represent the minimum required functionality for system integration.