namespace ttt.Bot;

public class Bot
{
    private readonly CellState _ownCellState;
    private readonly CellState _opponentCellState;

    public Bot(Player player)
    {
        _ownCellState = player == Player.X ? CellState.Cross : CellState.Nought;
        _opponentCellState = player == Player.X ? CellState.Nought : CellState.Cross;
    }

    public void TakeTurn(Board board)
    {
        var gameTree = GameTree.BuildFrom(board);
        var bestBoard = Minimax(gameTree, true).CurrentBoard;
        var bestCellNumber = bestBoard.LastTakenCellNumber;
        board.TakeTurn(bestCellNumber);
    }

    private GameTree Minimax(GameTree root, bool maximize)
    {
        if (root.CurrentBoard.GameEnded())
            return root;
        return maximize ? Maximize(root) : Minimize(root);
    }

    private GameTree Minimize(GameTree root)
    {
        var bestUtility = 1000000;
        var bestNode = root;
        foreach (var node in root.NextBoards)
        {
            var minimized = Minimax(node, true);
            var utility = GetUtility(minimized.CurrentBoard);
            if (utility < bestUtility)
            {
                bestUtility = utility;
                bestNode = minimized;
            }
        }

        return bestNode;
    }

    private GameTree Maximize(GameTree root)
    {
        var bestUtility = -1000000;
        var bestNode = root;
        foreach (var node in root.NextBoards)
        {
            var maximized = Minimax(node, false);
            var utility = GetUtility(maximized.CurrentBoard);
            if (utility > bestUtility)
            {
                bestUtility = utility;
                bestNode = maximized;
            }
        }

        return bestNode;
    }

    private int GetUtility(Board board)
    {
        var utility = 0;
        for (int i = 0; i < board.Size; i++)
        {
            utility += GetLineUtility(board.Row(i));
            utility += GetLineUtility(board.Column(i));
        }

        utility += GetLineUtility(board.MainDiagonal());
        return utility + GetLineUtility(board.CounterDiagonal());
    }

    private int GetLineUtility(CellState[] line)
    {
        var maxCellCount = line.Length;
        var (ownCount, opponentCount) = (0, 0);
        foreach (var cell in line)
        {
            if (cell == _ownCellState)
                ownCount++;
            else if (cell == _opponentCellState)
                opponentCount++;
        }

        return CalculateUtility(maxCellCount, ownCount, opponentCount);
    }

    private static int CalculateUtility(int maxCount, int ownCount, int opponentCount)
    {
        if (opponentCount == 0)
            return ownCount == maxCount ? 1000000 : ownCount * 10;
        if (ownCount == 0)
            return opponentCount == maxCount ? -1000000 : opponentCount * -10;
        return 0;
    }
}