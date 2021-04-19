using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.AspNetCore
{
    public enum TokenKind
    {
        /// <summary>
        /// Invalid token.
        /// </summary>
        Invalid,

        /// <summary>
        /// ID token.
        /// </summary>
        IdToken,

        /// <summary>
        /// Access token.
        /// </summary>
        AccessToken,

        /// <summary>
        /// Refresh token.
        /// </summary>
        RefreshToken,
    }
}
