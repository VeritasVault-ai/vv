# vv.Domain

## Overview

This project contains the core domain model for the Phoenix Market Data Platform. It represents the innermost layer of our hexagonal/onion architecture and is free from infrastructure concerns, focusing solely on the business domain.

## Architectural Principles

### Domain Layer Responsibilities

- Defines the domain entities, value objects, and primitives
- Contains business logic and domain rules
- Defines domain events representing important business occurrences
- Declares repository interfaces (ports) that outer layers must implement
- Implements domain specifications for complex querying logic

### Design Guidelines

- No dependencies on infrastructure, data access, or UI concerns
- No direct dependencies on external libraries except for core functionality (e.g., MediatR for events)
- Domain objects should enforce invariants and business rules
- All external operations are defined through interfaces (ports)

## Project Structure

```txt
vv.Domain/
├── Constants/
│   └── CurrencyCodes.cs
├── Events/
│   ├── DomainEvent.cs
│   ├── EntityEvents.cs
│   └── IMarketDataEventPublisher.cs
├── Exceptions/
│   ├── InvalidMarketDataException.cs
│   └── MarketDataNotFoundException.cs
├── Models/
│   ├── Interfaces/
│   │   ├── IMarketDataEntity.cs
│   │   ├── ISoftDeletable.cs
│   │   └── IVersionedEntity.cs
│   ├── ValueObjects/
│   │   ├── CurrencyPair.cs
│   │   └── Price.cs
│   ├── AssetClass.cs
│   ├── BaseMarketData.cs
│   ├── FxSpotPriceData.cs
│   └── ...
├── Repositories/
│   ├── IMarketDataCommands.cs
│   ├── IMarketDataQueries.cs
│   └── ...
├── Services/
│   └── IMarketDataService.cs
└── Specifications/
    ├── ISpecification.cs
    └── MarketDataSpecification.cs
```

### Models

- `BaseMarketData`: Abstract base class for all market data entities
- `FxSpotPriceData`: Foreign exchange spot price data entity
- `FxSpotPriceRate`: Value object representing FX rate information
- `FxVolSurfaceData`: FX volatility surface data entity
- `CryptoOrdinalSpotPriceData`: Cryptocurrency spot price data entity
- Enums: `AssetClass`, `PriceSide`, `Regions` for domain classification

### Interfaces

- `IMarketDataEntity`: Base interface for all market data entities
- `ISoftDeletable`: Interface for entities supporting soft deletion
- `IVersionedEntity`: Interface for entities supporting versioning

### Specifications

- `ISpecification<T>`: Generic specification pattern interface
- `MarketDataSpecification`: Implementation for building market data queries
  - Supports filtering by asset class, region, date range, etc.
  - Used for composable, reusable query logic

### Repositories (Ports)

- `IMarketDataCommands`: Commands for modifying market data (CQRS write side)
- `IMarketDataQueries`: Queries for retrieving market data (CQRS read side)
- `IRepository<T>`: Generic repository interface
- `IVersionedRepository<T>`: Repository interface for versioned entities

### Events

- `EntityEvents`: Domain events for entity changes
- `IMarketDataEventPublisher`: Interface for publishing domain events

### Services

- `IMarketDataService`: Domain service for complex operations on market data

## CQRS Implementation

This domain layer supports CQRS (Command Query Responsibility Segregation) through:

1. Separate repository interfaces for commands (`IMarketDataCommands`) and queries (`IMarketDataQueries`)
2. Domain events for communicating changes across the system
3. Specifications for building complex queries

## Usage Examples

### Creating a Specification for Query

```csharp
// Create a specification for EUR/USD prices in the last week
var spec = new MarketDataSpecification()
    .WithAssetClass("fx")
    .WithAssetId("eurusd")
    .WithFromDate(DateOnly.FromDateTime(DateTime.Now.AddDays(-7)))
    .WithToDate(DateOnly.FromDateTime(DateTime.Now));

// Use with a query repository
var prices = await _marketDataQueries.GetAsync(spec);
```

### Working with Domain Events

```csharp
// When updating an entity, you might raise an event
public async Task UpdatePrice(FxSpotPriceData priceData)
{
    // Update entity
    priceData.BidPrice = newBidPrice;
    priceData.AskPrice = newAskPrice;
    
    // Publish domain event
    await _eventPublisher.PublishAsync(new MarketDataUpdatedEvent(priceData));
}
```

## Extending the Domain

When extending the domain:

1. New entities should inherit from appropriate base classes
2. Ensure business rules are enforced in entity methods
3. Define new specifications as needed for querying
4. Add appropriate repository methods to interface definitions
5. Create domain events for significant state changes

## Dependencies

- **MediatR**: Used for implementing the mediator pattern for domain events

## Testing

The domain layer should be thoroughly unit tested:

1. Test entity behavior and business rules
2. Test specifications to ensure they produce correct expressions
3. Test value objects for correct behavior and validation
4. Use mocks/stubs for interfaces to test in isolation

## Best Practices

1. **Rich Domain Model**: Entities should contain behavior, not just data
2. **Encapsulation**: Use proper encapsulation to maintain invariants
3. **Immutable Value Objects**: Value objects should be immutable
4. **Domain Events**: Use events for cross-aggregate communication
5. **Aggregate Roots**: Define clear aggregate boundaries
6. **Repository Per Aggregate**: Follow the repository per aggregate root pattern

## Integration with Application Layer

The domain layer defines the contracts (interfaces/ports) that the application layer will use. Applications should:

1. Use domain entities, value objects, and events
2. Implement use cases that orchestrate domain operations
3. Use domain repositories through their interfaces
4. Follow the domain rules and constraints

## Architecture Diagram

```txt
┌───────────────────────────────────────────────────────────┐
│                                                           │
│                    Presentation Layer                     │
│                      (vv.API, vv.UI)                      |
│                      (vv.Functions)                       |
│                                                           │
└──────────────────────────┬────────────────────────────────┘
                           │
                           ▼
┌───────────────────────────────────────────────────────────┐
│                                                           │
│                   Application Layer                       │
│                   (vv.Application)                        │
│                                                           │
└───────────────┬───────────────────────────┬───────────────┘
                │                           │
                ▼                           ▼
┌───────────────────────────┐   ┌───────────────────────────┐
│                           │   │                           │
│        Domain Layer       │◄──│    Infrastructure Layer   │
│   (vv.Domain)|(vv.Data)   │   |         (vv.Infra)        |
│                           │   │                           │
└───────────────────────────┘   └───────────────────────────┘
```

## Relationship with Other Projects

The domain layer is at the center of our hexagonal architecture:

- **vv.Application**: Depends on Domain. Contains application services, command/query handlers, and DTOs that implement use cases using domain entities and interfaces.

- **vv.Data**: Depends on Domain. Implements repository interfaces defined in the domain layer, providing data persistence capabilities.

- **vv.Infrastructure**: Depends on Domain. Implements technical services required by the domain (event publishing, logging, etc.).

- **vv.API**: Depends on Application. Provides HTTP endpoints that map to application use cases.

Each layer depends inward toward the domain, maintaining the dependency rule of clean architecture. The domain layer has no knowledge of or dependencies on any other layer.
