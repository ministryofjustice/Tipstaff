using System.Configuration;
using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Host.SystemWeb;

namespace Tipstaff
{
    public partial class Startup
    {
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static string tenantId = ConfigurationManager.AppSettings["ida:TenantId"];
        private static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];
        private static string RedirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
        //private string authority = aadInstance + tenantId;


        public void ConfigureAuth(IAppBuilder app)
        {
            if (string.IsNullOrEmpty(RedirectUri))
            {
                RedirectUri = "https://localhost:44300/";
            }

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            //app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies",
                CookieManager = new SystemWebChunkingCookieManager()
                
            });

            OpenIdConnectAuthenticationOptions options = new OpenIdConnectAuthenticationOptions
            {
                ClientId = clientId,
               
                Authority = aadInstance + tenantId//,
                //PostLogoutRedirectUri = postLogoutRedirectUri,
                //RedirectUri = RedirectUri
            };

            app.UseOpenIdConnectAuthentication(options);
            


        }
    }
}