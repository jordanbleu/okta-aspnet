// <copyright file="OktaIdentityEngineMiddleware.cs" company="Okta, Inc">
// Copyright (c) 2018-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Okta.Idx.Sdk;

namespace Okta.AspNetCore
{
    public class OktaIdentityEngineMiddleware
    {
        private static Lazy<string> userAgent = new Lazy<string>(() => $"Okta-AspNetCore-IdentityEngine-Middleware/{Assembly.GetExecutingAssembly().GetName().Version}");

        private readonly RequestDelegate next;
        private readonly HttpClient httpClient;
        private string successPath;

        public OktaIdentityEngineMiddleware(RequestDelegate next, string successPath)
        {
            this.next = next;
            this.httpClient = new HttpClient();
            this.successPath = successPath;
        }

        public static event EventHandler<OktaSignInEventArgs> SignInSuccess;

        public static event EventHandler<OktaSignInEventArgs> SignInError;

        public static event EventHandler<OktaUserInfoEventArgs> UserInfoError;

        public static string UserAgent => userAgent.Value;

        public Task InvokeAsync(HttpContext httpContext, IdxClient idxClient, IOktaConfigurationProvider configurationProvider, IIdxContextProvider idxContextProvider, ILogger logger)
        {
            if (httpContext.Session != null)
            {
                OktaUserInfo oktaUserInfo = httpContext.GetOktaUserInfoAsync().Result;
                if (oktaUserInfo != null)
                {
                    string oktaTokensKey = httpContext.Session.GetString(OktaIdentityEngineExtensions.OktaTokensKey);
                    OktaTokens tokens = idxContextProvider.Get<OktaTokens>(oktaTokensKey);
                    ClaimsIdentity claimsIdentity = new OktaIdentity(oktaUserInfo, tokens);
                    GenericPrincipal userPrincipal = new GenericPrincipal(claimsIdentity, new string[] { }); // TODO: populate roles
                    httpContext.User = userPrincipal;
                }
            }

            if (httpContext.Request.Query.ContainsKey("interaction_code"))
            {
                OktaConfiguration oktaConfiguration = configurationProvider.GetOktaConfiguration();
                Uri configuredRedirectUri = new Uri(oktaConfiguration.RedirectUri);
                if (configuredRedirectUri.AbsolutePath.Equals(httpContext.Request.Path))
                {
                    httpContext.Response.Redirect(this.successPath);
                    return this.GetTokensAsync(httpContext, idxClient, oktaConfiguration, idxContextProvider, logger);
                }
            }

            return this.next(httpContext);
        }

        internal static async Task<OktaUserInfo> GetUserInfoAsync(OktaConfiguration oktaConfiguration, string accessToken, ILogger logger)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                Uri userInfoUri = null;
                string domain = oktaConfiguration.OktaDomain;
                if (!domain.EndsWith("/"))
                {
                    domain += "/";
                }

                if (!string.IsNullOrEmpty(oktaConfiguration.AuthorizationServerId))
                {
                    userInfoUri = new Uri($"{domain}oauth2/{oktaConfiguration.AuthorizationServerId}/v1/userinfo");
                }
                else
                {
                    userInfoUri = new Uri($"{domain}oauth2/v1/userInfo");
                }

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, userInfoUri);
                httpRequestMessage.Headers.Add("Accept", "application/json");
                httpRequestMessage.Headers.Add("Authorization", $"Bearer {accessToken}");
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    UserInfoError?.Invoke(typeof(OktaIdentityEngineMiddleware), new OktaUserInfoEventArgs { Logger = logger });
                }

                string responseJson = await httpResponseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<OktaUserInfo>(responseJson);
            }
            catch (Exception ex)
            {
                UserInfoError?.Invoke(typeof(OktaIdentityEngineMiddleware), new OktaUserInfoEventArgs { Logger = logger, Exception = ex });
            }

            return null;
        }

        private async Task GetTokensAsync(HttpContext httpContext, IdxClient idxClient, OktaConfiguration oktaConfiguration, IIdxContextProvider idxContextProvider, ILogger logger)
        {
            logger = logger ?? NullLogger.Instance;
            try
            {
                string state = httpContext.Request.Query["state"];
                IdxContext idxContext = idxContextProvider.Get<IdxContext>(state);

                string interactionCode = httpContext.Request.Query["interaction_code"];
                Uri tokenUri = new Uri($"{oktaConfiguration.OktaDomain}/oauth2/{oktaConfiguration.AuthorizationServerId}/v1/token");
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, tokenUri);

                StringBuilder requestContent = new StringBuilder();
                this.AddParameter(requestContent, "grant_type", "interaction_code", false);
                this.AddParameter(requestContent, "client_id", oktaConfiguration.ClientId, true);
                if (!string.IsNullOrEmpty(oktaConfiguration.ClientSecret))
                {
                    this.AddParameter(requestContent, "client_secret", oktaConfiguration.ClientSecret, true);
                }

                this.AddParameter(requestContent, "interaction_code", interactionCode, true);
                this.AddParameter(requestContent, "code_verifier", idxContext.CodeVerifier, true);

                requestMessage.Content = new StringContent(requestContent.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
                requestMessage.Headers.Add("Accept", "application/json");
                HttpResponseMessage responseMessage = await this.httpClient.SendAsync(requestMessage);
                string tokenResponseJson = await responseMessage.Content.ReadAsStringAsync();
                if (!responseMessage.IsSuccessStatusCode)
                {
                    SignInError?.Invoke(this, new OktaSignInEventArgs { ErrorResponse = JsonConvert.DeserializeObject(tokenResponseJson), Configuration = oktaConfiguration, Logger = logger });
                    logger.LogError(tokenResponseJson);
                }
                else
                {
                    OktaTokens tokens = JsonConvert.DeserializeObject<OktaTokens>(tokenResponseJson);
                    OktaSignInEventArgs signInEventArgs = new OktaSignInEventArgs { IdxClient = idxClient, IdxContext = idxContext, HttpContext = httpContext, Configuration = oktaConfiguration, Tokens = tokens, Logger = logger };

                    if (httpContext.Session != null)
                    {
                        httpContext.Session.SetString(OktaIdentityEngineExtensions.UserInfoSessionKey, JsonConvert.SerializeObject(signInEventArgs.ProfileData.UserInfo));
                        string tokenKey = $"okta_tokens_{state}";
                        httpContext.Session.SetString(OktaIdentityEngineExtensions.OktaTokensKey, tokenKey);
                        idxContextProvider.Set(tokenKey, tokens);
                    }

                    httpContext.User = new OktaPrincipal(signInEventArgs.ProfileData.UserInfo);

                    SignInSuccess?.Invoke(this, signInEventArgs);
                }
            }
            catch (Exception ex)
            {
                SignInError?.Invoke(this, new OktaSignInEventArgs { Exception = ex, Configuration = oktaConfiguration, Logger = logger });
                logger.LogError("Exception occurred getting tokens: {0}", ex.Message);
            }

            await Task.CompletedTask;
        }

        private void AddParameter(StringBuilder stringBuilder, string key, string value, bool ampersandPrefix = false)
        {
            if (ampersandPrefix)
            {
                stringBuilder.Append("&");
            }

            stringBuilder.Append($"{key}={value}");
        }
    }
}
