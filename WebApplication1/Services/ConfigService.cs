using System.Collections.Generic;

namespace WebApplication1.Services
{
    public class ConfigService
    {
        public readonly Dictionary<string, string> Config = new Dictionary<string, string>
        {
            {AppSettings.PartnerIdPOkta, "http://www.okta.com/exkgbcut4bpS80zci0h7" },
            {AppSettings.PartnerIdPAzureAD, "https://sts.windows.net/5f6a1902-c4dd-435a-8cb2-122315f532bf/" },
            {AppSettings.RolesAuthRedirectUrl, "/home/forbidden" }
        };

        public string GetByKey(string key)
        {
            return Config[key];
        }
    }
}