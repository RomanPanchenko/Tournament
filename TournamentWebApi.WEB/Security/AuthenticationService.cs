using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using TournamentWebApi.BLL.Interfaces;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.WEB.Interfaces;
using TournamentWebApi.WEB.Models;

namespace TournamentWebApi.WEB.Security
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAccountService _accountService;

        public AuthenticationService(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<AccountModel> Login(HttpResponse httpResponse, LoginModel loginModel)
        {
            AccountModel accountModel = await _accountService.Get(loginModel.Login, loginModel.Password);
            if (accountModel != null)
            {
                var principalModel = new CustomPrincipalSerializeModel
                {
                    FirstName = accountModel.FirstName,
                    LastName = accountModel.LastName,
                    UserId = accountModel.AccountId,
                    Roles = accountModel.Roles.Select(role => role.Name).ToArray(),
                    Login = accountModel.Login
                };

                ProlongateUserSession(httpResponse, principalModel);
            }

            return accountModel;
        }

        public void Logout(HttpResponse httpResponse)
        {
            HttpCookie authCookie = httpResponse.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                authCookie.Expires = DateTime.Now.AddDays(-1);
                authCookie.Value = null;
                httpResponse.Cookies.Remove(FormsAuthentication.FormsCookieName);
                httpResponse.SetCookie(authCookie);
            }
        }

        public void ProlongateUserSession(HttpResponse httpResponse, CustomPrincipalSerializeModel principalModel)
        {
            string userData = JsonConvert.SerializeObject(principalModel);
            var expirationTime = DateTime.Now.AddMinutes(15);

            var authTicket = new FormsAuthenticationTicket(
                1,
                principalModel.Login,
                DateTime.Now,
                expirationTime,
                false, // pass here true, if you want to implement remember me functionality
                userData);

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            httpResponse.Cookies.Remove(FormsAuthentication.FormsCookieName);
            httpResponse.SetCookie(cookie);
        }
    }
}