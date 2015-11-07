using System.Linq;
using AutoMapper;
using System.Collections.Generic;
using TournamentWebApi.BLL.Interfaces;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.DAL.Entities;
using TournamentWebApi.DAL.Interfaces;

namespace TournamentWebApi.BLL.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlayerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public PlayerModel Get(int playerId)
        {
            Player player = _unitOfWork.PlayerRepository.GetById(playerId);
            var playerModel = Mapper.Map<PlayerModel>(player);
            return playerModel;
        }

        public IEnumerable<PlayerModel> GetAllPlayers()
        {
            IEnumerable<Player> players = _unitOfWork.PlayerRepository.GetAll();
            var playerModels = Mapper.Map<IEnumerable<PlayerModel>>(players);
            return playerModels;
        }

        public PlayerListModel GetPlayerListModel(IEnumerable<PlayerModel> playerModels)
        {
            var playerListModel = new PlayerListModel
            {
                Items = playerModels != null ? playerModels.ToList() : new List<PlayerModel>()
            };

            return playerListModel;
        }
    }
}