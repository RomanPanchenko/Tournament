namespace TournamentWebApi.Core.Entities
{
    public abstract class RoleEntity : Entity
    {
        public virtual int RoleId { get; set; }
        public virtual string Name { get; set; }
    }
}
