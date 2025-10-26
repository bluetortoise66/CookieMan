using System;
using UnityEngine;

/// <summary>
/// Handles rendering and grid space-world space conversions for a tilemap grid system.
/// </summary>
public class GridRenderer
{
    private Vector3 _origin;
    private GameGrid _grid;
    
    private float gridSize = 1.0f;
    public float GridSize => gridSize;

    public GridRenderer(Vector3 origin, GameGrid grid)
    {
        _origin = origin;
        _grid = grid;
    }

    /// <summary>
    /// Calculates the world position based on grid coordinates and the origin point.
    /// </summary>
    /// <param name="x">The x-coordinate of the grid position.</param>
    /// <param name="y">The y-coordinate of the grid position.</param>
    /// <returns>The corresponding world position as a Vector3.</returns>
    public Vector3 GetWorldPosition(GridCell cell)
    {
        return new Vector3(cell.X, cell.Y, 0) + _origin;
    }

    /// <summary>
    /// Retrieves the grid cell corresponding to a given world position, relative to the grid's origin.
    /// </summary>
    /// <param name="worldPosition">The position in world space for which the corresponding grid cell is to be determined.</param>
    /// <returns>The grid cell that corresponds to the provided world position.</returns>
    /// <exception cref="System.Exception">Thrown when the calculated grid cell is not valid according to the grid's boundaries.</exception>
    public GridCell GetCellFromWorldPosition(Vector3 worldPosition)
    {
        // Get local cell position from the world position 
        Vector3 originReverted = worldPosition - _origin;
        
        // Get the corresponding grid cell
        GridCell cellCandidate = new GridCell(Mathf.FloorToInt(originReverted.x), Mathf.FloorToInt(originReverted.y));
        
        // If the grid says it's a valid cell, return it
        if (!_grid.IsValidCell(cellCandidate)) throw new Exception("Invalid cell position");
        
        return cellCandidate;
    }

    /// <summary>
    /// Converts a direction vector into a corresponding grid direction.
    /// </summary>
    /// <param name="directionVector">The vector representing a direction.</param>
    /// <returns>The corresponding grid direction as a <see cref="Direction"/> enumeration value.</returns>
    public Direction GetDirectionFromVector(Vector2 directionVector)
    {
        if (directionVector == Vector2.up) return Direction.Up;
        if (directionVector == Vector2.down) return Direction.Down;
        if (directionVector == Vector2.left) return Direction.Left;
        if (directionVector == Vector2.right) return Direction.Right;
        return Direction.Invalid;
    }

    /// <summary>
    /// Calculates the center position of a grid cell in world space.
    /// </summary>
    /// <param name="cell">The grid cell for which the center position is to be calculated.</param>
    /// <returns>The center position of the grid cell as a Vector3.</returns>
    public Vector3 GetCellCenter(GridCell cell)
    {
        return GetWorldPosition(cell) + new Vector3(gridSize / 2, gridSize / 2, 0);
    }

    /// <summary>
    /// Converts a world position to grid coordinates relative to the grid's origin point.
    /// </summary>
    /// <param name="worldPosition">The position in world space to be converted.</param>
    /// <returns>The corresponding coordinates in grid space as a Vector3.</returns>
    public Vector3 GetPosInGridCoordinates(Vector3 worldPosition)
    {
        return worldPosition - _origin;
    }

    /// <summary>
    /// Determines whether a given position in the grid has reached the center of a cell
    /// in the specified movement direction.
    /// </summary>
    /// <param name="direction">The direction of movement as a 2D vector.</param>
    /// <param name="currentPosition">The current position in world space.</param>
    /// <returns>True if the current position has reached the cell center in the given direction; otherwise, false.</returns>
    public bool HasReachedCellCenterInDirection(Vector2 direction, Vector3 currentPosition)
    {
        // Convert the direction vector to a Direction enum value
        Direction directionInEnum = GetDirectionFromVector(direction);
        
        // Get the position in grid coordinates
        Vector3 posInGridCoordinates = GetPosInGridCoordinates(currentPosition);
        
        // Calculate the distance from the cell center to the current position
        float xDistanceFromCellStart = posInGridCoordinates.x - Mathf.Floor(posInGridCoordinates.x); 
        float yDistanceFromCellStart = posInGridCoordinates.y - Mathf.Floor(posInGridCoordinates.y);

        // Calculate the distance to the cell center in the given direction
        float distanceToCenter = gridSize / 2;

        // Compare the distance to the cell center with the distance from the current position
        // to the cell center in the given direction
        switch (directionInEnum)
        {
            case Direction.Up: return (yDistanceFromCellStart >= distanceToCenter);
            case Direction.Down: return (yDistanceFromCellStart <= distanceToCenter);
            case Direction.Left: return (xDistanceFromCellStart >= distanceToCenter);
            case Direction.Right: return (xDistanceFromCellStart <= distanceToCenter);
            default: return false;
        }
    }
}