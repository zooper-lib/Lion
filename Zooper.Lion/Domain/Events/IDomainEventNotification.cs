using Zooper.Lion.Common;

namespace Zooper.Lion.Domain.Events
{
    /// <summary>
    /// Represents a notification that wraps a domain event with additional context data.
    /// This allows passing extra information that may not be part of the domain event itself.
    /// </summary>
    /// <typeparam name="TDomainEvent">The type of domain event being notified</typeparam>
    public interface IDomainEventNotification<out TDomainEvent> where TDomainEvent : IEvent
    {
        /// <summary>
        /// Gets the domain event that triggered this notification
        /// </summary>
        TDomainEvent DomainEvent { get; }
    }
}
