using System;
using ComponentSpace.SAML2;
using ComponentSpace.SAML2.Configuration;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using WebApplication1.Providers;

[assembly: OwinStartupAttribute(typeof(WebApplication1.Startup))]
namespace WebApplication1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            ConfigureComponentSpace();
        }

        private void ConfigureComponentSpace()
        {
            SAMLConfiguration samlConfiguration = new SAMLConfiguration
            {
                LocalServiceProviderConfiguration = new LocalServiceProviderConfiguration()
                {
                    Name = "http://DemoEntitySPId",
                    Description = "OWIN Example Service Provider",
                    AssertionConsumerServiceUrl = "~/SAML/AssertionConsumerService",
                    LocalCertificateFile = "Certificates\\sp.pfx",
                    LocalCertificatePassword = "password"
                }
            };

            samlConfiguration.AddPartnerIdentityProvider(
                new PartnerIdentityProviderConfiguration
                {
                    Name = "http://www.okta.com/exkgbcut4bpS80zci0h7",
                    SignAuthnRequest = true,
                    SignLogoutRequest = true,
                    SignLogoutResponse = true,
                    WantSAMLResponseSigned = true,
                    WantAssertionSigned = true,
                    WantAssertionEncrypted = false,
                    WantLogoutRequestSigned = true,
                    WantLogoutResponseSigned = true,
                    SingleSignOnServiceUrl = "https://dev-257161.oktapreview.com/app/mortgagecadencedev257161_demossoapp_1/exkgbcut4bpS80zci0h7/sso/saml",
                    SingleLogoutServiceUrl = "https://dev-257161.oktapreview.com/app/mortgagecadencedev257161_demossoapp_1/exkgbcut4bpS80zci0h7/slo/saml",
                    PartnerCertificateFile = "Certificates\\okta.cert"
                });

            samlConfiguration.AddPartnerIdentityProvider(
                new PartnerIdentityProviderConfiguration
                {
                    Name = "https://sts.windows.net/5f6a1902-c4dd-435a-8cb2-122315f532bf/",
                    SignAuthnRequest = true,
                    SignLogoutRequest = true,
                    SignLogoutResponse = true,
                    WantSAMLResponseSigned = true,
                    WantAssertionSigned = true,
                    WantAssertionEncrypted = false,
                    WantLogoutRequestSigned = true,
                    WantLogoutResponseSigned = true,
                    SingleSignOnServiceUrl = "https://login.microsoftonline.com/5f6a1902-c4dd-435a-8cb2-122315f532bf/saml2",
                    SingleLogoutServiceUrl = "https://login.microsoftonline.com/5f6a1902-c4dd-435a-8cb2-122315f532bf/saml2",
                    PartnerCertificateFile = "Certificates\\azure.cert"
                });

            SAMLController.Configuration = samlConfiguration;
        }

        private void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Account/Login"),
                LogoutPath = new PathString("/account/logout"),
                CookieName = "DemoAppCookie",
            });

            //app.UseExternalSignInCookie("ExternalCookie");

            // Configure the application for OAuth based flow
            const string publicClientId = "self";
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(publicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = true
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthAuthorizationServer(options);

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
