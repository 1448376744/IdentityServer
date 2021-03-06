using IdentityServer;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddOrUpdateTransient<TService, TImplementation>(this IServiceCollection services)
           where TService : class where TImplementation : class, TService
        {
            if (services.Any(a => a.ServiceType == typeof(TService)
                && a.ImplementationType == typeof(TImplementation) 
                && a.Lifetime == ServiceLifetime.Transient))
            {
                services.Replace(ServiceDescriptor.Transient<TService, TImplementation>());
            }
            else
            {
                services.Add(ServiceDescriptor.Transient<TService, TImplementation>());
            }
            return services;
        }

        public static IServiceCollection AddLoaclApiAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(LocalAuthenticationDefaults.Scheme)
                .AddLoaclApiAuthentication();
            return services;
        }

        public static IIdentityServerBuilder AddIdentityServerBuilder(this IServiceCollection services)
        {
            return new IdentityServerBuilder(services);
        }

        public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services)
        {
            var builder = services.AddIdentityServerBuilder();

            builder
                .AddRequiredPlatformServices()
                .AddPluggableValidators()
                .AddPluggableServices()
                .AddRequiredPlatformEndpoints()
                .AddPluggableResponseGenerators();

            return builder;
        }

        public static IIdentityServerBuilder AddIdentityServer(this IServiceCollection services, Action<IdentityServerOptions> setupAction)
        {
            services.Configure(setupAction);
            return services.AddIdentityServer();
        }
    }
}
