using System;
using UnityEngine;

/// <summary>
/// The GridRenderer class is responsible for rendering and managing grid positions in world space.
/// It provides utilities to calculate world positions based on grid coordinates and an origin point.
/// </summary>
public class GridRenderer
{
    private Vector3 _origin;
    private GameGrid _grid;
    /// <summary>
    /// Responsible for rendering and managing grid positions in a game world.
    /// Provides functionality for converting between grid coordinates and world positions
    /// as well as determining grid-related attributes such as cell adjacency and movement directions.
    /// </summary>
    /// <param name="origin">The origin point for the grid in world space.</param>
    /// <param name="grid">The grid to be rendered.</param>
    public GridRenderer(Vector3 origin, GameGrid grid)
    {
        _origin = origin;
        _grid = grid;
    }
    
    private float gridSize = 1.0f;
    public float GridSize() => gridSize;

    /// <summary>
    /// Converts a grid cell to its corresponding world position in the scene.
    /// </summary>
    /// <param name="cell">The grid cell to be converted to world position.</param>
    /// <returns>The world position of the specified grid cell as a Vector3.</returns>
    public Vector3 GetWorldPosition(GridCell cell)
    {
        return new Vector3(cell.X, cell.Y, 0) + _origin;
    }

    /// <summary>
    /// Calculates the center position of a specific grid cell in world space.
    /// The center is determined based on the world position of the cell and the grid size.
    /// </summary>
    /// <param name="cell">The grid cell for which the center position is to be calculated.</param>
    /// <returns>The world-space position representing the center of the specified grid cell as a Vector3.</returns>
    public Vector3 GetCellCenter(GridCell cell)
    {
        // Get the center of the cell in world space
        return GetWorldPosition(cell) + new Vector3(gridSize / 2, gridSize / 2, 0);
    }

    /// <summary>
    /// Retrieves the grid cell corresponding to a given world position, relative to the grid's origin.
    /// </summary>
    /// <param name="worldPosition">The position in world space for which the corresponding grid cell is to be determined.</param>
    /// <returns>The grid cell that corresponds to the provided world position.</returns>
    /// <exception cref="System.Exception">Thrown when the calculated grid cell is not valid according to the grid's boundaries.</exception>
    public GridCell GetCell(Vector3 worldPosition)
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
    /// Calculates the grid coordinates from a given world position relative to the grid origin.
    /// </summary>
    /// <param name="worldPosition">The world position for which grid coordinates are to be calculated.</param>
    /// <returns>The grid coordinates as a Vector3 relative to the grid origin.</returns>
    private Vector3 GetPosInGridCoordinates(Vector3 worldPosition)
    {
        return worldPosition - _origin;
    }

    /// <summary>
    /// Determines the grid direction based on the provided 2D directional vector.
    /// </summary>
    /// <param name="directionVector">A Vector2 representing the direction to be evaluated.</param>
    /// <returns>A Direction enum value corresponding to the provided direction vector.</returns>
    public Direction GetDirectionFromVector(Vector2 directionVector)
    {
        if (directionVector == Vector2.up) return Direction.Up;
        if (directionVector == Vector2.down) return Direction.Down;
        if (directionVector == Vector2.left) return Direction.Left;
        if (directionVector == Vector2.right) return Direction.Right;
        return Direction.Invalid;
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