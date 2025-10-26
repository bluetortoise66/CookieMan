using UnityEngine;

/// <summary>
/// The GridRenderer class is responsible for rendering and managing grid positions in world space.
/// It provides utilities to calculate world positions based on grid coordinates and an origin point.
/// </summary>
public class GridRenderer
{
    private Vector3 _origin;

    public GridRenderer(Vector3 origin)
    {
        _origin = origin;
    }

    /// <summary>
    /// Converts a grid cell to its corresponding world position in the scene.
    /// </summary>
    /// <param name="cell">The grid cell to be converted to world position.</param>
    /// <returns>The world position of the specified grid cell as a Vector3.</returns>
    public Vector3 GetWorldPosition(GridCell cell)
    {
        return new Vector3(cell.X, cell.Y, 0) + _origin;
    }
}