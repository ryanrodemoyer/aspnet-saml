using ComponentSpace.SAML2;
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
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserService _userService = new UserService();
        private readonly RoleService _roleService = new RoleService();
        private readonly ConfigService _configService = new ConfigService();

        [AllowAnonymous]
        public ActionResult SSOAuto()
        {
            string idp = _configService.GetByKey(AppSettings.PartnerIdPAzureAD);

            // To login at the service provider, initiate single sign-on to the identity provider (SP-initiated SSO).
            SAMLServiceProvider.InitiateSSO(Response, null, idp);

            return new EmptyResult();
        }

        [AllowAnonymous]
        public ActionResult SingleSignOn(string provider)
        {
            string idp;
            switch (provider)
            {
                case "okta":
                    idp = _configService.GetByKey(AppSettings.PartnerIdPOkta);
                    break;
                case "azuread":
                    idp = _configService.GetByKey(AppSettings.PartnerIdPAzureAD);
                    break;
                default:
                    throw new InvalidOperationException("provider type not supported");
            }

            // To login at the service provider, initiate single sign-on to the identity provider (SP-initiated SSO).
            SAMLServiceProvider.InitiateSSO(Response, null, idp);

            return new EmptyResult();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel { Email = "example@localhost", Password = "example" });
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _userService.GetByEmail(model.Email);
            if (user == null || user.Password != model.Password)
            {
                ModelState.AddModelError("InvalidCredentials", "An account with those credentials does not exist.");
                return View(model);
            }

            //check username and password from database, naive checking: password should be in SHA
            //if (user.Password == model.Password)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("user_type", "database")
                };

                var identity = new ClaimsIdentity(claims, "ApplicationCookie");

                // Add roles into claims
                var roles = _roleService.GetByUserId(user.Id);
                if (roles.Any())
                {
                    var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r.Name));
                    identity.AddClaims(roleClaims);
                }

                var props = new AuthenticationProperties { IsPersistent = model.RememberMe };
                Request.GetOwinContext().Authentication.SignIn(props, identity);

                return RedirectToAction("Protected", "Home");
            }
        }

        public ActionResult Claims()
        {
            return View(HttpContext.GetOwinContext().Authentication.User.Claims);
        }

        [HttpPost]
        public ActionResult LogOut()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");

            // this code block is to allow our app (SP) to sign the user out of their IdP as well as our app
            //if (SAMLServiceProvider.CanSLO())
            //{
            //    // Request logout at the identity provider.
            //    SAMLServiceProvider.InitiateSLO(Response, null, null);

            //    return new EmptyResult();
            //}

            return RedirectToAction("Index", "Home");
        }
    }
}