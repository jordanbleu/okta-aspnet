// <copyright file="ProfileIdxConfigurationProvider.cs" company="Okta, Inc">
// Copyright (c) 2020 - present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Net.Http;
using FlexibleConfiguration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Okta.Idx.Sdk.Configuration;
using Okta.Sdk.Abstractions;

namespace Okta.AspNetCore
{
    /// <summary>
    /// A configuration provider that reads configuration from ~/.okta/okta.yaml.
    /// </summary>
    public class ProfileOktaConfigurationProvider : IOktaConfigurationProvider
    {
        /// <inheritdoc/>
        public OktaConfiguration GetOktaConfiguration()
        {
            var configBuilder = new ConfigurationBuilder()
                .AddYamlFile(HomePath.Resolve("~", ".okta", "okta.yaml"), optional: true);

            var compiledConfig = new OktaConfiguration();
            configBuilder.Build().GetSection("okta").GetSection("client").Bind(compiledConfig);

            return compiledConfig;
        }

        public T GetConfiguration<T>(string oktaSection)
        {
            var configBuilder = new ConfigurationBuilder()
                .AddYamlFile(HomePath.Resolve("~", ".okta", "okta.yaml"), optional: true);

            var compiledConfig = new OktaConfiguration();
            configBuilder.Build().GetSection("okta").GetSection(oktaSection).Bind(compiledConfig);

            string json = JsonConvert.SerializeObject(compiledConfig);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
