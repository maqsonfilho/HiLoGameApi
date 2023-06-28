namespace HiLoGame.Domain.Entities;

public class GuessEntity : EntityBase
{
    public GameEntity Game { get; set; }
    public Guid GameId { get; set; }
    public PlayerEntity Player { get; set; }
    public Guid PlayerId { get; set; }
    public int GuessNumber { get; set; }
    public string Feedback { get; set; }
}
