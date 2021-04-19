using Microsoft.Extensions.Logging;
using Okta.Idx.Sdk.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Okta.AspNetCore
{
    /// <summary>
    /// Interface to represent a provider of Okta specific configuration.
    /// </summary>
    public interface IOktaConfigurationProvider

    {
        /// <summary>
        /// Gets an Okta configuration.
        /// </summary>
        /// <returns>IdxConfiguration</returns>
        OktaConfiguration GetOktaConfiguration();

        T GetConfiguration<T>(string oktaSection);
    }
}
