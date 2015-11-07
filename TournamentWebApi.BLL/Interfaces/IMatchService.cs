using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.DAL.Entities;

namespace TournamentWebApi.BLL.Interfaces
{
    public interface IMatchService
    {
        /// <summary>
        ///     Adds the specified match to the database.
        /// </summary>
        /// <param name="matchModel">The match model.</param>
        void Add(MatchModel matchModel);

        /// <summary>
        ///     Adds the specified matches to the database.
        /// </summary>
        /// <param name="matchModels">List of match models.</param>
        void AddRange(IEnumerable<MatchModel> matchModels);

        /// <summary>
        ///     Gets all matches from database
        /// </summary>
        /// <returns>MatchModel entities collection</returns>
        IEnumerable<MatchModel> GetAllMatches();

        /// <summary>
        ///     Gets specific match model by its Id
        /// </summary>
        /// <param name="matchId">MatchId</param>
        /// <returns>MatchModel entity</returns>
        MatchModel Get(int matchId);

        /// <summary>
        ///     Gets all matches filtered by filter condition
        /// </summary>
        /// <param name="filterCondition"></param>
        /// <returns>MatchModel entities collection filtered by filterCondition</returns>
        IEnumerable<MatchModel> GetRange(Expression<Func<Match, bool>> filterCondition);

        /// <summary>
        ///     Generates matches for next round
        /// </summary>
        /// <returns>MatchModel entities collection</returns>
        IEnumerable<MatchModel> GenerateMatchesForNextRound();

        /// <summary>
        ///     Generates matches for next round
        /// </summary>
        /// <param name="players">Players taking part in tournament</param>
        /// <param name="matches">List of previous matches in this tournament</param>
        /// <returns>MatchModel entities collection</returns>
        IEnumerable<MatchModel> GenerateMatchesForNextRound(IEnumerable<Player> players, IEnumerable<Match> matches);

        /// <summary>
        ///     Generates matches for next round
        /// </summary>
        /// <param name="players">Players taking part in tournament</param>
        /// <param name="matches">List of previous matches in this tournament</param>
        /// <returns>MatchModel entities collection</returns>
        IEnumerable<MatchModel> GenerateMatchesForNextRound(IList<PlayerModel> players, IList<MatchModel> matches);

        /// <summary>
        ///     Generates matches for all games
        /// </summary>
        /// <returns>MatchModel entities collection</returns>
        IEnumerable<MatchModel> GenerateMatches();

        /// <summary>
        ///     Generates matches for all games
        /// </summary>
        /// <param name="players">Players taking part in tournament</param>
        /// <param name="matches">List of previous matches in this tournament</param>
        /// <returns>MatchModel entities collection</returns>
        IEnumerable<MatchModel> GenerateMatches(IList<Player> players, IList<Match> matches);

        /// <summary>
        ///     Generates matches for all games
        /// </summary>
        /// <param name="players">Players taking part in tournament</param>
        /// <param name="matches">List of previous matches in this tournament</param>
        /// <returns>MatchModel entities collection</returns>
        IEnumerable<MatchModel> GenerateMatches(IList<PlayerModel> players, IList<MatchModel> matches);

        /// <summary>
        ///     Gets player score
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns>ScoreModel entity</returns>
        ScoreModel GetPlayerScore(int playerId);

        /// <summary>
        ///     Gets scores for all players
        /// </summary>
        /// <returns>ScoreModel entities collection for all players</returns>
        IEnumerable<ScoreModel> GetScoreForAllPlayers();

        /// <summary>
        ///     Gets all matches for specified player
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns>MatchModel entities collection for the given player id</returns>
        IEnumerable<MatchModel> GetPlayerMatches(int playerId);

        /// <summary>
        ///     Gets rounds count
        /// </summary>
        /// <param name="playersCount">Count of players taking part in the tournament</param>
        /// <returns></returns>
        int GetTotalRoundsCount(int playersCount);

        /// <summary>
        ///     Gets matches with assigned players result randomly assigned
        /// </summary>
        /// <param name="roundMatches">List of matches in the last round</param>
        /// <returns>MatchModel entities collection for the next tournament round. Players result are randomly assigned.</returns>
        IEnumerable<MatchModel> AssignRandomResultsForGeneratedMatches(IList<MatchModel> roundMatches);

        /// <summary>
        ///     Gets ListModel for Scores 
        /// </summary>
        /// <param name="scoreModels">List of Score models</param>
        /// <returns></returns>
        ScoreListModel GetScoreListModel(IEnumerable<ScoreModel> scoreModels);

        /// <summary>
        ///     Gets ListModel for Matches
        /// </summary>
        /// <param name="matchModels">List of Match models</param>
        /// <returns></returns>
        MatchListModel GetMatchListModel(IEnumerable<MatchModel> matchModels);
    }
}