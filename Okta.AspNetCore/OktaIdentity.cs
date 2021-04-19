using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Okta.AspNetCore
{
    public class OktaIdentity : ClaimsIdentity
    {
        public OktaIdentity(OktaUserInfo userInfo, OktaTokens oktaTokens = null)
        {
            this.OktaUserInfo = userInfo;
            this.OktaTokens = oktaTokens;
            if ((bool)oktaTokens?.TokenType?.Equals("Bearer"))
            {
                BearerToken = new BearerToken(oktaTokens.AccessToken);
            }
        }

        protected OktaUserInfo OktaUserInfo { get; set; }

        public OktaTokens OktaTokens { get; set; }

        public BearerToken BearerToken { get; set; }

        public override string AuthenticationType
        {
            get => "OktaIdentityEngine";
        }

        public override bool IsAuthenticated
        {
            get
            {
                return !string.IsNullOrEmpty(this.OktaUserInfo?.PreferredUserName);
            }
        }

        public override string Name
        {
            get
            {
                return OktaUserInfo?.PreferredUserName;
            }
        }
    }
}
