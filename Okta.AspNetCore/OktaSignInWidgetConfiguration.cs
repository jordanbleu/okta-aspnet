using Newtonsoft.Json;
using Okta.Idx.Sdk;
using Okta.Idx.Sdk.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.AspNetCore
{
    public class OktaSignInWidgetConfiguration
    {
        public const string DefaultVersion = "5.5.2";

        private readonly OktaSignInWidgetConfigurationValidator signInWidgetConfigurationValidator;

        public OktaSignInWidgetConfiguration(OktaConfiguration oktaConfiguration, IIdxContext idxContext, string version = DefaultVersion)
        {
            this.UseInteractionCodeFlow = true;
            this.Version = version;

            this.signInWidgetConfigurationValidator = new OktaSignInWidgetConfigurationValidator();

            this.BaseUrl = oktaConfiguration?.Issuer?.Split("/oauth2")[0];
            this.ClientId = oktaConfiguration?.ClientId;
            this.RedirectUri = oktaConfiguration?.RedirectUri;
            this.AuthParams = new OktaSignInWidgetAuthParams(oktaConfiguration);

            this.InteractionHandle = idxContext.InteractionHandle;
            this.State = idxContext.State;
            this.IdxContext = idxContext;
            this.CodeChallenge = idxContext.CodeChallenge;
            this.CodeChallengeMethod = idxContext.CodeChallengeMethod;
        }

        [JsonIgnore]
        public IIdxContext IdxContext{ get; set; }

        [JsonProperty("interactionHandle")]
        public string InteractionHandle { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("baseUrl")]
        public string BaseUrl { get; set; }

        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("redirectUri")]
        public string RedirectUri { get; set; }

        [JsonProperty("authParams")]
        public OktaSignInWidgetAuthParams AuthParams { get; set; }

        [JsonProperty("useInteractionCodeFlow")]
        public bool UseInteractionCodeFlow { get; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("codeChallenge")]
        public string CodeChallenge { get; set; }

        [JsonProperty("codeChallengeMethod")]
        public string CodeChallengeMethod { get; set; }

        public OktaSignInWidgetValidationResult Validate()
        {
            return signInWidgetConfigurationValidator.Validate(this);
        }
    }
}
