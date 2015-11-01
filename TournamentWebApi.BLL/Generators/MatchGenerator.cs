using System;
using System.Collections.Generic;
using System.Linq;
using TournamentWebApi.BLL.Generators.Interfaces;
using TournamentWebApi.BLL.Interfaces;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.Core.Constants;
using TournamentWebApi.Core.Enums;

namespace TournamentWebApi.BLL.Generators
{
    public class MatchGenerator : IMatchGenerator
    {
        private readonly IPlayersHelper _playerHelper;
        private readonly IMatchesHelper _matchHelper;

        public MatchGenerator(IMatchesHelper matchHelper, IPlayersHelper playerHelper)
        {
            _matchHelper = matchHelper;
            _playerHelper = playerHelper;
        }

        public IEnumerable<MatchModel> GetMatchesForNextRound(IEnumerable<PlayerModel> players, IEnumerable<MatchModel> previousMatches)
        {
            var matchesForNextRound = new List<MatchModel>();

            List<MatchModel> matchModels = previousMatches.ToList();
            List<PlayerModel> playerModels = players.ToList();

            int round = matchModels.Count == 0 ? 1 : matchModels.OrderByDescending(p => p.Round).First().Round + 1;

            players = _playerHelper.GetPlayersWithCalculatedRating(playerModels, matchModels);
            playerModels = players.OrderByDescending(p => p.Rate).ToList();

            while (playerModels.Count > 0)
            {
                PlayerModel player = playerModels.First();
                playerModels.Remove(player);

                MatchModel matchModel = _matchHelper.GetMatchModelForNextRound(player, playerModels, matchModels);
                matchModel.Round = round;
                matchModel.Player1PlaysWhite = _playerHelper.PlayerCanPlayWithDefinedColor(player, ChessColor.White, matchModels);
                matchModel.MatchStartTime = DateTime.Now;
                matchModel.Winner = new PlayerModel { PlayerId = SpecialPlayerIds.WinnerIdForGameWithUndefinedResult };

                playerModels.Remove(matchModel.Player2);
                matchesForNextRound.Add(matchModel);
            }

            return matchesForNextRound;
        }

        public IEnumerable<MatchModel> AssignRandomResultsForGeneratedMatches(IEnumerable<MatchModel> roundMatches)
        {
            List<MatchModel> matches = roundMatches.ToList();
            var random = new Random(DateTime.Now.Millisecond);
            foreach (MatchModel match in matches)
            {
                match.Winner = new PlayerModel { PlayerId = SpecialPlayerIds.WinnerIdForDrawnGame };
                int rnd = random.Next(0, 2);
                switch (rnd)
                {
                    case 1:
                        match.Player1.Winner = true;
                        match.Winner.PlayerId = match.Player1.PlayerId;
                        break;
                    case 2:
                        match.Player2.Winner = true;
                        match.Winner.PlayerId = match.Player2.PlayerId;
                        break;
                }
            }

            return matches;
        }
    }
}
