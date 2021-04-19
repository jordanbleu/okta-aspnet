// <copyright file="DelegateAuthenticationArguments.cs" company="Okta, Inc">
// Copyright (c) 2018-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Okta.AspNetCore
{
    public class OktaAuthenticationServiceEventArgs
    : EventArgs
    {
        public string AuthenticationServiceMethodName { get; set; }
        public HttpContext HttpContext { get; set; }

        public string Scheme { get; set; }

        public AuthenticationProperties AuthenticationProperties { get; set; }

        public ClaimsPrincipal ClaimsPrincipal { get; set; }

        public Exception Exception { get; set; }

        public Task Task { get; set; }
    }
}
