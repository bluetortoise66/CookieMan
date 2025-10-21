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
    /// Calculates the world position based on grid coordinates and the origin point.
    /// </summary>
    /// <param name="x">The x-coordinate of the grid position.</param>
    /// <param name="y">The y-coordinate of the grid position.</param>
    /// <returns>The corresponding world position as a Vector3.</returns>
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y, 0) + _origin;
    }
}