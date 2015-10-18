using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using AutoMapper;
using Newtonsoft.Json;
using Ninject;
using TournamentWebApi.WEB.DependencyResolver;
using TournamentWebApi.WEB.Interfaces;
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
            if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
            {
                try
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                    if (authTicket != null)
                    {
                        if (!authTicket.Expired)
                        {
                            var principalModel = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);

                            var newUser = new CustomPrincipal(authTicket.Name)
                            {
                                UserId = principalModel.UserId,
                                FirstName = principalModel.FirstName,
                                LastName = principalModel.LastName,
                                Roles = principalModel.Roles
                            };

                            HttpContext.Current.User = newUser;

                            var authenticationService = NinjectWebCommon.Kernel.Get<IAuthenticationService>();
                            authenticationService.ProlongateUserSession(HttpContext.Current.Response, principalModel);
                        }
                    }
                }
                catch (Exception ex)
                {
                    FormsAuthentication.SignOut();
                }
            }
        }
    }
}