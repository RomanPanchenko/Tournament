using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using Newtonsoft.Json;
using TournamentWebApi.BLL.Interfaces;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.WEB.Models;

namespace TournamentWebApi.WEB.ApiControllers
{
    public class AccountController : ApiController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // POST api/<controller>
        [HttpPost]
        [Route("api/account")]
        public async Task<HttpResponseMessage> Post(LoginModel loginModel)
        {
            return await Task.Run(() =>
            {
                if (ModelState.IsValid)
                {
                    LoginUser(loginModel);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }

                return Request.CreateResponse(HttpStatusCode.Forbidden);
            });
        }

        private void LoginUser(LoginModel loginModel)
        {
            AccountModel accountModel = _accountService.Get(loginModel.Login, loginModel.Password);
            if (accountModel != null)
            {
                string userData = JsonConvert.SerializeObject(accountModel);
                var authTicket = new FormsAuthenticationTicket(
                    1,
                    accountModel.Login,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(15),
                    false, // pass here true, if you want to implement remember me functionality
                    userData);

                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                HttpContent.Current.Response.Cookies.Add(cookie);

            }
        }
    }
}