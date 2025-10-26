/// <summary>
/// Represents a two-dimensional game grid, managing its dimensions and the objects placed within.
/// </summary>
public class GameGrid
{
    private int _width;
    private int _height;
    public int Height => _height;
    public int Width => _width;

    private GridObject[,] _gridObjects;
    public GridObject[,] GetGridObjects() => _gridObjects;

    /// <summary>
    /// Represents a two-dimensional grid, managing its dimensions and the objects placed within it.
    /// </summary>
    /// <param name="width">The overall width of the grid.</param>
    /// <param name="height">The overall height of the grid.</param>
    public GameGrid(int width, int height)
    {
        this._width = width;
        this._height = height;

        _gridObjects = new GridObject[width, height];
        InitializeGrid();
    }

    /// <summary>
    /// Initializes the grid by populating each cell with a new instance of GridObject,
    /// associating it with its respective position within the grid.
    /// </summary>
    private void InitializeGrid()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                GridCell cellPosition = new GridCell(x, y);
                _gridObjects[x, y] = new GridObject(cellPosition);
            }
        }
    }
}


/// <summary>
/// Represents a single cell within a grid, identified by its X and Y coordinates.
/// </summary>
public readonly struct GridCell
{
    public int X { get; }
    public int Y { get; }

    /// <summary>
    /// Represents a single cell within a grid, identified by its X and Y coordinates.
    /// </summary>
    /// <param name="x">The X coordinate of the cell.</param>
    /// <param name="y">The Y coordinate of the cell.</param>
    public GridCell(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    Invalid
}