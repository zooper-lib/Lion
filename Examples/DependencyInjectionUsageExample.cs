using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Zooper.Lion.Extensions.DependencyInjection;

namespace Zooper.Lion.Examples.Usage
{
    /// <summary>
    /// Example showing how to configure dependency injection for event mappers
    /// </summary>
    public static class DependencyInjectionExample
    {
        /// <summary>
        /// Example of how to register event mappers in your application startup
        /// </summary>
        public static void ConfigureServices(IServiceCollection services)
        {
            // Option 1: Register event mappers from current assembly
            services.AddEventMappers();

            // Option 2: Register event mappers from specific assemblies
            services.AddEventMappers(Assembly.GetExecutingAssembly());

            // Option 3: Register event mappers from assembly containing a specific type
            services.AddEventMappersFromAssemblyOf<DependencyInjectionExample>();

            // The above will automatically register all classes that implement:
            // - IEventMapper<TNotification>
            // - IFlexibleEventMapper<TNotification>
        }
    }
}
