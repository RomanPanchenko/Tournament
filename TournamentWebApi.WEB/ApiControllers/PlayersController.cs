using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TournamentWebApi.BLL.Interfaces;

namespace TournamentWebApi.WEB.ApiControllers
{
    public class PlayersController : ApiController
    {
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        // GET api/<controller>
        [HttpGet]
        public async Task<HttpResponseMessage> Get()
        {
            return await Task.Run(() =>
            {
                var players = _playerService.GetAllPlayers();
                return Request.CreateResponse(HttpStatusCode.OK, players);
            });
        }

        // GET api/<controller>/5
        [HttpGet]
        public async Task<HttpResponseMessage> Get(int id)
        {
            return await Task.Run(() =>
            {
                var playerModel = _playerService.Get(id);
                return Request.CreateResponse(HttpStatusCode.OK, playerModel);
            });
        }
    }
}