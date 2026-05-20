namespace WebApplication1.DTOs;

public class MovePlayedResponse
{
    public int Cell { get; set; }
    public string Mark { get; set; } = string.Empty;
    public string[][] Board { get; set; } = [];
}
