using System.Collections.Generic;
using System.Linq;
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
            var httpStatusCode = HttpStatusCode.OK;
            List<ScoreModel> scores = _servicesProvider.MatchService.GetScoreForAllPlayers().ToList();
            if (!scores.Any())
            {
                httpStatusCode = HttpStatusCode.NotFound;
            }

            return Request.CreateResponse(httpStatusCode, scores);
        }

        // GET api/players/5/score
        [Route("api/players/{id:int}/score")]
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            var httpStatusCode = HttpStatusCode.OK;
            ScoreModel playerModel = _servicesProvider.MatchService.GetPlayerScore(id);
            if (playerModel == null)
            {
                httpStatusCode = HttpStatusCode.NotFound;
            }

            return Request.CreateResponse(httpStatusCode, playerModel);
        }
    }
}