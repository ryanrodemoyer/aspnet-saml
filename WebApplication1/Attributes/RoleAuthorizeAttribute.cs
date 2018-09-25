using System.Web.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Attributes
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        public readonly string BaseRedirectUrl;

        private readonly ConfigService _configService = new ConfigService();

        public RoleAuthorizeAttribute()
        {
        }

        public RoleAuthorizeAttribute(string baseRedirectUrl = "")
        {
            BaseRedirectUrl = baseRedirectUrl;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                string authUrl = BaseRedirectUrl; //passed from attribute

                // if null, get it from config
                if (string.IsNullOrWhiteSpace(authUrl))
                {
                    authUrl = _configService.GetByKey(AppSettings.RolesAuthRedirectUrl);
                }

                if (!string.IsNullOrWhiteSpace(authUrl))
                {
                    filterContext.HttpContext.Response.Redirect(authUrl);
                }
            }

            // otherwise do normal process
            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}