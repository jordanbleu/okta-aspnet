using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Okta.Idx.Sdk;
using Okta.Idx.Sdk.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Okta.AspNetCore
{
    public static class OktaIdentityEngineExtensions
    {
        public const string ConfigurationSection = "idx";
        public const string OktaTokensKey = "OktaTokens";
        public const string UserInfoSessionKey = "OktaUserInfo";

        public static async Task<OktaUserInfo> GetOktaUserInfoAsync(this HttpContext httpContext)
        {
            return await GetOktaUserInfoAsync(httpContext.Session);
        }

        public static async Task<OktaUserInfo> GetOktaUserInfoAsync(this ISession session)
        {
            string userInfoJson = session.GetString(UserInfoSessionKey);
            if (!string.IsNullOrEmpty(userInfoJson))
            {
                return JsonConvert.DeserializeObject<OktaUserInfo>(userInfoJson);
            }

            return null;
        }

        /// <summary>
        /// Registers the Okta Identity Engine middleware.
        /// </summary>
        /// <param name="applicationBuilder">The application builder.</param>
        /// <returns>A reference to this instance after the operation completes.</returns>
        public static IApplicationBuilder UseOktaIdentityEngine(this IApplicationBuilder applicationBuilder, string successPath)
        {
            return applicationBuilder.UseMiddleware<OktaIdentityEngineMiddleware>(successPath);
        }

        public static IServiceCollection AddOktaIdentityEngine(this IServiceCollection services)
        {
            return services
                    .AddSingleton<ILogger>(NullLogger.Instance)
                    .AddSingleton<IOktaConfigurationProvider, ProfileOktaConfigurationProvider>()
                    .AddSingleton<IIdxClient>(services =>
                    {
                        IOktaConfigurationProvider oktaConfigurationProvider = services.GetService<IOktaConfigurationProvider>();
                        return new IdxClient(oktaConfigurationProvider.GetConfiguration<IdxConfiguration>(ConfigurationSection));
                    })
                    .AddSingleton(services => (IdxClient)services.GetService<IIdxClient>())
                    .AddSingleton<IIdxContextProvider, InMemoryIdxContextProvider>()
                    .AddSingleton<IOktaSignInWidgetConfigurationProvider, OktaSignInWidgetConfigurationProvider>();
        }

        public static IServiceCollection AddOktaIdentityEngine<TOktaConfigurationProvider>(this IServiceCollection services)
            where TOktaConfigurationProvider : class, IOktaConfigurationProvider
        {
            return services
                        .AddSingleton<ILogger>(NullLogger.Instance)
                        .AddSingleton<IOktaConfigurationProvider, TOktaConfigurationProvider>()
                        .AddSingleton<IIdxClient>(services =>
                        {
                            IOktaConfigurationProvider oktaConfigurationProvider = services.GetService<IOktaConfigurationProvider>();
                            return new IdxClient(oktaConfigurationProvider.GetConfiguration<IdxConfiguration>(ConfigurationSection));
                        })
                        .AddSingleton<IIdxContextProvider, InMemoryIdxContextProvider>()
                        .AddSingleton<IOktaSignInWidgetConfigurationProvider, OktaSignInWidgetConfigurationProvider>();
        }

        public static IServiceCollection AddOktaIdentityEngine<TOktaConfigurationProvider, TIdxStateProvider>(this IServiceCollection services)
            where TOktaConfigurationProvider : class, IOktaConfigurationProvider
            where TIdxStateProvider : class, IIdxContextProvider
        {
            return services
                        .AddSingleton<ILogger>(NullLogger.Instance)
                        .AddSingleton<IOktaConfigurationProvider, TOktaConfigurationProvider>()
                        .AddSingleton<IIdxClient>(services =>
                        {
                            IOktaConfigurationProvider oktaConfigurationProvider = services.GetService<IOktaConfigurationProvider>();
                            return new IdxClient(oktaConfigurationProvider.GetConfiguration<IdxConfiguration>(ConfigurationSection));
                        })
                        .AddSingleton<IIdxContextProvider, TIdxStateProvider>()
                        .AddSingleton<IOktaSignInWidgetConfigurationProvider, OktaSignInWidgetConfigurationProvider>();
        }

        public static IServiceCollection AddOktaIdentityEngine<TOktaConfigurationProvider, TIdxStateProvider, TLogger>(this IServiceCollection services)
            where TOktaConfigurationProvider : class, IOktaConfigurationProvider
            where TIdxStateProvider : class, IIdxContextProvider
            where TLogger : class, ILogger
        {
            return services
                        .AddSingleton<ILogger, TLogger>()
                        .AddSingleton<IOktaConfigurationProvider, TOktaConfigurationProvider>()
                        .AddSingleton<IIdxClient>(services =>
                        {
                            IOktaConfigurationProvider oktaConfigurationProvider = services.GetService<IOktaConfigurationProvider>();
                            return new IdxClient(oktaConfigurationProvider.GetConfiguration<IdxConfiguration>(ConfigurationSection));
                        })
                        .AddSingleton<IIdxContextProvider, TIdxStateProvider>()
                        .AddSingleton<IOktaSignInWidgetConfigurationProvider, OktaSignInWidgetConfigurationProvider>();
        }
    }
}
