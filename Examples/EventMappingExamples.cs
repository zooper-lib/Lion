using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zooper.Lion.Domain.Events;
using Zooper.Lion.Integration.Events;

namespace Zooper.Lion.Examples.EventMapping
{
    // Example domain event
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

    // Example notification with additional context
    public class UserCreatedNotification : DomainEventNotification<UserCreatedDomainEvent>
    {
        public string PlaintextPassword { get; } // Additional data not in domain event
        public string ActivationToken { get; }   // Additional data not in domain event

        public UserCreatedNotification(
            UserCreatedDomainEvent domainEvent,
            string plaintextPassword,
            string activationToken) : base(domainEvent)
        {
            PlaintextPassword = plaintextPassword ?? throw new ArgumentNullException(nameof(plaintextPassword));
            ActivationToken = activationToken ?? throw new ArgumentNullException(nameof(activationToken));
        }
    }

    // Example integration events
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

    public class UserAnalyticsIntegrationEvent : IIntegrationEvent
    {
        public string UserId { get; }
        public string Email { get; }

        public UserAnalyticsIntegrationEvent(string userId, string email)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }
    }

    // Example event mapper using the typed interface
    public class UserCreatedEventMapper : IEventMapper<UserCreatedNotification>
    {
        public Task<IEnumerable<IIntegrationEvent>> CreateEventsAsync(
            UserCreatedNotification notification,
            CancellationToken cancellationToken = default)
        {
            var domainEvent = notification.DomainEvent;

            // Create multiple integration events from one domain event
            var integrationEvents = new List<IIntegrationEvent>
            {
                new UserRegisteredIntegrationEvent(
                    domainEvent.UserId,
                    domainEvent.Email,
                    $"{domainEvent.FirstName} {domainEvent.LastName}"),

                new WelcomeEmailIntegrationEvent(
                    domainEvent.Email,
                    domainEvent.FirstName,
                    notification.ActivationToken), // Using additional context data
                    
                new UserAnalyticsIntegrationEvent(
                    domainEvent.UserId,
                    domainEvent.Email)
            };

            return Task.FromResult<IEnumerable<IIntegrationEvent>>(integrationEvents);
        }
    }

    // Example flexible event mapper (returns objects for framework compatibility)
    public class UserCreatedFlexibleEventMapper : IFlexibleEventMapper<UserCreatedNotification>
    {
        public Task<IEnumerable<object>> CreateEventsAsync(
            UserCreatedNotification notification,
            CancellationToken cancellationToken = default)
        {
            var domainEvent = notification.DomainEvent;

            // This could return any type of objects, including framework-specific event types
            var events = new List<object>
            {
                new UserRegisteredIntegrationEvent(
                    domainEvent.UserId,
                    domainEvent.Email,
                    $"{domainEvent.FirstName} {domainEvent.LastName}"),
                    
                // Could also include non-IIntegrationEvent objects if needed by specific frameworks
                new { EventType = "UserCreated", UserId = domainEvent.UserId }
            };

            return Task.FromResult<IEnumerable<object>>(events);
        }
    }
}
