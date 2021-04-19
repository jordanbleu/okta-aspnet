using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.AspNetCore
{
    public class OktaConfiguration
    {
        public static List<string> DefaultScopes = new List<string> { "openid", "profile" };

        public string OktaDomain { get; set; }

        public string AuthorizationServerId { get; set; }

        /// <summary>
        /// Gets or sets the client ID for your application.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client Secret for your application. Optional.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets a list of string based scopes.
        /// </summary>
        public List<string> Scopes { get; set; } = DefaultScopes;

        /// <summary>
        /// Gets or sets the issuer.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the redirect URI.
        /// </summary>
        public string RedirectUri { get; set; }
    }
}
