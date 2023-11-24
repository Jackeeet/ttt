namespace ttt.Bot;

public class GameTree
{
    public readonly Board CurrentBoard;
    public readonly List<GameTree> NextBoards;

    private GameTree(Board board)
    {
        CurrentBoard = board;
        NextBoards = new List<GameTree>();
    }

    public static GameTree BuildFrom(Board board)
    {
        var root = new GameTree(board);
        if (!board.GameEnded())
        {
            for (int i = 0; i < board.Size * board.Size; i++)
            {
                var nextBoard = board.Clone();
                var gameState = nextBoard.TakeTurn(i + 1);
                if (gameState != GameState.Error)
                {
                    root.NextBoards.Add(BuildFrom(nextBoard));
                }
            }
        }

        return root;
    }
}