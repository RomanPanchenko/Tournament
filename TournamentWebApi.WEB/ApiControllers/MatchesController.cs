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
    public class MatchesController : BaseController
    {
        private readonly ITournamentServiceProvider _servicesProvider;

        public MatchesController(ITournamentServiceProvider servicesProvider)
        {
            _servicesProvider = servicesProvider;
        }

        // GET api/<controller>/5
        [Route("api/players/{id:int}/matches")]
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            var httpStatusCode = HttpStatusCode.OK;
            IEnumerable<MatchModel> matchesForSpecificPlayer = _servicesProvider.MatchService.GetPlayerMatches(id);
            MatchListModel matcheListModel = _servicesProvider.MatchService.GetMatchListModel(matchesForSpecificPlayer);
            if (!matcheListModel.Items.Any())
            {
                httpStatusCode = HttpStatusCode.NotFound;
            }

            return Request.CreateResponse(httpStatusCode, matcheListModel);
        }
    }
}