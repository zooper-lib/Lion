# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0] - 2025-05-29

### 🚨 BREAKING CHANGES

- **Namespace Restructuring**: Complete reorganization of namespaces for better code organization and separation of concerns
  - `Zooper.Lion.Interfaces` → Split into multiple specialized namespaces
  - `Zooper.Lion.Domain` → Split into `Zooper.Lion.Domain.Entities`, `Zooper.Lion.Domain.ValueObjects`, `Zooper.Lion.Domain.Events`
  - Added new namespaces: `Zooper.Lion.Common`, `Zooper.Lion.Integration.Events`, `Zooper.Lion.Extensions.Records`

### Added

- **Event Support**: Added comprehensive event interfaces for event-driven architectures
  - `IEvent` - Base interface for all events in `Zooper.Lion.Common`
  - `IDomainEvent` - Interface for domain events in `Zooper.Lion.Domain.Events`
  - `IIntegrationEvent` - Interface for integration events in `Zooper.Lion.Integration.Events`

- **Better Organization**: New directory structure that separates concerns more clearly
  - `Common/` - Shared interfaces and utilities
  - `Domain/Entities/` - Entity and aggregate root interfaces
  - `Domain/ValueObjects/` - Value object interfaces and extensions
  - `Domain/Events/` - Domain event interfaces (ready for event mapping functionality)
  - `Integration/Events/` - Integration event interfaces (ready for event mapping functionality)
  - `Extensions/Records/` - Record-specific extension methods

### Changed

- **Examples Location**: Moved example files to root level for better project organization
- **Namespace Updates**: All existing interfaces moved to more specific, logical namespaces

### Migration Guide

If you're upgrading from v1.x.x, you'll need to update your using statements:

#### Before (v1.x.x)
```csharp
using Zooper.Lion.Domain;
using Zooper.Lion.Interfaces;
```

#### After (v2.0.0)
```csharp
using Zooper.Lion.Domain.Entities;      // For IEntity, IAggregateRoot
using Zooper.Lion.Domain.ValueObjects;  // For IValueObject, IValueObjectWithComponents
using Zooper.Lion.Domain.Events;        // For IDomainEvent
using Zooper.Lion.Integration.Events;   // For IIntegrationEvent
using Zooper.Lion.Common;               // For IEvent
using Zooper.Lion.Extensions.Records;   // For extension methods
```

## [1.0.0] - 2025-05-28

### Added

- Initial release of Zooper.Lion
- `IEntity<TId>` interface for entities with unique identity
- `IAggregateRoot<TId>` interface for aggregate roots
- `IValueObject` interface for value objects
- `IValueObjectWithComponents` interface for cross-implementation equality
- Extension methods for both class and record implementations
- Support for both class-based and record-based domain object implementations
- Comprehensive examples demonstrating usage patterns
