﻿namespace ttt;

public class Board
{
    private CellState[][] _board;

    private GameState _lastState;

    private int _cellsTaken;

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

    public readonly int MinCellNumber;

    public readonly int MaxCellNumber;

    public Board(int size)
    {
        Size = size;
        MinCellNumber = 1;
        MaxCellNumber = Size * Size;

        _board = new CellState[Size][];
        for (int i = 0; i < Size; i++)
            _board[i] = new CellState[Size];
        _cellsTaken = 0;
        _lastState = GameState.Initial;
    }

    public CellState this[int cellNumber]
    {
        get
        {
            var (row, column) = ToBoardIndices(cellNumber);
            return _board[row][column];
        }
        set
        {
            var (row, column) = ToBoardIndices(cellNumber);
            _board[row][column] = value;
        }
    }

    public GameState TakeTurn(int cellNumber)
    {
        if (GameEnded())
            throw new InvalidGameStateException();

        if (this[cellNumber] != CellState.Empty)
            return GameState.Error;

        return SetCell(cellNumber);
    }

    public bool GameEnded() => LastState is GameState.Tie
        or GameState.WinX or GameState.WinO;

    private GameState SetCell(int cellNumber)
    {
        var (value, state) = LastState switch
        {
            GameState.Initial => (CellState.Cross, GameState.TurnX),
            GameState.TurnO => (CellState.Cross, GameState.TurnX),
            GameState.TurnX => (CellState.Nought, GameState.TurnO),
            _ => throw new InvalidGameStateException()
        };

        this[cellNumber] = value;
        _cellsTaken += 1;
        LastState = state;
        return CheckGameEnded(cellNumber);
    }

    private GameState CheckGameEnded(int cellNumber)
    {
        var cellState = LastState switch
        {
            GameState.TurnO => CellState.Nought,
            GameState.TurnX => CellState.Cross,
            _ => throw new InvalidGameStateException()
        };

        if (HasWinningLine(cellNumber, cellState))
        {
            LastState = cellState == CellState.Cross ? GameState.WinX : GameState.WinO;
        }
        else if (_cellsTaken == Size * Size)
        {
            LastState = GameState.Tie;
        }

        return LastState;
    }

    private bool HasWinningLine(int cellNumber, CellState cellState)
    {
        var (rowNumber, colNumber) = ToBoardIndices(cellNumber);
        return IsWinningLine(Row(rowNumber), cellState)
               || IsWinningLine(Column(colNumber), cellState)
               || OnMainDiagonal(cellNumber) && IsWinningLine(MainDiagonal(), cellState)
               || OnCounterDiagonal(cellNumber) && IsWinningLine(CounterDiagonal(), cellState);
    }


    private CellState[] Row(int rowNumber) => _board[rowNumber];

    private CellState[] Column(int colNumber)
    {
        var column = new CellState[Size];
        for (int i = 0; i < Size; i++)
            column[i] = _board[i][colNumber];
        return column;
    }

    private CellState[] MainDiagonal()
    {
        var diagonal = new CellState[Size];
        for (int i = 0; i < Size; i++)
            diagonal[i] = _board[i][i];
        return diagonal;
    }

    private CellState[] CounterDiagonal()
    {
        var diagonal = new CellState[Size];
        for (int i = 0; i < Size; i++)
            diagonal[i] = _board[i][Size - 1 - i];
        return diagonal;
    }

    private bool IsWinningLine(CellState[] line, CellState cellState) =>
        line.All((cell) => cell == cellState);

    private (int, int) ToBoardIndices(int cellNumber) =>
        ((cellNumber - 1) / Size, (cellNumber - 1) % Size);

    private bool OnMainDiagonal(int cellNumber)
    {
        var (row, col) = ToBoardIndices(cellNumber);
        return row == col;
    }

    private bool OnCounterDiagonal(int cellNumber)
    {
        var (row, col) = ToBoardIndices(cellNumber);
        return row == Size - 1 - col;
    }
}