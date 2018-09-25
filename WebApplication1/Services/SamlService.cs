using System.Collections.Generic;
using System.Web;
using ComponentSpace.SAML2;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class SamlService
    {
        public ReceiveSSOResult ReceiveSSO(HttpRequestBase request)
        {
            // Receive and process the SAML assertion contained in the SAML response.
            // The SAML response is received either as part of IdP-initiated or SP-initiated SSO.

            SAMLServiceProvider.ReceiveSSO(request,
                out var isInResponseTo,
                out var partnerIdP,
                out var authnContext,
                out var username,
                out IDictionary<string, string> attributes,
                out var targetUrl);

            return new ReceiveSSOResult(isInResponseTo, partnerIdP, authnContext, username, attributes, targetUrl);
        }
    }
}