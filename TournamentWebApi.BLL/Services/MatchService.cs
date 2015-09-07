using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TournamentWebApi.BLL.Interfaces;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.DAL.Entities;
using TournamentWebApi.DAL.Interfaces;

namespace TournamentWebApi.BLL.Services
{
    public class MatchService : IMatchService
    {
        private readonly IGenericRepository<Match> _matchRepository;

        public MatchService(IUnitOfWork unitOfWork)
        {
            _matchRepository = unitOfWork.MatchRepository;
        }

        public void Add(MatchModel matchModel)
        {
            var match = Mapper.Map<Match>(matchModel);
            _matchRepository.Add(match);
        }

        public void AddRange(IEnumerable<MatchModel> matchModels)
        {
            var matches = Mapper.Map<IEnumerable<Match>>(matchModels);
            _matchRepository.AddRange(matches);
        }

        public IEnumerable<MatchModel> GetAllMatches()
        {
            IEnumerable<Match> matches = _matchRepository.GetAll();
            var matchModels = Mapper.Map<IEnumerable<MatchModel>>(matches);
            return matchModels;
        }

        public IEnumerable<MatchModel> GetRange(Expression<Func<Match, bool>> filterCondition)
        {
            IEnumerable<Match> matches = _matchRepository.GetRange(filterCondition);
            var matchModels = Mapper.Map<IEnumerable<MatchModel>>(matches);
            return matchModels;
        }

        public MatchModel Get(int matchId)
        {
            Match match = _matchRepository.Get(matchId);
            var matchModel = Mapper.Map<MatchModel>(match);
            return matchModel;
        }
    }
}