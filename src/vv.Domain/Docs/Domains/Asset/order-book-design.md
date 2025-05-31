---
document_type: guide
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

# Order Book Design

> Architecture and Implementation of the VeritasVault Trading Order Book

---

## Overview

The Order Book is a core component of the trading infrastructure that maintains buy and sell orders, implements price-time priority matching, and provides market depth visibility.

## Key Components

### Order Book Structure

```typescript
interface OrderBook {
  assetPair: AssetPair;            // Trading pair this book manages
  buySide: PriceLevelMap;          // Buy orders organized by price level
  sellSide: PriceLevelMap;         // Sell orders organized by price level
  version: number;                  // Optimistic locking version
  lastTradePrice: Price | null;     // Most recent execution price
  lastTradeTimestamp: Timestamp | null; // Time of most recent execution
  status: OrderBookStatus;          // Current operational status
  metrics: OrderBookMetrics;        // Performance and depth metrics
}

interface PriceLevelMap {
  levels: Map<Price, PriceLevel>;   // Price levels with queued orders
  bestPrice: Price | null;          // Best price in the book
  totalVolume: Quantity;            // Total volume across all levels
  totalOrders: number;              // Total order count
}

interface PriceLevel {
  price: Price;                     // Price for this level
  orders: Queue<Order>;             // Time-priority queue of orders
  totalVolume: Quantity;            // Total volume at this level
  timestamp: Timestamp;             // Last update time
}
```

### Order Structure

```typescript
interface Order {
  id: OrderId;                     // Unique order identifier
  assetPair: AssetPair;            // Trading pair
  side: OrderSide;                 // BUY or SELL
  orderType: OrderType;            // LIMIT, MARKET, etc.
  price: Price | null;             // Limit price (null for market orders)
  quantity: Quantity;              // Total order quantity
  remainingQuantity: Quantity;     // Unfilled quantity
  timeInForce: TimeInForce;        // Order duration policy
  trader: EntityId;                // Order owner
  timestamp: Timestamp;            // Submission time
  status: OrderStatus;             // Current order status
  expirationTime: Timestamp | null; // Order expiration time
  executionConstraints: ExecutionConstraints; // Special execution conditions
  metadata: OrderMetadata;         // Additional order information
}

enum OrderSide {
  BUY,
  SELL
}

enum OrderType {
  LIMIT,
  MARKET,
  STOP,
  STOP_LIMIT,
  ICEBERG,
  FILL_OR_KILL,
  GOOD_TILL_DATE
}

enum OrderStatus {
  PENDING,
  ACTIVE,
  PARTIALLY_FILLED,
  FILLED,
  CANCELLED,
  REJECTED,
  EXPIRED
}
```

## Order Book Operations

### Order Placement

The order placement process:

1. Validate order data and permissions
2. Check pre-trade risk controls
3. Apply compliance rules
4. Attempt immediate matching if appropriate
5. Place unfilled quantity in the book
6. Update order book metrics
7. Emit order placement event

```typescript
function placeOrder(order: Order): Result<OrderId> {
  // Input validation
  if (!validateOrderInput(order)) {
    return { success: false, error: TradingError.INVALID_ORDER };
  }
  
  // Pre-trade validation
  const validationResult = validatePreTrade(order);
  if (!validationResult.success) {
    return validationResult;
  }
  
  // Apply compliance rules
  const complianceResult = checkOrderCompliance(order);
  if (!complianceResult.success) {
    return complianceResult;
  }
  
  // Try immediate matching for market orders
  let remainingOrder = order;
  if (order.orderType === OrderType.MARKET || canMatchImmediately(order)) {
    const matchResult = matchOrderImmediately(order);
    remainingOrder = matchResult.remainingOrder;
    
    // If fully matched or FOK that couldn't be fully matched
    if (remainingOrder.remainingQuantity.isZero() || 
        (order.timeInForce === TimeInForce.FILL_OR_KILL && 
         remainingOrder.remainingQuantity.equals(order.quantity))) {
      return { 
        success: true, 
        data: order.id,
        context: { matched: matchResult.trades }
      };
    }
  }
  
  // Add to order book if there's remaining quantity
  if (!remainingOrder.remainingQuantity.isZero()) {
    insertOrderInBook(remainingOrder);
    updateOrderBookMetrics();
  }
  
  // Emit order event
  emitOrderPlacedEvent(order);
  
  return { success: true, data: order.id };
}
```

### Order Matching

The price-time priority matching algorithm:

1. Find the best matching price level
2. Match orders in time priority at that price
3. Generate trade records for matched quantities
4. Update order status and remaining quantities
5. Remove filled orders from the book
6. Update market depth and metrics

```typescript
function matchOrders(
  version: number, 
  optimisticLock: string
): Result<[Trade[], string]> {
  // Verify order book version matches expected version
  if (orderBook.version !== version) {
    return { success: false, error: TradingError.CONCURRENCY_CONFLICT };
  }
  
  const trades: Trade[] = [];
  
  // Continue matching while there are matchable orders
  while (canMatch(orderBook.buySide.bestPrice, orderBook.sellSide.bestPrice)) {
    // Get the best prices from each side
    const buyPrice = orderBook.buySide.bestPrice!;
    const sellPrice = orderBook.sellSide.bestPrice!;
    
    // Get the orders at the best price levels
    const buyOrders = orderBook.buySide.levels.get(buyPrice)!.orders;
    const sellOrders = orderBook.sellSide.levels.get(sellPrice)!.orders;
    
    // Match orders at these levels
    const matchResult = matchOrdersAtPriceLevel(buyOrders, sellOrders);
    trades.push(...matchResult.trades);
    
    // Clean up filled orders
    removeFilledOrders(buyOrders);
    removeFilledOrders(sellOrders);
    
    // Update price levels if needed
    updatePriceLevels();
  }
  
  // Increment order book version
  orderBook.version++;
  
  // Update metrics
  updateOrderBookMetrics();
  
  // Generate new optimistic lock token
  const newLockToken = generateLockToken(orderBook);
  
  return { 
    success: true, 
    data: [trades, newLockToken],
    context: { newVersion: orderBook.version }
  };
}
```

### Order Cancellation

The order cancellation process:

1. Validate cancellation request
2. Find the order in the book
3. Remove the order from its price level
4. Update the order status
5. Update book metrics
6. Emit cancellation event

```typescript
function cancelOrder(orderId: OrderId): Result<void> {
  // Find the order
  const order = findOrderById(orderId);
  if (!order) {
    return { success: false, error: TradingError.ORDER_NOT_FOUND };
  }
  
  // Verify cancellation permissions
  if (!canCancelOrder(order)) {
    return { success: false, error: TradingError.UNAUTHORIZED };
  }
  
  // Remove from book
  removeOrderFromBook(order);
  
  // Update order status
  order.status = OrderStatus.CANCELLED;
  
  // Update metrics
  updateOrderBookMetrics();
  
  // Emit event
  emitOrderCancelledEvent(order);
  
  return { success: true };
}
```

## Market Depth Management

### Depth Calculation

The order book maintains current market depth information:

```typescript
interface MarketDepth {
  buySide: PriceLevel[];          // Buy side price levels
  sellSide: PriceLevel[];         // Sell side price levels
  spread: Price;                  // Current bid-ask spread
  midPrice: Price;                // Mid price between best bid and ask
  lastTradePrice: Price | null;   // Most recent execution price
  volumeWeightedMidPrice: Price;  // Volume-weighted mid price
  timestamp: Timestamp;           // Depth snapshot time
}

function getMarketDepth(levels: number): MarketDepth {
  return {
    buySide: getTopPriceLevels(orderBook.buySide, levels, true),
    sellSide: getTopPriceLevels(orderBook.sellSide, levels, false),
    spread: calculateSpread(),
    midPrice: calculateMidPrice(),
    lastTradePrice: orderBook.lastTradePrice,
    volumeWeightedMidPrice: calculateVWMP(),
    timestamp: getCurrentTimestamp()
  };
}
```

### Depth Visualization

Market depth is visualized as a price ladder showing liquidity at each level:

```typescript
interface DepthVisualization {
  priceLevels: VisualPriceLevel[];    // Combined price levels
  totalBuyVolume: Quantity;           // Total buy side volume
  totalSellVolume: Quantity;          // Total sell side volume
  maxVolumeForScale: Quantity;        // Maximum volume for visual scaling
}

interface VisualPriceLevel {
  price: Price;                       // Level price
  buyVolume: Quantity;                // Buy volume at this price
  sellVolume: Quantity;               // Sell volume at this price
  buyOrders: number;                  // Number of buy orders
  sellOrders: number;                 // Number of sell orders
  cumBuyVolume: Quantity;             // Cumulative buy volume to this level
  cumSellVolume: Quantity;            // Cumulative sell volume to this level
}
```

## Circuit Breakers and Risk Controls

Order books implement circuit breakers that can halt trading when specific conditions are met:

```typescript
interface CircuitBreaker {
  type: CircuitBreakerType;          // Type of breaker
  parameters: CircuitBreakerParams;  // Configurable parameters
  status: CircuitBreakerStatus;      // Current status
  lastTriggered: Timestamp | null;   // Last triggered time
}

enum CircuitBreakerType {
  PRICE_BAND,                         // Price movement limits
  VOLATILITY,                         // Volatility threshold
  ORDER_IMBALANCE,                    // Order book imbalance
  TRADE_VOLUME,                       // Excessive volume
  EXECUTION_QUALITY,                  // Execution quality deterioration
  MANUAL                              // Manual intervention
}

enum CircuitBreakerStatus {
  INACTIVE,                           // Not triggered
  WARNING,                            // Warning level reached
  TRIGGERED,                          // Fully triggered
  COOLING_DOWN                        // Post-trigger cool-down period
}
```

## Performance Considerations

The order book is optimized for the following operations:

1. Fast order insertion (O(log n) where n is the number of price levels)
2. Constant-time best price access (O(1))
3. Fast matching (O(m) where m is the number of orders matched)
4. Efficient cancellation (O(log n) to find price level + O(1) with direct pointer)
5. Compact memory representation for high-frequency updates

## Concurrency Control

The order book uses optimistic concurrency control:

1. Each order book has a version number
2. Operations specify the expected version
3. If versions don't match, the operation is rejected
4. Successful operations increment the version
5. A lock token is used for multi-operation transactions

## Event Emission

Every order book state change emits events:

1. OrderPlaced - New order added to the book
2. OrderCancelled - Order removed from the book
3. OrderModified - Order parameters changed
4. TradeExecuted - Orders matched and executed
5. OrderBookStateChanged - Book status changed
6. CircuitBreakerTriggered - Risk control activated
7. DepthChanged - Significant depth change detected

## Metrics and Monitoring

The order book continuously tracks key metrics:

```typescript
interface OrderBookMetrics {
  depth: {                            // Market depth metrics
    buyLevels: number;                 // Buy price levels count
    sellLevels: number;                // Sell price levels count
    buyVolume: Quantity;               // Total buy volume
    sellVolume: Quantity;              // Total sell volume
    spread: Price;                     // Current spread
    spreadBps: number;                 // Spread in basis points
  };
  activity: {                         // Activity metrics
    ordersPerSecond: number;           // Order arrival rate
    tradesPerSecond: number;           // Trade execution rate
    cancelRatio: number;               // Cancel to order ratio
    fillRatio: number;                 // Fill to order ratio
  };
  performance: {                      // Performance metrics
    matchLatencyMs: number;            // Matching latency
    updateLatencyMs: number;           // Book update latency
    queryLatencyMs: number;            // Query response latency
  };
  riskControls: {                     // Risk metrics
    circuitBreakerStatus: CircuitBreakerStatus;  // Current circuit breaker status
    warningEvents: number;             // Warning events count
    lastWarningTime: Timestamp | null; // Last warning time
  };
}
```