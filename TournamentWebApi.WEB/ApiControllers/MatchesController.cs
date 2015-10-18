using System.Collections.Generic;
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
        public IEnumerable<MatchModel> Get(int id)
        {
            IEnumerable<MatchModel> matches = _servicesProvider.MatchService.GetPlayerMatches(id);
            return matches;
        }
    }
}