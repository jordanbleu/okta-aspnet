using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.AspNetCore
{
    public class OktaSignInWidgetValidationResult
    {
        public OktaSignInWidgetValidationResult()
        {
            this.Messages = new List<string>();
        }

        public List<string> Messages { get; set; }

        public bool Success { get => Messages.Count == 0; }
    }
}
