using Okta.Idx.Sdk;
using Okta.Idx.Sdk.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Okta.AspNetCore
{
    public interface IOktaSignInWidgetConfigurationProvider
    {
        Task<OktaSignInWidgetConfiguration> GetOktaSignInWidgetConfigurationAsync(string state = null, string version = OktaSignInWidgetConfiguration.DefaultVersion);
    }
}
