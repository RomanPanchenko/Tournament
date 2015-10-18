using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Ninject;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.WEB.Filters;
using TournamentWebApi.WEB.Interfaces;
using TournamentWebApi.WEB.Models;

namespace TournamentWebApi.WEB.ApiControllers
{
    [ApiExceptionLoggingFilter]
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController()
        {
            _authenticationService = NinjectWebCommon.Kernel.Get<IAuthenticationService>();
        }

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        // POST api/<controller>
        [HttpPost]
        [Route("api/authentication")]
        public async Task<HttpResponseMessage> Post(LoginModel loginModel)
        {
            HttpResponse response = HttpContext.Current.Response;
            if (ModelState.IsValid)
            {
                AccountModel accountModel = await _authenticationService.Login(response, loginModel);
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.Forbidden);
        }

        // POST api/<controller>
        [HttpDelete]
        [Route("api/authentication")]
        public HttpResponseMessage Delete()
        {
            HttpResponse response = HttpContext.Current.Response;
            _authenticationService.Logout(response);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}