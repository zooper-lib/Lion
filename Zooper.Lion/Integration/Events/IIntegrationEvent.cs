using Zooper.Lion.Common;

namespace Zooper.Lion.Integration.Events;

/// <summary>
/// Represents an integration event that is used for communication between microservices
/// in a distributed, event-driven architecture.
/// </summary>
public interface IIntegrationEvent : IEvent;