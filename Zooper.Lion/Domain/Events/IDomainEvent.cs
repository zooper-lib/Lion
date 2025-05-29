using Zooper.Lion.Common;

namespace Zooper.Lion.Domain.Events;

/// <summary>
/// Represents a domain event that is used for indicating state changes within a domain (aka. Service).
/// </summary>
public interface IDomainEvent : IEvent;