using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TournamentWebApi.BLL.Interfaces;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.WEB.Filters;

namespace TournamentWebApi.WEB.ApiControllers
{
    [ApiExceptionLoggingFilter]
    public class ScoresController : BaseController
    {
        private readonly ITournamentServiceProvider _servicesProvider;

        public ScoresController(ITournamentServiceProvider servicesProvider)
        {
            _servicesProvider = servicesProvider;
        }


        // GET api/scores
        [HttpGet]
        [Route("api/players/score")]
        public HttpResponseMessage Get()
        {
            IEnumerable<ScoreModel> scores = _servicesProvider.MatchService.GetScoreForAllPlayers();
            return Request.CreateResponse(HttpStatusCode.OK, scores);
        }

        // GET api/players/5/score
        [Route("api/players/{id:int}/score")]
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            ScoreModel playerModel = _servicesProvider.MatchService.GetPlayerScore(id);
            return Request.CreateResponse(HttpStatusCode.OK, playerModel);
        }
    }
}