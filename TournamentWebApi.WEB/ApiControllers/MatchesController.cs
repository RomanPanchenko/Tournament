using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using TournamentWebApi.BLL.Interfaces;
using TournamentWebApi.BLL.Models;

namespace TournamentWebApi.WEB.ApiControllers
{
    public class MatchesController : ApiController
    {
        private readonly IMatchService _matchService;
        private readonly IPlayerService _playerService;

        public MatchesController(IPlayerService playerService, IMatchService matchService)
        {
            _playerService = playerService;
            _matchService = matchService;
        }

        // GET api/<controller>/5
        [Route("api/players/{id:int}/matches")]
        [HttpGet]
        public async Task<IEnumerable<MatchModel>> Get(int id)
        {
            return await Task.Run(() =>
            {
                IEnumerable<MatchModel> matches = _playerService.GetMatches(id, _matchService);
                return matches;
            });
        }
    }
}