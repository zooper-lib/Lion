# Zooper.Lion

<img src="icon.png" alt="drawing" width="256"/>

## Domain-Driven Design (DDD) Library

This library provides a unified and minimalist approach to implementing Domain-Driven Design (DDD) patterns in .NET applications. The architecture uses interfaces and extension methods to support both class-based and record-based implementations, allowing you to choose the most appropriate approach for your specific use case without the need for duplicate implementations.

## Core Concepts

### Entities and Aggregate Roots

Entities are objects with a unique identity. The library provides:

- `IEntity<TId>` interface: The base contract for all entities
- `IAggregateRoot<TId>` interface: A marker interface for aggregate roots
- Extension methods that provide common implementation logic for both classes and records

### Value Objects

Value objects are immutable objects identified by their properties, not by identity. The library provides:

- `IValueObject` interface: The base contract for all value objects
- `IValueObjectWithComponents` interface: Enables equality comparison across implementation styles
- Extension methods that provide common implementation logic for both classes and records

### Events

The library provides interfaces for event-driven architectures:

- `IEvent` interface: The base contract for all events
- `IDomainEvent` interface: Events that represent business changes within a domain
- `IIntegrationEvent` interface: Events for communication between services/microservices

## Namespace Organization

The library is organized into logical namespaces for better code organization:

- `Zooper.Lion.Common` - Shared interfaces and utilities
- `Zooper.Lion.Domain.Entities` - Entity and aggregate root interfaces
- `Zooper.Lion.Domain.ValueObjects` - Value object interfaces and extensions
- `Zooper.Lion.Domain.Events` - Domain event interfaces
- `Zooper.Lion.Integration.Events` - Integration event interfaces
- `Zooper.Lion.Extensions.Records` - Extension methods for record implementations

## Interface-Based Design

This library uses a minimal interface-based design that gives you the freedom to implement your domain objects however you want. Instead of providing bulky abstract base classes, it offers:

1. Simple interfaces that define the contracts
2. Extension methods that provide shared implementation logic
3. Example implementations for both class and record-based approaches

This approach gives you maximum flexibility while maintaining the semantic consistency of domain concepts across implementation styles.

## Implementation Examples

### Class-Based Approach

```csharp
using Zooper.Lion.Domain.Entities;
using Zooper.Lion.Domain.ValueObjects;
using Zooper.Lion.Extensions.Records;

// Entity
public class Product : IEntity<Guid>
{
    // For classes, we can control mutability with access modifiers
    public Guid Id { get; protected set; }
    public string Name { get; private set; }

    protected Product() { }

    public Product(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    // Use the extension methods for equality
    public override bool Equals(object? obj) => this.EntityEquals(obj);
    public override int GetHashCode() => this.EntityGetHashCode();
}

// Aggregate Root
public class Order : IEntity<Guid>, IAggregateRoot<Guid>
{
    public Guid Id { get; protected set; }
    private readonly List<OrderItem> _items = new();

    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

    public Order(Guid id)
    {
        Id = id;
    }

    public void AddItem(Product product, int quantity)
    {
        // Domain logic here
    }

    // Use the extension methods for equality
    public override bool Equals(object? obj) => this.EntityEquals(obj);
    public override int GetHashCode() => this.EntityGetHashCode();
}

// Value Object
public class Address : IValueObject, IValueObjectWithComponents
{
    public string Street { get; }
    public string City { get; }

    public Address(string street, string city)
    {
        Street = street;
        City = city;
    }

    public IEnumerable<object?> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
    }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Street))
            throw new InvalidOperationException("Street cannot be empty");
    }

    // Use the extension methods for equality
    public override bool Equals(object? obj) =>
        this.ValueObjectEquals(obj, GetEqualityComponents);
    public override int GetHashCode() =>
        this.ValueObjectGetHashCode(GetEqualityComponents);
}
```

### Record-Based Approach

```csharp
using Zooper.Lion.Domain.Entities;
using Zooper.Lion.Domain.ValueObjects;
using Zooper.Lion.Extensions.Records;

// Entity
public record ProductRecord : IEntity<Guid>
{
    // For records, we use init-only properties for immutability
    public Guid Id { get; init; }
    public string Name { get; init; }

    public ProductRecord(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    // Use the extension methods for equality
    public bool Equals(object? obj) => this.EntityEquals(obj);
    public override int GetHashCode() => this.EntityGetHashCode();
}

// Aggregate Root
public record OrderRecord : IEntity<Guid>, IAggregateRoot<Guid>
{
    public Guid Id { get; init; }
    public IReadOnlyList<OrderItemRecord> Items { get; init; }

    public OrderRecord(Guid id, IReadOnlyList<OrderItemRecord> items)
    {
        Id = id;
        Items = items;
    }

    // With records, we create new instances for mutations
    public OrderRecord AddItem(ProductRecord product, int quantity)
    {
        var newItems = Items.ToList();
        newItems.Add(new OrderItemRecord(product, quantity));
        return this with { Items = newItems };
    }

    // Use the extension methods for equality
    public bool Equals(object? obj) => this.EntityEquals(obj);
    public override int GetHashCode() => this.EntityGetHashCode();
}

// Value Object
public record AddressRecord : IValueObject, IValueObjectWithComponents
{
    public string Street { get; init; }
    public string City { get; init; }

    public AddressRecord(string street, string city)
    {
        Street = street;
        City = city;
    }

    public IEnumerable<object?> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
    }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Street))
            throw new InvalidOperationException("Street cannot be empty");
    }

    // Use the extension methods for equality
    public bool Equals(object? obj) =>
        this.ValueObjectEquals(obj, GetEqualityComponents);
    public override int GetHashCode() =>
        this.ValueObjectGetHashCode(GetEqualityComponents);
}
```

## Mixing Implementation Styles

One of the benefits of this approach is that you can mix and match class-based and record-based implementations as needed:

```csharp
using Zooper.Lion.Domain.Entities;
using Zooper.Lion.Domain.ValueObjects;
using Zooper.Lion.Extensions.Records;

// Use records for immutable reference data
public record ProductRecord : IEntity<Guid>, IValueObjectWithComponents
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }

    // Implementation details...
}

// Use classes for mutable entities with behavior
public class ShoppingCart : IEntity<Guid>, IAggregateRoot<Guid>
{
    public Guid Id { get; protected set; }
    private readonly List<CartItem> _items = new();

    public void AddItem(ProductRecord product, int quantity)
    {
        // Domain logic...
    }

    // Implementation details...
}
```

## Event Mapping Framework

The library includes a comprehensive event mapping framework that enables clean conversion from domain events to integration events, with support for additional context and one-to-many mappings.

### Core Concepts

#### Domain Event Notifications

Domain event notifications wrap domain events with additional context needed for integration:

```csharp
using Zooper.Lion.Domain.Events;

// Your domain event
public class UserCreatedDomainEvent : IDomainEvent
{
    public string UserId { get; }
    public string Email { get; }
    public string FirstName { get; }
    public string LastName { get; }

    public UserCreatedDomainEvent(string userId, string email, string firstName, string lastName)
    {
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
    }
}

// Notification with additional context
public class UserCreatedNotification : DomainEventNotification<UserCreatedDomainEvent>
{
    public string PlaintextPassword { get; }  // Additional context
    public string ActivationToken { get; }    // Additional context

    public UserCreatedNotification(
        UserCreatedDomainEvent domainEvent,
        string plaintextPassword,
        string activationToken) : base(domainEvent)
    {
        PlaintextPassword = plaintextPassword ?? throw new ArgumentNullException(nameof(plaintextPassword));
        ActivationToken = activationToken ?? throw new ArgumentNullException(nameof(activationToken));
    }
}
```

#### Integration Events

Integration events represent the external contract for cross-service communication:

```csharp
using Zooper.Lion.Integration.Events;

public class UserRegisteredIntegrationEvent : IIntegrationEvent
{
    public string UserId { get; }
    public string Email { get; }
    public string FullName { get; }

    public UserRegisteredIntegrationEvent(string userId, string email, string fullName)
    {
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
    }
}

public class WelcomeEmailIntegrationEvent : IIntegrationEvent
{
    public string Email { get; }
    public string FirstName { get; }
    public string ActivationToken { get; }

    public WelcomeEmailIntegrationEvent(string email, string firstName, string activationToken)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        ActivationToken = activationToken ?? throw new ArgumentNullException(nameof(activationToken));
    }
}
```

### Event Mappers

#### Typed Event Mapper

For type-safe mapping with compile-time checking:

```csharp
using Zooper.Lion.Integration.Events;

public class UserCreatedEventMapper : IEventMapper<UserCreatedNotification>
{
    public IEnumerable<IIntegrationEvent> MapToIntegrationEvents(UserCreatedNotification notification)
    {
        var domainEvent = notification.DomainEvent;
        
        // Map to multiple integration events
        yield return new UserRegisteredIntegrationEvent(
            domainEvent.UserId,
            domainEvent.Email,
            $"{domainEvent.FirstName} {domainEvent.LastName}");

        yield return new WelcomeEmailIntegrationEvent(
            domainEvent.Email,
            domainEvent.FirstName,
            notification.ActivationToken);
    }
}
```

#### Flexible Event Mapper

For framework compatibility (returns objects instead of strongly typed events):

```csharp
using Zooper.Lion.Integration.Events;

public class FlexibleUserCreatedEventMapper : IFlexibleEventMapper<UserCreatedNotification>
{
    public IEnumerable<object> MapToIntegrationEvents(UserCreatedNotification notification)
    {
        var domainEvent = notification.DomainEvent;
        
        yield return new UserRegisteredIntegrationEvent(
            domainEvent.UserId,
            domainEvent.Email,
            $"{domainEvent.FirstName} {domainEvent.LastName}");

        yield return new WelcomeEmailIntegrationEvent(
            domainEvent.Email,
            domainEvent.FirstName,
            notification.ActivationToken);
    }
}
```

### Dependency Injection Setup

The library provides extension methods for automatic registration of event mappers:

```csharp
using Microsoft.Extensions.DependencyInjection;
using Zooper.Lion.Extensions.DependencyInjection;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Register all event mappers from the calling assembly
        services.AddEventMappers();
        
        // Register event mappers from specific assemblies
        services.AddEventMappers(typeof(UserCreatedEventMapper).Assembly);
        
        // Register event mappers from assembly containing a specific type
        services.AddEventMappersFromAssemblyOf<UserCreatedEventMapper>();
    }
}
```

### Usage in Application Services

```csharp
public class UserApplicationService
{
    private readonly IEventMapper<UserCreatedNotification> _eventMapper;
    private readonly IServiceProvider _serviceProvider;

    public UserApplicationService(
        IEventMapper<UserCreatedNotification> eventMapper,
        IServiceProvider serviceProvider)
    {
        _eventMapper = eventMapper;
        _serviceProvider = serviceProvider;
    }

    public async Task CreateUserAsync(CreateUserCommand command)
    {
        // Create user and raise domain event
        var domainEvent = new UserCreatedDomainEvent(
            command.UserId, 
            command.Email, 
            command.FirstName, 
            command.LastName);

        // Create notification with additional context
        var notification = new UserCreatedNotification(
            domainEvent,
            command.PlaintextPassword,
            GenerateActivationToken());

        // Map to integration events
        var integrationEvents = _eventMapper.MapToIntegrationEvents(notification);

        // Publish integration events
        foreach (var integrationEvent in integrationEvents)
        {
            await PublishIntegrationEventAsync(integrationEvent);
        }
    }
}
```

### Event Mapping Benefits

- **Separation of Concerns**: Domain events focus on business changes, integration events focus on external contracts
- **Additional Context**: Notifications can include context not available in the original domain event
- **One-to-Many Mapping**: Single domain events can trigger multiple integration events
- **Type Safety**: Strongly typed mappers provide compile-time checking
- **Framework Compatibility**: Flexible mappers work with any event publishing framework
- **Automatic Registration**: Dependency injection extensions simplify setup

## Benefits of This Approach

- **Simplicity**: Minimal interfaces with no bulky abstract classes
- **Flexibility**: Choose between classes or records based on your needs
- **DRY**: Share implementation logic through extension methods
- **Immutability**: Records provide immutability by default when desired
- **Encapsulation**: Proper access control across implementation styles
- **Low Coupling**: Your domain model doesn't depend on base classes
- **Event-Driven Architecture**: Comprehensive support for domain and integration events
