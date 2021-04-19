using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Okta.AspNetCore
{
    public class OktaPrincipal : ClaimsPrincipal
    {
        public OktaPrincipal(OktaUserInfo oktaUserInfo)
        {
            this.Identity = new OktaIdentity(oktaUserInfo);
        }

        public IIdentity Identity
        {
            get;
            private set;
        }

        public override bool IsInRole(string role)
        {
            return false; // TODO: properly implement this based on profile data
        }
    }
}
