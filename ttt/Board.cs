namespace ttt;

public class Board
{
    private CellState[] _board;

    private GameState _lastState;

    public GameState LastState
    {
        get => _lastState;
        set
        {
            switch (_lastState)
            {
                case GameState.TurnO when value == GameState.TurnO:
                case GameState.TurnX when value == GameState.TurnX:
                case GameState.WinX:
                case GameState.WinO:
                case GameState.Tie:
                    throw new ArgumentException(
                        $"Cannot set game state to {value} when the current state is {_lastState}"
                    );
            }

            _lastState = value;
        }
    }

    public readonly int Size;

    public Board(int size)
    {
        _board = new CellState[size * size];
        _lastState = GameState.Initial;
        Size = size;
    }

    public CellState this[int i]
    {
        get => _board[i - 1];
        set => _board[i - 1] = value;
    }

    public GameState TakeTurn(int cellNumber)
    {
        if (!GameEnded())
        {
            if (_board[cellNumber] == CellState.Empty)
                SetCell(cellNumber);
            else
                LastState = GameState.Error;
        }

        return LastState;
    }

    private void SetCell(int cellNumber)
    {
        var (value, state) = LastState switch
        {
            GameState.Initial => (CellState.Cross, GameState.TurnX),
            GameState.TurnO => (CellState.Cross, GameState.TurnX),
            GameState.TurnX => (CellState.Nought, GameState.TurnO),
            _ => throw new InvalidGameStateException()
        };

        _board[cellNumber] = value;
        LastState = state;
    }

    private bool GameEnded() => LastState is GameState.Tie
        or GameState.WinX or GameState.WinO;

    private CellState[] Row(int rowNumber)
    {
        var start = Size * rowNumber;
        var end = 3 * (Size - rowNumber - 1);
        return _board[start..end];
    }

    private CellState[] Column(int colNumber)
    {
        return _board.Where((c, i) => i % colNumber == 0).ToArray();
    }

    private CellState[] Diagonal(bool main)
    {
        var result = new CellState[Size];
        for (int i = 0; i < Size; i++)
        {
            result[i] = main ? _board[i * Size + i] : _board[(i + 1) * Size - (i + 1)];
        }

        return result;
    }
}