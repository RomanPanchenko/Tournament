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
        private readonly IPlayersHelper _playersHelper;
        private readonly IMatchesHelper _matchesHelper;

        public MatchGenerator(IMatchesHelper matchesHelper, IPlayersHelper playersHelper)
        {
            _matchesHelper = matchesHelper;
            _playersHelper = playersHelper;
        }

        public IEnumerable<MatchModel> GetMatchesForNextRound(IList<PlayerModel> players, IList<MatchModel> previousMatches)
        {
            var matchesForNextRound = new List<MatchModel>();

            int round = previousMatches.Count == 0 ? 1 : previousMatches.Max(p => p.Round) + 1;

            List<PlayerModel> playerModels = _playersHelper.GetPlayersWithCalculatedRating(players, previousMatches)
                                                    .OrderByDescending(p => p.Rate)
                                                    .ToList();

            while (playerModels.Count > 0)
            {
                PlayerModel player = playerModels.First();
                playerModels.Remove(player);

                MatchModel matchModel = _matchesHelper.GetMatchModelForNextRound(player, playerModels, previousMatches);
                matchModel.Round = round;
                matchModel.Player1PlaysWhite = _playersHelper.PlayerCanPlayWithDefinedColor(player, ChessColor.White, previousMatches);
                matchModel.MatchStartTime = DateTime.Now;
                matchModel.Winner = new PlayerModel { PlayerId = SpecialPlayerIds.WinnerIdForGameWithUndefinedResult };

                playerModels.Remove(matchModel.Player2);
                matchesForNextRound.Add(matchModel);
            }

            return matchesForNextRound;
        }

        public IEnumerable<MatchModel> AssignRandomResultsForGeneratedMatches(IList<MatchModel> roundMatches)
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
