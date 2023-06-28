namespace HiLoGame.Domain.Entities;

public class GameEntity : EntityBase
{
    public int MinNumber { get; set; }
    public int MaxNumber { get; set; }
    public bool IsGameStarted { get; set; }
    public bool IsGameFinished { get; set; }
    public int MysteryNumber { get; set; }
    public virtual IList<PlayerEntity> Players { get; set; }
    public virtual IList<GuessEntity> Guesses { get; set; }

    public GameEntity()
    {
        Players = new List<PlayerEntity>();
        Guesses = new List<GuessEntity>();
    }
}
