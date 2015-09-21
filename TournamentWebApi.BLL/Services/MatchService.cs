using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TournamentWebApi.BLL.Interfaces;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.Core.Constants;
using TournamentWebApi.DAL.Entities;
using TournamentWebApi.DAL.Interfaces;

namespace TournamentWebApi.BLL.Services
{
    public class MatchService : IMatchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMatchGenerator _matchGenerator;

        public MatchService(IUnitOfWork unitOfWork, IMatchGenerator matchGenerator)
        {
            _unitOfWork = unitOfWork;
            _matchGenerator = matchGenerator;
        }

        public void Add(MatchModel matchModel)
        {
            var match = Mapper.Map<Match>(matchModel);
            _unitOfWork.MatchRepository.Add(match);
        }

        public void AddRange(IEnumerable<MatchModel> matchModels)
        {
            var matches = Mapper.Map<IEnumerable<Match>>(matchModels);
            _unitOfWork.MatchRepository.AddRange(matches);
        }

        public IEnumerable<MatchModel> GetAllMatches()
        {
            IEnumerable<Match> matches = _unitOfWork.MatchRepository.GetAll();
            var matchModels = Mapper.Map<IEnumerable<MatchModel>>(matches);
            return matchModels;
        }

        public IEnumerable<MatchModel> GetRange(Expression<Func<Match, bool>> filterCondition)
        {
            IEnumerable<Match> matches = _unitOfWork.MatchRepository.GetRange(filterCondition);
            var matchModels = Mapper.Map<IEnumerable<MatchModel>>(matches);
            return matchModels;
        }

        public MatchModel Get(int matchId)
        {
            Match match = _unitOfWork.MatchRepository.Get(matchId);
            var matchModel = Mapper.Map<MatchModel>(match);
            return matchModel;
        }

        public IEnumerable<MatchModel> GenerateMatchesForNextRound()
        {
            IEnumerable<Player> players = _unitOfWork.PlayerRepository.GetAll();
            var existingPlayers = Mapper.Map<IEnumerable<PlayerModel>>(players);

            IEnumerable<Match> matches = _unitOfWork.MatchRepository.GetAll();
            var previousMatches = Mapper.Map<IEnumerable<MatchModel>>(matches);

            return GenerateMatchesForNextRound(existingPlayers, previousMatches);
        }

        public IEnumerable<MatchModel> GenerateMatchesForNextRound(IEnumerable<Player> players, IEnumerable<Match> matches)
        {
            var existingPlayers = Mapper.Map<IEnumerable<PlayerModel>>(players);
            var previousMatches = Mapper.Map<IEnumerable<MatchModel>>(matches);

            return GenerateMatchesForNextRound(existingPlayers, previousMatches);
        }

        public IEnumerable<MatchModel> GenerateMatchesForNextRound(IEnumerable<PlayerModel> players, IEnumerable<MatchModel> matches)
        {
            return _matchGenerator.GetMatchesForNextRound(players, matches);
        }

        public IEnumerable<MatchModel> GenerateMatches()
        {
            IEnumerable<Player> players = _unitOfWork.PlayerRepository.GetAll();
            var existingPlayers = Mapper.Map<IEnumerable<PlayerModel>>(players);

            IEnumerable<Match> matches = _unitOfWork.MatchRepository.GetAll();
            var pastMatches = Mapper.Map<IEnumerable<MatchModel>>(matches);

            IEnumerable<MatchModel> matchesForNextRound = _matchGenerator.GetMatchesForNextRound(existingPlayers, pastMatches);

            return matchesForNextRound;
        }

        public IEnumerable<MatchModel> GenerateMatches(IEnumerable<Player> players, IEnumerable<Match> matches)
        {
            var existingPlayers = Mapper.Map<IEnumerable<PlayerModel>>(players);
            var pastMatches = Mapper.Map<IEnumerable<MatchModel>>(matches);

            IEnumerable<MatchModel> matchesForNextRound = _matchGenerator.GetMatchesForNextRound(existingPlayers, pastMatches);

            return matchesForNextRound;
        }

        public IEnumerable<MatchModel> GenerateMatches(IEnumerable<PlayerModel> players, IEnumerable<MatchModel> matches)
        {
            List<MatchModel> matchesForNextRound = matches.ToList();
            List<PlayerModel> playerModels = players.ToList();

            for (int i = 0; i < GetTotalRoundsCount(playerModels.Count); i++)
            {
                matchesForNextRound.AddRange(_matchGenerator.GetMatchesForNextRound(playerModels, matchesForNextRound));
            }

            return matchesForNextRound;
        }

        public ScoreModel GetPlayerScore(int playerId)
        {
            IEnumerable<ScoreModel> scoreModels = GetScoreForAllPlayers();
            return scoreModels.First(p => p.PlayerId == playerId);
        }

        public IEnumerable<ScoreModel> GetScoreForAllPlayers()
        {
            IEnumerable<Player> players = _unitOfWork.PlayerRepository.GetAll();
            List<ScoreModel> scores = Mapper.Map<IEnumerable<ScoreModel>>(players).ToList();
            var matchEntities = _unitOfWork.MatchRepository.GetRange(p => p.Winner.PlayerId != SpecialPlayerIds.WinnerIdForGameWithUndefinedResult);
            var matches = Mapper.Map<IEnumerable<MatchModel>>(matchEntities);

            CalculatePlayerScores(scores, players, matches);
            CalculatePlayerPositions(scores);

            return scores.OrderBy(p => p.Position);
        }

        public IEnumerable<MatchModel> GetPlayerMatches(int playerId)
        {
            IEnumerable<Match> matchEntities = _unitOfWork.MatchRepository.GetRange(p => p.Player1.PlayerId == playerId || p.Player2.PlayerId == playerId);
            var matches = Mapper.Map<IEnumerable<MatchModel>>(matchEntities);

            return matches;
        }

        public int GetTotalRoundsCount(int playersCount)
        {
            var roundsCount = (int)Math.Floor(Math.Log(playersCount, 2));
            return roundsCount;
        }

        public IEnumerable<MatchModel> AssignRandomResultsForGeneratedMatches(IEnumerable<MatchModel> roundMatches)
        {
            return _matchGenerator.AssignRandomResultsForGeneratedMatches(roundMatches);
        }

        private static void CalculatePlayerScores(IEnumerable<ScoreModel> scores, IEnumerable<Player> players, IEnumerable<MatchModel> matches)
        {
            // Dictionary<PlayerId, ScoreValue>
            var playersScores = new Dictionary<int, int>();

            foreach (Player player in players)
            {
                playersScores[player.PlayerId] = 0;
            }

            foreach (MatchModel matchModel in matches)
            {
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
        }

        private static void CalculatePlayerPositions(IEnumerable<ScoreModel> scores)
        {
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
        }
    }
}