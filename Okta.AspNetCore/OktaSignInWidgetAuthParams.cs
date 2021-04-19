// <copyright file="OktaSignInWidgetAuthParams.cs" company="Okta, Inc">
// Copyright (c) 2018-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Okta.AspNetCore
{
    /// <summary>
    /// Class containg issuer and scopes to pass to the sign in widget.
    /// </summary>
    public class OktaSignInWidgetAuthParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OktaSignInWidgetAuthParams"/> class.
        /// </summary>
        public OktaSignInWidgetAuthParams()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OktaSignInWidgetAuthParams"/> class.
        /// </summary>
        /// <param name="oktaConfiguration">The Okta configuration.</param>
        public OktaSignInWidgetAuthParams(OktaConfiguration oktaConfiguration)
        {
            this.Issuer = oktaConfiguration?.Issuer;
            this.Scopes = oktaConfiguration?.Scopes;
        }

        /// <summary>
        /// Gets or sets the issuer.
        /// </summary>
        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the scopes.
        /// </summary>
        [JsonProperty("scopes")]
        public List<string> Scopes { get; set; }
    }
}
