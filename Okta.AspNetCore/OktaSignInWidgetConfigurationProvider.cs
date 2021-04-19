using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Okta.Idx.Sdk;
using Okta.Idx.Sdk.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Okta.AspNetCore
{
    public class OktaSignInWidgetConfigurationProvider : IOktaSignInWidgetConfigurationProvider
    {
        private IdxClient idxClient;
        private IOktaConfigurationProvider oktaConfigurationProvider;
        private IIdxContextProvider oktaApplicationSessionProvider;

        public OktaSignInWidgetConfigurationProvider(IdxClient idxClient, IOktaConfigurationProvider oktaConfigurationProvider, IIdxContextProvider oktaApplicationSessionProvider)
        {
            this.idxClient = idxClient;
            this.oktaConfigurationProvider = oktaConfigurationProvider;
            this.oktaApplicationSessionProvider = oktaApplicationSessionProvider;
        }

        public async Task<OktaSignInWidgetConfiguration> GetOktaSignInWidgetConfigurationAsync(string state = null, string version = OktaSignInWidgetConfiguration.DefaultVersion)
        {
            IIdxContext idxContext = await this.idxClient.InteractAsync(state);
            this.oktaApplicationSessionProvider.Set(idxContext.State, idxContext);
            OktaConfiguration oktaConfiguraiton = this.oktaConfigurationProvider.GetOktaConfiguration();
            return new OktaSignInWidgetConfiguration(oktaConfiguraiton, idxContext);
        }
    }
}
