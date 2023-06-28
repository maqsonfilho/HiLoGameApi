namespace HiLoGame.Domain.Entities;

public class PlayerEntity : EntityBase
{
    public string Name { get; set; }
    public Guid GameId { get; set; }
    public GameEntity Game { get; set; }
    public virtual IList<GuessEntity> Guesses { get; set; }
}
