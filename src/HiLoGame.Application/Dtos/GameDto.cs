namespace HiLoGame.Application.Dtos;

public class GameDto : DtoBase 
{
    public int MinNumber { get; set; }
    public int MaxNumber { get; set; }
    public bool IsGameStarted { get; set; }
    public bool IsGameFinished { get; set; }
    public int MysteryNumber { get; set; }
    public List<PlayerDto> Players { get; set; }
    public List<GuessDto> Guesses { get; set; }

    public GameDto()
    {
        Players = new List<PlayerDto>();
        Guesses = new List<GuessDto>();
    }
}
