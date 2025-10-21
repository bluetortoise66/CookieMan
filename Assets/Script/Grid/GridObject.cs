using UnityEngine;

/// <summary>
/// Represents an object within a grid, storing its position as a two-dimensional coordinate.
/// </summary>
public class GridObject
{
    /// <summary>
    /// Represents the position of the object within a grid as a two-dimensional integer coordinate.
    /// </summary>
    private Vector2Int _cellPosition;
    /// <summary>
    /// Retrieves the position of the object within the grid as a two-dimensional coordinate.
    /// </summary>
    /// <returns>The position of the object within the grid.</returns>
    public Vector2Int GetCellPosition() => _cellPosition;
    
    public GridObjectType Type { get; set; } = GridObjectType.Empty;
    
    /// <summary>
    /// Represents an object within a grid with a specific position.
    /// </summary>
    /// <param name="cellPosition">The position of the object within the grid.</param>
    public GridObject(Vector2Int cellPosition)
    {
        this._cellPosition = cellPosition;
    }

    /// <summary>
    /// Returns a string that represents the current grid object, including its position within the grid.
    /// </summary>
    /// <returns>A string representation of the grid object and its position.</returns>
    public override string ToString()
    {
        return "GridObject: " + _cellPosition;
    }
}

public enum GridObjectType
{
    Empty,
    Wall,
    Path,
}
