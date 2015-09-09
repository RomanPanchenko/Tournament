using System.Collections.Generic;
using TournamentWebApi.Core.Entities;

namespace TournamentWebApi.DAL.Entities
{
    public class Account : AccountEntity
    {
        public virtual ICollection<Role> Roles { get; set; }
    }
}
