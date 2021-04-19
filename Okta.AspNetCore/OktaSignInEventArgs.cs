// <copyright file="OktaSignInEventArgs.cs" company="Okta, Inc">
// Copyright (c) 2018-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Okta.Idx.Sdk;

namespace Okta.AspNetCore
{
    /// <summary>
    /// Class encapsulating data relevant to a sign in event.
    /// </summary>
    public class OktaSignInEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OktaSignInEventArgs"/> class.
        /// </summary>
        public OktaSignInEventArgs()
        {
            this.Tokens = new OktaTokens();
            this.ProfileData = new OktaProfile(this);
            this.Logger = NullLogger.Instance;
        }

        /// <summary>
        /// Gets or sets the IdxClient.
        /// </summary>
        internal IIdxClient IdxClient { get; set; }

        internal ILogger Logger { get; set; }

        public IdxContext IdxContext { get; set; }

        public OktaProfile ProfileData { get; set; }

        public HttpContext HttpContext { get; set; }

        public OktaTokens Tokens { get; set; }

        public string AccessToken
        {
            get => Tokens?.AccessToken;
        }

        public string IdToken
        {
            get => Tokens?.IdToken;
        }

        public string RefreshToken
        {
            get => Tokens?.RefreshToken;
        }

        public bool IsSuccessful
        {
            get => ErrorResponse == null && Exception == null;
        }

        public object ErrorResponse{ get; set; }

        public Exception Exception { get; set; }

        public OktaConfiguration Configuration { get; set; }
    }
}
