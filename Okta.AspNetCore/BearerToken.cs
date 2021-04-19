// <copyright file="BearerToken.cs" company="Okta, Inc">
// Copyright (c) 2018-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Okta.AspNetCore
{
    public class BearerToken
    {
        public BearerToken(string jwtToken)
        {
            if (string.IsNullOrEmpty(jwtToken))
            {
                return;
            }

            string[] strArray = jwtToken.Split('.');
            if (strArray.Length != 3)
            {
                return;
            }

            this.Base64UrlEncodedHeader = strArray[0];
            this.Base64UrlEncodedPayload = strArray[1];
            this.Signature = strArray[2];
        }

        public string Header => !string.IsNullOrEmpty(this.Base64UrlEncodedHeader) ? Base64UrlEncoder.Decode(this.Base64UrlEncodedHeader) : "";

        public string Payload => !string.IsNullOrEmpty(this.Base64UrlEncodedPayload) ? Base64UrlEncoder.Decode(this.Base64UrlEncodedPayload) : "";

        public override string ToString() => this.Base64UrlEncodedHeader + "." + this.Base64UrlEncodedPayload + "." + this.Signature;

        protected string Base64UrlEncodedHeader { get; set; }

        protected string Base64UrlEncodedPayload { get; set; }

        public string Signature { get; set; }

        public Dictionary<string, object> GetClaims() => JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(BearerTokenClaims.FromBearerToken(this)));
    }
}
