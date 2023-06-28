using System.ComponentModel.DataAnnotations;

namespace HiLoGame.Api.Requests;

public class CreateGameRequest
{
    [Required(ErrorMessage = "Minimum value is missing")]
    public int MinNumber { get; set; }

    [Required(ErrorMessage = "Maximum value is missing")]
    public int MaxNumber { get; set; }
}
