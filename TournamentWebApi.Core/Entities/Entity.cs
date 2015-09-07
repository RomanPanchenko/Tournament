using TournamentWebApi.Core.Validation;

namespace TournamentWebApi.Core.Entities
{
    public abstract class Entity
    {
        public virtual ValidationResult Result { get; set; }
    }
}
