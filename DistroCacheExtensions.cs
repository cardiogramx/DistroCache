using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;

namespace DistroCache.Extensions.DependencyInjection
{
    public static class DistroCacheExtensions
    {
        /// <summary>
        /// Adds distro to the service collection as in-memory distributed cache.
        /// </summary>
        public static IServiceCollection AddDistroMemoryCache(this IServiceCollection services, Action<DistributedCacheEntryOptions> entryOptions)
        {
            services.AddDistributedMemoryCache();

            //Configures IOptions<DistroCacheOption>
            services.Configure(entryOptions);

            //Registers IAutoMapperBuilder in the service container
            services.AddScoped<IDistroCache, DistroCache>();

            return services;
        }

        /// <summary>
        /// Adds distro to the service collection as MSSQL distributed cache.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="sqlOptions"></param>
        /// <param name="entryOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddDistroSqlCache(this IServiceCollection services, Action<SqlServerCacheOptions> sqlOptions, Action<DistributedCacheEntryOptions> entryOptions)
        {
            services.AddDistributedSqlServerCache(sqlOptions);

            //Configures IOptions<DistroCacheOption>
            services.Configure(entryOptions);

            //Registers IAutoMapperBuilder in the service container
            services.AddScoped<IDistroCache, DistroCache>();

            return services;
        }

        /// <summary>
        /// Adds distro to the service collection as redis distributed cache.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="redisOptions"></param>
        /// <param name="entryOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddDistroRedisCache(this IServiceCollection services, Action<RedisCacheOptions> redisOptions, Action<DistributedCacheEntryOptions> entryOptions)
        {
            services.AddStackExchangeRedisCache(redisOptions);

            //Configures IOptions<DistroCacheOption>
            services.Configure(entryOptions);

            //Registers IAutoMapperBuilder in the service container
            services.AddScoped<IDistroCache, DistroCache>();

            return services;
        }
    }
}
