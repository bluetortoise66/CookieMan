using UnityEngine;

public partial class GridManager
{
    public float GetGridSize()
    {
        return gridRenderer.GridSize();
    }

    /// <summary>
    /// Retrieves the world position of the neighboring cell in a specific direction from the current position.
    /// </summary>
    /// <param name="currentPos">The current world position of the object.</param>
    /// <param name="direction">The direction vector indicating the desired neighboring cell's direction.</param>
    /// <returns>A Vector3 representing the world position of the neighboring cell's center.
    /// Throws an exception if the position or direction is invalid.</returns>
    public Vector3 GetNeighborPosition(Vector3 currentPos, Vector2 direction)
    {
        // Get the neighbor cell
        GridCell neighborCell = GetNeighborCell(currentPos, direction);

        // Return the position of the neighbor cell
        return gridRenderer.GetCellCenter(neighborCell);
    }

    /// <summary>
    /// Determines if the neighboring cell in the specified direction from the current position is walkable.
    /// </summary>
    /// <param name="currentPos">The current world position of the object.</param>
    /// <param name="direction">The direction vector indicating the desired neighboring cell's direction.</param>
    /// <returns>A boolean value indicating whether the neighboring cell is walkable.</returns>
    public bool IsNeighborCellWalkable(Vector3 currentPos, Vector2 direction)
    {
        // Get the neighbor cell
        GridCell neighborCell = GetNeighborCell(currentPos, direction);
        
        // Check if the neighbor cell is walkable (empty, not wall)
        GridObject gridObject = grid.GetGridObjects()[neighborCell.X, neighborCell.Y];
        
        // If the neighbor cell is walkable, return true
        return gridObject.Type == GridObjectType.Path;
    }

    private GridCell GetNeighborCell(Vector3 currentPos, Vector2 direction)
    {
        // Get the current Grid cell, based on the current player position
        GridCell currentCell = gridRenderer.GetCell(currentPos);
        
        // Convert the direction vector into a declared struct Direction
        Direction dir = gridRenderer.GetDirectionFromVector(direction);
        
        // Get the neighbor cell in the requested direction
        return grid.GetNeighborCell(currentCell, dir);
    }

    /// <summary>
    /// Determines if the current position of an object has reached the center of a grid cell
    /// in the specified direction.
    /// </summary>
    /// <param name="direction">The direction vector indicating the movement direction toward the cell center.</param>
    /// <param name="currentPosition">The current world position of the object on the grid.</param>
    /// <returns>True if the object has reached the center of the cell in the specified direction; otherwise, false.</returns>
    public bool HasReachedCellCenterInDirection(Vector2 direction, Vector3 currentPosition)
    {
        return gridRenderer.HasReachedCellCenterInDirection(direction, currentPosition);
    }

    /// <summary>
    /// Calculates and returns the center position of the current grid cell based on the provided world position.
    /// </summary>
    /// <param name="worldPos">The world position of the object within the grid.</param>
    /// <returns>A Vector3 representing the center position of the current grid cell.</returns>
    public Vector3 GetCellPosition(Vector3 worldPos)
    {
        GridCell cell = gridRenderer.GetCell(worldPos);
        return gridRenderer.GetCellCenter(cell);
    }
}