using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Zooper.Lion.Integration.Events
{
    /// <summary>
    /// Base interface for components that convert domain notifications to integration events.
    /// This enables the mapping of domain events to one or more integration events for cross-service communication.
    /// </summary>
    /// <typeparam name="TNotification">The type of domain notification to process</typeparam>
    public interface IEventMapper<in TNotification>
    {
        /// <summary>
        /// Creates integration events from a domain notification.
        /// </summary>
        /// <param name="notification">The domain notification containing the domain event and additional context</param>
        /// <param name="cancellationToken">Cancellation token for async operations</param>
        /// <returns>Collection of integration events to be published</returns>
        Task<IEnumerable<IIntegrationEvent>> CreateEventsAsync(
            TNotification notification,
            CancellationToken cancellationToken = default);
    }
}
