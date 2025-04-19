# Zooper.Lion

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

## Interface-Based Design

This library uses a minimal interface-based design that gives you the freedom to implement your domain objects however you want. Instead of providing bulky abstract base classes, it offers:

1. Simple interfaces that define the contracts
2. Extension methods that provide shared implementation logic
3. Example implementations for both class and record-based approaches

This approach gives you maximum flexibility while maintaining the semantic consistency of domain concepts across implementation styles.

## Implementation Examples

### Class-Based Approach

```csharp
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

## Benefits of This Approach

- **Simplicity**: Minimal interfaces with no bulky abstract classes
- **Flexibility**: Choose between classes or records based on your needs
- **DRY**: Share implementation logic through extension methods
- **Immutability**: Records provide immutability by default when desired
- **Encapsulation**: Proper access control across implementation styles
- **Low Coupling**: Your domain model doesn't depend on base classes
