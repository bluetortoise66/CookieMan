using UnityEngine;

/// <summary>
/// Represents a two-dimensional game grid, managing its dimensions and the objects placed within.
/// </summary>
public class GameGrid
{
    private int _width;
    private int _height;
    /// <summary>
    /// A 2D array representing the objects placed in the grid.
    /// Stores instances of GridObject mapped to their grid positions.
    /// </summary>
    private GridObject[,] _gridObjects;

    public int Height => _height;
    public int Width => _width;
    /// <summary>
    /// Retrieves a two-dimensional array containing all grid objects placed within the grid.
    /// </summary>
    /// <returns>A 2D array of grid objects representing the current state of the grid.</returns>
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
                Vector2Int cellPosition = new Vector2Int(x, y);
                _gridObjects[x, y] = new GridObject(cellPosition);
            }
        }
    }
}