using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.AspNetCore
{
    public class OktaSignInWidgetConfigurationValidator
    {
        private List<string> messages;

        public OktaSignInWidgetValidationResult Validate(OktaSignInWidgetConfiguration signInWidgetConfiguration)
        {
            this.messages = new List<string>();
            if (signInWidgetConfiguration == null)
            {
                this.AddMessage($"{nameof(OktaSignInWidgetConfiguration)} was not specified.");
            }

            if (string.IsNullOrEmpty(signInWidgetConfiguration.ClientId))
            {
                this.AddMessage($"{nameof(OktaSignInWidgetConfiguration)}.{nameof(OktaSignInWidgetConfiguration.ClientId)} was not specified");
            }

            if (string.IsNullOrEmpty(signInWidgetConfiguration.RedirectUri))
            {
                this.AddMessage($"{nameof(OktaSignInWidgetConfiguration)}.{nameof(OktaSignInWidgetConfiguration.RedirectUri)} was not specified");
            }

            if (string.IsNullOrEmpty(signInWidgetConfiguration?.CodeChallenge))
            {
                this.AddMessage($"{nameof(OktaSignInWidgetConfiguration)}.{nameof(OktaSignInWidgetConfiguration.CodeChallenge)}  was not specified");
            }

            if (string.IsNullOrEmpty(signInWidgetConfiguration?.CodeChallengeMethod))
            {
                this.AddMessage($"{nameof(OktaSignInWidgetConfiguration)}.{nameof(OktaSignInWidgetConfiguration.CodeChallengeMethod)}  was not specified");
            }

            return new OktaSignInWidgetValidationResult { Messages = messages };
        }

        private void AddMessage(string message)
        {
            this.messages.Add(message);
        }
    }
}
