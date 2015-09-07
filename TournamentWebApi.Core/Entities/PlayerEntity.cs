using System;

namespace TournamentWebApi.Core.Entities
{
    public abstract class PlayerEntity : Entity
    {
        public virtual int PlayerId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual DateTime RegistrationTime { get; set; }
        public virtual int Rate { get; set; }
    }
}
