/// <summary>
/// Represents an object within a grid, storing its position as a two-dimensional coordinate.
/// </summary>
public class GridObject
{
    private GridCell _cellPosition;
    public GridCell GetCellPosition() => _cellPosition;

    public GridObjectType Type { get; set; } = GridObjectType.Empty;

    public GridObject(GridCell cellPosition)
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

/// <summary>
/// Specifies the type of object that can exist within a grid cell.
/// </summary>  
public enum GridObjectType
{
    Empty,
    Wall,
    Path,
}