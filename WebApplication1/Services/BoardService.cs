namespace WebApplication1.Services;

public class BoardService
{
    public const string HumanMark = "X";
    public const string BotMark = "O";

    private const string EmptyCell = "";
    private const int CellCount = 9;

    private readonly string[] _cells = new string[CellCount];

    public BoardService()
    {
        Array.Fill(_cells, EmptyCell);
    }

    public string[][] GetBoard()
    {
        return
        [
            [_cells[0], _cells[1], _cells[2]],
            [_cells[3], _cells[4], _cells[5]],
            [_cells[6], _cells[7], _cells[8]],
        ];
    }

    public bool TryPlayHumanMove(int cell)
    {
        if (_cells[cell] != EmptyCell)
        {
            return false;
        }

        _cells[cell] = HumanMark;
        return true;
    }

    public int? PlayBotMoveOnRandomEmptyCell()
    {
        var emptyCells = new List<int>();
        for (var cell = 0; cell < CellCount; cell++)
        {
            if (_cells[cell] == EmptyCell)
            {
                emptyCells.Add(cell);
            }
        }

        if (emptyCells.Count == 0)
        {
            return null;
        }

        var chosenCell = emptyCells[Random.Shared.Next(emptyCells.Count)];
        _cells[chosenCell] = BotMark;
        return chosenCell;
    }
}
