using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TournamentWebApi.BLL.Interfaces;
using TournamentWebApi.BLL.Models;
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
        //[CustomApiAuthorize(Roles = "Admin, Manager")]
        public HttpResponseMessage Get()
        {
            var httpStatusCode = HttpStatusCode.OK;
            IEnumerable<PlayerModel> allPlayers = _playerService.GetAllPlayers();
            PlayerListModel playerListModel = _playerService.GetPlayerListModel(allPlayers);
            if (!playerListModel.Items.Any())
            {
                httpStatusCode = HttpStatusCode.NotFound;
            }

            return Request.CreateResponse(httpStatusCode, playerListModel);
        }

        // GET api/<controller>/5
        [HttpGet]
        //[CustomApiAuthorize(Roles = "Admin, Manager")]
        public HttpResponseMessage Get(int id)
        {
            var httpStatusCode = HttpStatusCode.OK;
            var playerModel = _playerService.Get(id);
            if (playerModel == null)
            {
                httpStatusCode = HttpStatusCode.NotFound;
            }

            return Request.CreateResponse(httpStatusCode, playerModel);
        }
    }
}