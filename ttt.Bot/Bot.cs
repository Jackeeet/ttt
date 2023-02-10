namespace ttt.Bot;

public class Bot
{
    private Board _board;

    public Board Board
    {
        get => _board;
        set => _board.Clone();
    }

    public Player Player { get; set; }

    public void TakeTurn()
    {
        throw new NotImplementedException();
    }

    private double GetUtility(Board board)
    {
        throw new NotImplementedException();
    }
}