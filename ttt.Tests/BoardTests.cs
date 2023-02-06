namespace ttt.Tests;

[TestFixture]
public class BoardTests
{
    private Board _board;

    [SetUp]
    public void Setup()
    {
        _board = new Board(3);
    }

    [Test]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    [TestCase(5)]
    [TestCase(6)]
    [TestCase(7)]
    [TestCase(8)]
    [TestCase(9)]
    public void Should_TakeFirstTurn(int cellNumber)
    {
        var gameState = _board.TakeTurn(cellNumber);
        Assert.That(_board[cellNumber], Is.EqualTo(CellState.Cross));
        Assert.That(_board.LastState, Is.EqualTo(GameState.TurnX));
        Assert.That(gameState, Is.EqualTo(_board.LastState));
    }

    [Test]
    public void Should_TakeSecondTurn()
    {
        _board.TakeTurn(1);

        _board.TakeTurn(9);

        Assert.That(_board[1], Is.EqualTo(CellState.Cross));
        Assert.That(_board[5], Is.EqualTo(CellState.Empty));
        Assert.That(_board[9], Is.EqualTo(CellState.Nought));

        Assert.That(_board.LastState, Is.EqualTo(GameState.TurnO));
    }

    [Test]
    public void Should_WinRowWithCross()
    {
        _board.TakeTurn(1);
        _board.TakeTurn(4);
        _board.TakeTurn(2);
        _board.TakeTurn(5);

        _board.TakeTurn(3);

        Assert.That(_board.GameEnded());
        Assert.That(_board.LastState, Is.EqualTo(GameState.WinX));
    }

    [Test]
    public void Should_WinColumnWithCross()
    {
        _board.TakeTurn(1);
        _board.TakeTurn(2);
        _board.TakeTurn(4);
        _board.TakeTurn(5);

        _board.TakeTurn(7);

        Assert.That(_board.GameEnded());
        Assert.That(_board.LastState, Is.EqualTo(GameState.WinX));
    }

    [Test]
    public void Should_WinMainDiagonalWithCross()
    {
        _board.TakeTurn(1);
        _board.TakeTurn(2);
        _board.TakeTurn(5);
        _board.TakeTurn(3);

        _board.TakeTurn(9);

        Assert.That(_board.GameEnded());
        Assert.That(_board.LastState, Is.EqualTo(GameState.WinX));
    }

    [Test]
    public void Should_WinCounterDiagonalWithCross()
    {
        _board.TakeTurn(3);
        _board.TakeTurn(2);
        _board.TakeTurn(5);
        _board.TakeTurn(1);

        _board.TakeTurn(7);

        Assert.That(_board.GameEnded());
        Assert.That(_board.LastState, Is.EqualTo(GameState.WinX));
    }

    [Test]
    public void Should_WinRowWithNought()
    {
        _board.TakeTurn(7);
        _board.TakeTurn(1);
        _board.TakeTurn(5);
        _board.TakeTurn(2);
        _board.TakeTurn(4);

        _board.TakeTurn(3);

        Assert.That(_board.GameEnded());
        Assert.That(_board.LastState, Is.EqualTo(GameState.WinO));
    }

    [Test]
    public void Should_WinColumnWithNought()
    {
        _board.TakeTurn(7);
        _board.TakeTurn(2);
        _board.TakeTurn(6);
        _board.TakeTurn(5);
        _board.TakeTurn(1);

        _board.TakeTurn(8);

        Assert.That(_board.GameEnded());
        Assert.That(_board.LastState, Is.EqualTo(GameState.WinO));
    }

    [Test]
    public void Should_WinMainDiagonalWithNought()
    {
        _board.TakeTurn(2);
        _board.TakeTurn(1);
        _board.TakeTurn(6);
        _board.TakeTurn(5);
        _board.TakeTurn(7);

        _board.TakeTurn(9);

        Assert.That(_board.GameEnded());
        Assert.That(_board.LastState, Is.EqualTo(GameState.WinO));
    }

    [Test]
    public void Should_WinCounterDiagonalWithNought()
    {
        _board.TakeTurn(2);
        _board.TakeTurn(3);
        _board.TakeTurn(6);
        _board.TakeTurn(5);
        _board.TakeTurn(9);

        _board.TakeTurn(7);

        Assert.That(_board.GameEnded());
        Assert.That(_board.LastState, Is.EqualTo(GameState.WinO));
    }

    [Test]
    public void Should_Tie()
    {
        _board.TakeTurn(1);
        _board.TakeTurn(5);
        _board.TakeTurn(9);
        _board.TakeTurn(8);
        _board.TakeTurn(2);
        _board.TakeTurn(3);
        _board.TakeTurn(7);
        _board.TakeTurn(4);
        _board.TakeTurn(6);

        Assert.That(_board.GameEnded());
        Assert.That(_board.LastState, Is.EqualTo(GameState.Tie));
    }

    [Test]
    [TestCase(0)]
    [TestCase(10)]
    public void Should_HandleOutOfBoundsInput(int cellNumber)
    {
        var gameState = _board.TakeTurn(cellNumber);
        Assert.That(!_board.GameEnded());
        Assert.That(gameState, Is.EqualTo(GameState.Error));
        Assert.That(_board.LastState, Is.EqualTo(GameState.Initial));
    }

    [Test]
    public void Should_HandleAlreadyTakenCell()
    {
        _board.TakeTurn(1);

        var gameState = _board.TakeTurn(1);

        Assert.That(!_board.GameEnded());
        Assert.That(gameState, Is.EqualTo(GameState.Error));
        Assert.That(_board.LastState, Is.EqualTo(GameState.TurnX));
    }

    [Test]
    public void Should_ThrowOnInvalidOperation()
    {
        _board.TakeTurn(2);
        _board.TakeTurn(1);
        _board.TakeTurn(6);
        _board.TakeTurn(5);
        _board.TakeTurn(7);
        _board.TakeTurn(9);

        Assert.That(_board.GameEnded());
        Assert.Throws<InvalidGameStateException>(
            () => _board.TakeTurn(3)
        );
    }
}