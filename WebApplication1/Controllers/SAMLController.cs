using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class SAMLController : Controller
    {
        private readonly UserService _userService = new UserService();

        [HttpPost]
        public ActionResult AssertionConsumerService()
        {
            var result = new SamlService().ReceiveSSO(Request);

            // Automatically provision the user.
            // If the user doesn't exist locally then create the user.
            var user = _userService.GetByUsername(result.SamlUsername);
            if (user == null)
            {
                throw new InvalidOperationException($"{result.SamlUsername} not found");
            }

            if (user.AuthType == AuthType.Database)
            {
                throw new InvalidOperationException("user not available to SSO");
            }

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("user_type", "sso"),
            };

            var identity = new ClaimsIdentity(claims, "ApplicationCookie");

            // Add roles into claims
            var roles = new RoleService().GetByUserId(user.Id);
            if (roles.Any())
            {
                var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r.Name));
                identity.AddClaims(roleClaims);
            }

            var ssoclaims = result.Attributes?.Select(x => new Claim($"SSO_{x.Key}", x.Value));
            if (ssoclaims != null)
                identity.AddClaims(ssoclaims);

            // Automatically login using the asserted identity.
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);

            // Redirect to the target URL if supplied.
            if (!string.IsNullOrEmpty(result.TargetUrl))
            {
                return Redirect(result.TargetUrl);
            }

            return RedirectToAction("Protected", "Home");
        }

        //public ActionResult SLOService()
        //{
        //    // Receive the single logout request or response.
        //    // If a request is received then single logout is being initiated by the identity provider.
        //    // If a response is received then this is in response to single logout having been initiated by the service provider.
        //    bool isRequest = false;
        //    string logoutReason = null;
        //    string partnerIdP = null;
        //    string relayState = null;

        //    SAMLServiceProvider.ReceiveSLO(Request, out isRequest, out logoutReason, out partnerIdP, out relayState);

        //    if (isRequest)
        //    {
        //        // Logout locally.
        //        HttpContext.GetOwinContext().Authentication.SignOut("ApplicationCookie");

        //        // Respond to the IdP-initiated SLO request indicating successful logout.
        //        SAMLServiceProvider.SendSLO(Response, null);
        //    }
        //    else
        //    {
        //        // SP-initiated SLO has completed.
        //        return RedirectToAction("Index", "Home");
        //    }

        //    return new EmptyResult();
        //}
    }
}