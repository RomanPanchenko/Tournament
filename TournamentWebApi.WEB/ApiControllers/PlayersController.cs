using System.Net;
using System.Net.Http;
using System.Web.Http;
using TournamentWebApi.BLL.Interfaces;
using TournamentWebApi.WEB.Filters;
using TournamentWebApi.WEB.Security;

namespace TournamentWebApi.WEB.ApiControllers
{
    [ApiExceptionLoggingFilter]
    public class PlayersController : BaseController
    {
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        // GET api/<controller>
        [HttpGet]
        [CustomApiAuthorize(Roles = "Admin, Manager")]
        public HttpResponseMessage Get()
        {
            var players = _playerService.GetAllPlayers();
            return Request.CreateResponse(HttpStatusCode.OK, players);
        }

        // GET api/<controller>/5
        [HttpGet]
        [CustomApiAuthorize(Roles = "Admin, Manager")]
        public HttpResponseMessage Get(int id)
        {
            var playerModel = _playerService.Get(id);
            return Request.CreateResponse(HttpStatusCode.OK, playerModel);
        }
    }
}