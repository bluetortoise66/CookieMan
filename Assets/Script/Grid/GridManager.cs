using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// GridManager is a singleton class that manages the grid system throughout the application.
/// This class ensures a centralized point for accessing and managing the grid.
/// </summary>
[ExecuteAlways]
public class GridManager : MonoBehaviour
{
    private static GridManager _instance;
    public static GridManager Instance => _instance;

    [SerializeField] private Tilemap tilemap;

    private GameGrid grid;
    private GridRenderer gridRenderer;
    private bool isGridVisible;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;

        InitializeGrid();
    }

    // private void OnValidate()
    // {
    //     if (!Application.isPlaying)
    //     {
    //         // Regenerate the grid every time the width or height is changed on the inspector
    //         InitializeGrid();
    //     }
    // }

    private void InitializeGrid()
    {
        if (tilemap == null) return;

        // Compress the bounds of the tilemap when the grid is initialized
        // We need this because when the tilemap is erased, the bounds are not compressed
        tilemap.CompressBounds();

        grid = new GameGrid(tilemap.size.x, tilemap.size.y);
        gridRenderer = new GridRenderer(tilemap.origin);

        foreach (GridObject gridObject in grid.GetGridObjects())
        {
            int xPos = gridObject.GetCellPosition().x;
            int yPos = gridObject.GetCellPosition().y;

            Vector3 objectWorldPosition = gridRenderer.GetWorldPosition(xPos, yPos);

            int xVal = Mathf.FloorToInt(objectWorldPosition.x);
            int yVal = Mathf.FloorToInt(objectWorldPosition.y);

            TileBase tile = tilemap.GetTile(new Vector3Int(xVal, yVal, 0));

            if (tile == null) continue;

            Type tileType = tile.GetType();

            if (tileType == typeof(Wall))
            {
                gridObject.Type = GridObjectType.Wall;
            }

            if (tileType == typeof(Path))
            {
                gridObject.Type = GridObjectType.Path;
            }
        }
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (grid == null || tilemap == null || !isGridVisible) return;

        Gizmos.color = Color.cyan;

        GridObject[,] gridObjects = grid.GetGridObjects();

        foreach (GridObject gridObject in gridObjects)
        {
            // Get the translated grid position
            int cellPosX = gridObject.GetCellPosition().x;
            int cellPosY = gridObject.GetCellPosition().y;
            int xPos = (int)gridRenderer.GetWorldPosition(cellPosX, cellPosY).x;
            int yPos = (int)gridRenderer.GetWorldPosition(cellPosX, cellPosY).y;

            // Draw a vertical line at the grid position, only on the left side
            Vector3 startVertical = new Vector3(xPos, yPos);
            Vector3 endVertical = new Vector3(xPos, yPos + 1);
            Gizmos.DrawLine(startVertical, endVertical);

            // Draw a horizontal line at the grid position, only on the top side
            Vector3 startHorizontal = new Vector3(xPos, yPos);
            Vector3 endHorizontal = new Vector3(xPos + 1, yPos);
            Gizmos.DrawLine(startHorizontal, endHorizontal);

            // Configure the text style for an empty grid object type
            GUIStyle emptyTextStyle = new GUIStyle();
            emptyTextStyle.normal.textColor = Color.white;
            emptyTextStyle.alignment = TextAnchor.MiddleCenter;
            
            // Configure the text style for a wall grid object type
            GUIStyle wallTextStyle = new GUIStyle();
            wallTextStyle.normal.textColor = Color.yellow;
            wallTextStyle.alignment = TextAnchor.MiddleCenter;

            // Draw the text at the center of the cell
            Vector3 cellCenter = new Vector3(xPos + 0.5f, yPos + 0.5f);

            switch (gridObject.Type)
            {
                case GridObjectType.Wall:
                    Handles.Label(cellCenter, "wall", wallTextStyle);
                    break;
                // case GridObjectType.Path:
                //     Handles.Label(cellCenter, "path", emptyTextStyle);
                //     break;
                default:
                    Handles.Label(cellCenter, $"({cellPosX}, {cellPosY})", emptyTextStyle);
                    break;
            }
        }

        // The width and height of the tilemap on local space
        int tilemapWidth = grid.Width;
        int tilemapHeight = grid.Height;

        // Translate the lower right corner of the tilemap to world space
        Vector3 tileMapEndWorldSpace = gridRenderer.GetWorldPosition(tilemapWidth, tilemapHeight);

        // Translate the origin (top left corner) of the grid to world space
        float originWorldSpaceX = gridRenderer.GetWorldPosition(0, 0).x;
        float originWorldSpaceY = gridRenderer.GetWorldPosition(0, 0).y;

        // Get the width and height of the tilemap in world space
        float widthWorldSpace = tileMapEndWorldSpace.x;
        float heightWorldSpace = tileMapEndWorldSpace.y;

        // Because every cell only draws a line on the left and top side,
        // we need to draw a line on the right and bottom side of the grid
        Vector3 finalStartVertical = new Vector3(widthWorldSpace, originWorldSpaceY);
        Vector3 finalEndVertical = new Vector3(widthWorldSpace, heightWorldSpace);
        Gizmos.DrawLine(finalStartVertical, finalEndVertical);

        Vector3 finalStartHorizontal = new Vector3(originWorldSpaceX, heightWorldSpace);
        Vector3 finalEndHorizontal = new Vector3(widthWorldSpace, heightWorldSpace);
        Gizmos.DrawLine(finalStartHorizontal, finalEndHorizontal);
    }

#endif

    public void RegenerateGrid()
    {
        InitializeGrid();
    }

    public void ToggleVisibility()
    {
        isGridVisible = !isGridVisible;
    }
}