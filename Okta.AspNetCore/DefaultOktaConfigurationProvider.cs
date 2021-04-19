using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.AspNetCore
{
    public class DefaultOktaConfigurationProvider : IOktaConfigurationProvider
    {
        private IConfiguration configuration;

        public DefaultOktaConfigurationProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public T GetConfiguration<T>(string oktaSection)
        {
            return (T)configuration.GetSection(oktaSection);
        }

        public OktaConfiguration GetOktaConfiguration()
        {
            char[] scopeDelimiter = new char[] { ',', ' ' };
            string[] scopes = configuration.GetValue<string>("Okta:Scopes").Split(scopeDelimiter, StringSplitOptions.RemoveEmptyEntries);
            OktaConfiguration oktaConfiguration = new OktaConfiguration
            {
                OktaDomain = configuration.GetValue<string>("Okta:OktaDomain"),
                Issuer = configuration.GetValue<string>("Okta:Issuer"),
                ClientId = configuration.GetValue<string>("Okta:ClientId"),
                ClientSecret = configuration.GetValue<string>("Okta:ClientSecret"),
                AuthorizationServerId = configuration.GetValue<string>("Okta:AuthorizationServerId"),
                RedirectUri = configuration.GetValue<string>("Okta:RedirectUri"),
                Scopes = new List<string>(scopes)
            };

            return oktaConfiguration;
        }
    }
}
