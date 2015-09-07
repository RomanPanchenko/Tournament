using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TournamentWebApi.BLL.Interfaces;
using TournamentWebApi.BLL.Models;

namespace TournamentWebApi.WEB.ApiControllers
{
    public class ScoresController : ApiController
    {
        private readonly IMatchService _matchService;
        private readonly IPlayerService _playerService;

        public ScoresController(IPlayerService playerService, IMatchService matchService)
        {
            _playerService = playerService;
            _matchService = matchService;
        }


        // GET api/scores
        [HttpGet]
        [Route("api/players/score")]
        public async Task<HttpResponseMessage> Get()
        {
            return await Task.Run(() =>
            {
                IEnumerable<ScoreModel> scores = _playerService.GetAllPlayersScore(_matchService);
                return Request.CreateResponse(HttpStatusCode.OK, scores);
            });
        }

        // GET api/players/5/score
        [Route("api/players/{id:int}/score")]
        [HttpGet]
        public async Task<HttpResponseMessage> Get(int id)
        {
            return await Task.Run(() =>
            {
                ScoreModel playerModel = _playerService.GetPlayerScore(id, _matchService);
                return Request.CreateResponse(HttpStatusCode.OK, playerModel);
            });
        }
    }
}