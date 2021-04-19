using Okta.Idx.Sdk;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Okta.AspNetCore
{
    public class OktaProfile
    {
        public OktaProfile(OktaSignInEventArgs oktaSignInEventArgs)
        {
            this.OktaSignInEventArgs = oktaSignInEventArgs;
        }

        protected OktaSignInEventArgs OktaSignInEventArgs { get; set; }

        protected IIdxContext IdxContext { get => OktaSignInEventArgs?.IdxContext; }

        protected IdxClient IdxClient { get => OktaSignInEventArgs?.IdxClient as IdxClient; }

        public OktaConfiguration Configuration { get => OktaSignInEventArgs?.Configuration; }

        public OktaTokens Tokens { get => OktaSignInEventArgs.Tokens; }

        public string AccessToken { get => OktaSignInEventArgs.AccessToken; }

        public string IdToken { get => OktaSignInEventArgs.IdToken; }

        public string RefreshToken { get => OktaSignInEventArgs.RefreshToken; }

        OktaUserInfo oktaUserInfo;
        object oktaUserInfoLock = new object();

        public OktaUserInfo UserInfo
        {
            get
            {
                if (oktaUserInfo == null)
                {
                    lock (oktaUserInfoLock)
                    {
                        if (oktaUserInfo == null)
                        {
                            oktaUserInfo = OktaIdentityEngineMiddleware.GetUserInfoAsync(Configuration, AccessToken, OktaSignInEventArgs.Logger).Result;
                        }
                    }
                }

                return oktaUserInfo;
            }
        }

        public Dictionary<string, object> Introspection 
        {
            get;
            set;
        }
    }
}
