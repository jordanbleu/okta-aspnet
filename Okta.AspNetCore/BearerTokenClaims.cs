using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.AspNetCore
{
    public class BearerTokenClaims
    {
        [JsonProperty("iss")]
        public string Issuer { get; set; }

        [JsonProperty("sub")]
        public string Subject { get; set; }

        [JsonProperty("aud")]
        public string Audience { get; set; }

        [JsonProperty("exp")]
        public string ExpirationTime { get; set; }

        [JsonProperty("scp")]
        public string[] Scope { get; set; }

        public static BearerTokenClaims FromBearerToken(BearerToken bearerToken) => bearerToken == null || bearerToken.Payload == null ? new BearerTokenClaims() : BearerTokenClaims.FromPayload(bearerToken.Payload);

        public static BearerTokenClaims FromPayload(string payload) => string.IsNullOrEmpty(payload) ? new BearerTokenClaims() : JsonConvert.DeserializeObject<BearerTokenClaims>(payload);
    }
}
