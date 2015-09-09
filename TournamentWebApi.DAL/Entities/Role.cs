using System.Collections.Generic;
using TournamentWebApi.Core.Entities;

namespace TournamentWebApi.DAL.Entities
{
    public class Role : RoleEntity
    {
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
