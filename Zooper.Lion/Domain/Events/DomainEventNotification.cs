using System;
using Zooper.Lion.Common;

namespace Zooper.Lion.Domain.Events;

/// <summary>
/// Base implementation of a domain event notification that wraps a domain event with additional context.
/// </summary>
/// <typeparam name="TDomainEvent">The type of domain event being notified</typeparam>
public abstract class DomainEventNotification<TDomainEvent> : IDomainEventNotification<TDomainEvent>
    where TDomainEvent : IEvent
{
    /// <summary>
    /// Gets the domain event that triggered this notification
    /// </summary>
    public TDomainEvent DomainEvent { get; }

    /// <summary>
    /// Initializes a new instance of the domain event notification
    /// </summary>
    /// <param name="domainEvent">The domain event that triggered this notification</param>
    protected DomainEventNotification(TDomainEvent domainEvent)
    {
        DomainEvent = domainEvent ?? throw new ArgumentNullException(nameof(domainEvent));
    }
}
