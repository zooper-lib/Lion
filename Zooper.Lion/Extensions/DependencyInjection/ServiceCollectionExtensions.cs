using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Zooper.Lion.Integration.Events;

namespace Zooper.Lion.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for registering Lion event mapping services.
    /// This provides a clean API for dependency injection container configuration.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all event mappers from the specified assembly to the service collection.
        /// Scans for implementations of IEventMapper&lt;T&gt; and IFlexibleEventMapper&lt;T&gt; interfaces.
        /// </summary>
        /// <param name="services">The service collection to register services with</param>
        /// <param name="assemblies">The assemblies to scan for event mapper implementations</param>
        /// <returns>The service collection for method chaining</returns>
        public static IServiceCollection AddEventMappers(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0)
            {
                throw new ArgumentException("At least one assembly must be provided", nameof(assemblies));
            }

            foreach (var assembly in assemblies)
            {
                RegisterEventMappersFromAssembly(services, assembly);
            }

            return services;
        }

        /// <summary>
        /// Adds all event mappers from the calling assembly to the service collection.
        /// Scans for implementations of IEventMapper&lt;T&gt; and IFlexibleEventMapper&lt;T&gt; interfaces.
        /// </summary>
        /// <param name="services">The service collection to register services with</param>
        /// <returns>The service collection for method chaining</returns>
        public static IServiceCollection AddEventMappers(this IServiceCollection services)
        {
            var callingAssembly = Assembly.GetCallingAssembly();
            return AddEventMappers(services, callingAssembly);
        }

        /// <summary>
        /// Adds all event mappers from the assembly containing the specified type to the service collection.
        /// Scans for implementations of IEventMapper&lt;T&gt; and IFlexibleEventMapper&lt;T&gt; interfaces.
        /// </summary>
        /// <typeparam name="TAssemblyMarker">A type from the assembly to scan</typeparam>
        /// <param name="services">The service collection to register services with</param>
        /// <returns>The service collection for method chaining</returns>
        public static IServiceCollection AddEventMappersFromAssemblyOf<TAssemblyMarker>(this IServiceCollection services)
        {
            var assembly = typeof(TAssemblyMarker).Assembly;
            return AddEventMappers(services, assembly);
        }

        private static void RegisterEventMappersFromAssembly(IServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                if (type.IsInterface || type.IsAbstract)
                    continue;

                // Check for IEventMapper<T> implementations
                var eventMapperInterfaces = type.GetInterfaces()
                    .Where(i => i.IsGenericType &&
                               (i.GetGenericTypeDefinition() == typeof(IEventMapper<>) ||
                                i.GetGenericTypeDefinition() == typeof(IFlexibleEventMapper<>)));

                foreach (var interfaceType in eventMapperInterfaces)
                {
                    services.AddTransient(interfaceType, type);
                }
            }
        }
    }
}
