using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class ReceiveSSOResult
    {
        public bool IsInResponseTo { get; private set; }
        public string PartnerIdentityProvider { get; private set; }
        public string AuthnContext { get; private set; }
        public string SamlUsername { get; private set; }
        public IDictionary<string, string> Attributes { get; private set; }
        public string TargetUrl { get; private set; }

        public ReceiveSSOResult(bool isInResponseTo, string partnerIdentityProvider, string authnContext, string samlUsername, IDictionary<string, string> attributes, string targetUrl)
        {
            IsInResponseTo = isInResponseTo;
            PartnerIdentityProvider = partnerIdentityProvider;
            AuthnContext = authnContext;
            SamlUsername = samlUsername;
            Attributes = attributes;
            TargetUrl = targetUrl;
        }
    }
}