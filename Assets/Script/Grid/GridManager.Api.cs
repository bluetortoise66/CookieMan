using UnityEngine;

public partial class GridManager
{
    public float GetGridSize()
    {
        return gridRenderer.GridSize;
    }

    /// <summary>
    /// Retrieves the world position of the neighboring cell in a specific direction from the current position.
    /// </summary>
    /// <param name="currentPos">The current world position of the object.</param>
    /// <param name="direction">The direction vector indicating the desired neighboring cell's direction.</param>
    /// <returns>A Vector3 representing the world position of the neighboring cell's center. Throws an exception if the position or direction is invalid.</returns>
    public Vector3 GetNeighborPositionFromDirVector(Vector3 currentPos, Vector2 direction)
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
        GridObject gridObject = grid.GetGridObject(neighborCell);
        
        // If the neighbor cell is walkable, return true
        return gridObject.Type == GridObjectType.Empty;
    }

    private GridCell GetNeighborCell(Vector3 currentPos, Vector2 direction)
    {
        // Get the current Grid cell, based on the current player position
        GridCell currentCell = gridRenderer.GetCellFromWorldPosition(currentPos);
        
        // Convert the direction vector into a declared struct Direction
        Direction dir = gridRenderer.GetDirectionFromVector(direction);
        
        // Get the neighbor cell in the requested direction
        return grid.GetNeighborCell(currentCell, dir);
    }
}
