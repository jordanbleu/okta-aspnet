using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.AspNetCore
{
    public class OktaUserInfoEventArgs : EventArgs
    {
        public ILogger Logger { get; set; }
        public Exception Exception { get; set; }
    }
}
