using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using TournamentWebApi.BLL.Interfaces;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.Core.Constants;
using TournamentWebApi.DAL.Entities;
using TournamentWebApi.DAL.Interfaces;

namespace TournamentWebApi.BLL.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IGenericRepository<Player> _playerRepository;

        public PlayerService(IUnitOfWork unitOfWork)
        {
            _playerRepository = unitOfWork.PlayerRepository;
        }

        public PlayerModel Get(int playerId)
        {
            Player player = _playerRepository.Get(playerId);
            var playerModel = Mapper.Map<PlayerModel>(player);
            return playerModel;
        }

        public IEnumerable<PlayerModel> GetAllPlayers()
        {
            IEnumerable<Player> players = _playerRepository.GetRange(p => p.PlayerId > 0);
            var playerModels = Mapper.Map<IEnumerable<PlayerModel>>(players);
            return playerModels;
        }

        public ScoreModel GetPlayerScore(int playerId, IMatchService matchService)
        {
            IEnumerable<ScoreModel> scoreModels = GetAllPlayersScore(matchService);
            return scoreModels.First(p => p.PlayerId == playerId);
        }

        public IEnumerable<ScoreModel> GetAllPlayersScore(IMatchService matchService)
        {
            // Dictionary<PlayerId, ScoreValue>
            var playersScores = new Dictionary<int, int>();
            IEnumerable<Player> players = _playerRepository.GetRange(p => p.PlayerId > 0);
            List<ScoreModel> scores = Mapper.Map<IEnumerable<ScoreModel>>(players).ToList();
            List<MatchModel> matches = matchService
                .GetRange(p => p.Winner.PlayerId != SpecialPlayerIds.WinnerIdForGameWithUndefinedResult)
                .ToList();

            foreach (MatchModel matchModel in matches)
            {
                if (!playersScores.ContainsKey(matchModel.Player1.PlayerId))
                {
                    playersScores[matchModel.Player1.PlayerId] = 0;
                }

                if (!playersScores.ContainsKey(matchModel.Player2.PlayerId))
                {
                    playersScores[matchModel.Player2.PlayerId] = 0;
                }

                if (matchModel.Winner.PlayerId == matchModel.Player1.PlayerId)
                {
                    playersScores[matchModel.Player1.PlayerId] += Score.PointsForWin;
                }
                else if (matchModel.Winner.PlayerId == matchModel.Player2.PlayerId)
                {
                    playersScores[matchModel.Player2.PlayerId] += Score.PointsForWin;
                }
                else if (matchModel.Winner.PlayerId == SpecialPlayerIds.WinnerIdForDrawnGame)
                {
                    playersScores[matchModel.Player1.PlayerId] += Score.PointsForDrawn;
                    playersScores[matchModel.Player2.PlayerId] += Score.PointsForDrawn;
                }
            }

            foreach (ScoreModel scoreModel in scores)
            {
                scoreModel.Score = playersScores[scoreModel.PlayerId];
            }

            List<ScoreModel> scoreModels = scores.OrderByDescending(p => p.Score).ToList();
            int position = 0;
            int lastScore = -1;

            foreach (ScoreModel scoreModel in scoreModels)
            {
                if (scoreModel.Score != lastScore)
                {
                    lastScore = scoreModel.Score;
                    position++;
                }

                scoreModel.Position = position;
            }

            return scoreModels;
        }

        public IEnumerable<MatchModel> GetMatches(int playerId, IMatchService matchService)
        {
            IEnumerable<MatchModel> matches = matchService
                .GetRange(p => p.Player1.PlayerId == playerId || p.Player2.PlayerId == playerId);

            return matches;
        }
    }
}