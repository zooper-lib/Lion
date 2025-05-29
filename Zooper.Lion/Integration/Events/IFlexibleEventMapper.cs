using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Zooper.Lion.Integration.Events
{
    /// <summary>
    /// Flexible interface for components that convert domain notifications to integration events.
    /// This version returns objects for maximum flexibility when working with different event frameworks.
    /// </summary>
    /// <typeparam name="TNotification">The type of domain notification to process</typeparam>
    public interface IFlexibleEventMapper<in TNotification>
    {
        /// <summary>
        /// Creates integration events from a domain notification.
        /// </summary>
        /// <param name="notification">The domain notification containing the domain event and additional context</param>
        /// <param name="cancellationToken">Cancellation token for async operations</param>
        /// <returns>Collection of integration events to be published (typically IIntegrationEvent implementations)</returns>
        Task<IEnumerable<object>> CreateEventsAsync(
            TNotification notification,
            CancellationToken cancellationToken = default);
    }
}
