using System;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using AutoMapper;
using Newtonsoft.Json;
using TournamentWebApi.WEB.Mappings;
using TournamentWebApi.WEB.Security;

namespace TournamentWebApi.WEB
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Mapping.InitMapping();
            Mapper.AssertConfigurationIsValid();
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                try
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                    if (authTicket != null)
                    {
                        var serializeModel = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);

                        var newUser = new CustomPrincipal(authTicket.Name)
                        {
                            UserId = serializeModel.UserId,
                            FirstName = serializeModel.FirstName,
                            LastName = serializeModel.LastName,
                            Roles = serializeModel.Roles
                        };

                        HttpContext.Current.User = newUser;
                    }
                }
                catch (CryptographicException ex)
                {
                    FormsAuthentication.SignOut();
                }
            }
        }
    }
}