using System.ComponentModel.DataAnnotations;

namespace HiLoGame.Api.Requests;

public class CreateGameResponse
{
    public Guid GameId { get; set; }
    public int MinNumber { get; set; }
    public int MaxNumber { get; set; }
}
