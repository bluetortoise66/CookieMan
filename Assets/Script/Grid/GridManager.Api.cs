using UnityEngine;

public partial class GridManager
{
    public bool HasReachedCellCenterInDirection(Vector2 dir, Vector3 currentPosition)
    {
        return gridRenderer.HasReachedCellCenterInDirection(dir, currentPosition);
    }
}