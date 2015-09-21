using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using TournamentWebApi.BLL.Interfaces;
using TournamentWebApi.BLL.Models;

namespace TournamentWebApi.WEB.ApiControllers
{
    public class MatchesController : ApiController
    {
        private readonly ITournamentServiceProvider _servicesProvider;

        public MatchesController(ITournamentServiceProvider servicesProvider)
        {
            _servicesProvider = servicesProvider;
        }

        // GET api/<controller>/5
        [Route("api/players/{id:int}/matches")]
        [HttpGet]
        public async Task<IEnumerable<MatchModel>> Get(int id)
        {
            return await Task.Run(() =>
            {
                IEnumerable<MatchModel> matches = _servicesProvider.MatchService.GetPlayerMatches(id);
                return matches;
            });
        }
    }
}