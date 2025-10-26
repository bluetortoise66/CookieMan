using System;

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

    /// <summary>
    /// Determines whether a given grid cell is valid within the bounds of the game grid.
    /// </summary>
    /// <param name="cellToCheck">The grid cell to validate.</param>
    /// <returns>True if the cell is within the grid bounds; otherwise, false.</returns>
    public bool IsValidCell(GridCell cellToCheck)
    {
        // Check if the cell is within the grid bounds
        if (cellToCheck.X > _width || cellToCheck.Y > _height) return false;
        if (cellToCheck.X < 0 || cellToCheck.Y < 0) return false;

        // If the cell is within the grid bounds, it is valid
        return true;
    }

    /// <summary>
    /// Retrieves the grid cell adjacent to the specified cell in the given direction, ensuring the neighbor cell is within grid bounds.
    /// </summary>
    /// <param name="currentCell">The current cell from which to locate the neighbor.</param>
    /// <param name="direction">The direction in which to search for the neighbor cell.</param>
    /// <returns>The neighboring grid cell in the specified direction.</returns>
    /// <exception cref="Exception">Thrown when the specified direction is invalid or the neighbor cell is outside the grid bounds.</exception>
    public GridCell GetNeighborCell(GridCell currentCell, Direction direction)
    {
        // Check if the specified direction is valid
        GridCell neighborCell;
        switch (direction)
        {
            case Direction.Up:
                neighborCell = new GridCell(currentCell.X, currentCell.Y + 1);
                break;
            case Direction.Down:
                neighborCell = new GridCell(currentCell.X, currentCell.Y - 1);
                break;
            case Direction.Left:
                neighborCell = new GridCell(currentCell.X - 1, currentCell.Y);
                break;
            case Direction.Right:
                neighborCell = new GridCell(currentCell.X + 1, currentCell.Y);
                break;
            default:
                throw new Exception("Invalid direction specified.");
        }

        // If not, throw an exception
        if (!IsValidCell(neighborCell)) throw new Exception("Neighbor Cell is outside of the grid");

        return neighborCell;
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