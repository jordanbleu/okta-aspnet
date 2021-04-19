using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okta.AspNetCore
{
    public static class OktaHtmlHelperExtensions
    {
        public static IHtmlContent OktaSignInWidget(this IHtmlHelper helper, OktaSignInWidgetConfiguration config, string domId = null, string version = "5.5.2")
        {
            var signInWidgetScriptTag = new TagBuilder("script");
            signInWidgetScriptTag.Attributes.Add("src", $"https://global.oktacdn.com/okta-signin-widget/{version}/js/okta-sign-in.min.js");
            signInWidgetScriptTag.Attributes.Add("type", "text/javascript");

            var styleSheetLink = new TagBuilder("link");
            styleSheetLink.Attributes.Add("href", $"https://global.oktacdn.com/okta-signin-widget/{version}/css/okta-sign-in.min.css");
            styleSheetLink.Attributes.Add("type", "text/css");
            styleSheetLink.Attributes.Add("rel", "stylesheet");

            domId = domId ?? "okta-signin-widget-container";
            var containingSpan = new TagBuilder("span"); // need to wrap the output in a span because sign in widget requires both a target div and supporting script.
            var signInWidgetDiv = new TagBuilder("div");
            signInWidgetDiv.Attributes.Add("id", domId);
            var scriptTag = new TagBuilder("script");
            scriptTag.Attributes.Add("type", "text/javascript");
            StringBuilder script = new StringBuilder();

            script.AppendLine($"const widgetConfig = {JsonConvert.SerializeObject(config)};");

            script.AppendLine("const signIn = new OktaSignIn({");
            script.AppendLine($"  el: '#{domId}',");
            script.AppendLine($"  ...widgetConfig");
            script.AppendLine("});");
            script.AppendLine("signIn.showSignInAndRedirect()");
            script.AppendLine("  .catch(err => {");
            script.AppendLine("    console.log('Error in Okta Sign In Widget: ', err);");
            script.AppendLine("});");
            scriptTag.InnerHtml.AppendHtml(helper.Raw(script.ToString()));

            containingSpan.InnerHtml.AppendHtml(signInWidgetScriptTag);
            containingSpan.InnerHtml.AppendHtml(styleSheetLink);
            containingSpan.InnerHtml.AppendHtml(signInWidgetDiv);
            containingSpan.InnerHtml.AppendHtml(scriptTag);
            return containingSpan;
        }
    }
}
