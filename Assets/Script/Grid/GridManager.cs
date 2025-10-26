using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// GridManager is a singleton class that manages the grid system throughout the application.
/// This class ensures a centralized point for accessing and managing the grid.
/// </summary>
[ExecuteAlways]
public partial class GridManager : MonoBehaviour
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
        gridRenderer = new GridRenderer(tilemap.origin, grid);

        // Populate the grid with grid objects based on the tilemap
        foreach (GridObject gridObject in grid.GetGridObjects())
        {
            // Get the world position of the cell position in grid objects
            Vector3 objectWorldPosition = gridRenderer.GetWorldPosition(gridObject.GetCellPosition());

            // Get the tile at the world position of the grid object
            int xVal = Mathf.FloorToInt(objectWorldPosition.x);
            int yVal = Mathf.FloorToInt(objectWorldPosition.y);

            // Get the tile from the provided cell position
            TileBase tile = tilemap.GetTile(new Vector3Int(xVal, yVal, 0));

            // If the tile is null, continue to the next grid object
            if (tile == null) continue;

            // Get the type of the tile from the tile object
            Type tileType = tile.GetType();

            // Set the type of the grid object based on the tile type
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
            // Get the cell position of the grid object
            GridCell currentCell = gridObject.GetCellPosition();
            
            // Get the world position from the cell position
            Vector3 cellWorldPosition = gridRenderer.GetWorldPosition(gridObject.GetCellPosition());

            // Get the X and Y positions of the cell world position
            int xPos = (int)cellWorldPosition.x;
            int yPos = (int)cellWorldPosition.y;

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

            // Draw the text based on the type of the grid object
            switch (gridObject.Type)
            {
                case GridObjectType.Wall:
                    Handles.Label(cellCenter, "wall", wallTextStyle);
                    break;
                case GridObjectType.Path:
                    // Handles.Label(cellCenter, "path", emptyTextStyle);
                    Handles.Label(cellCenter, $"({currentCell.X}, {currentCell.Y})", emptyTextStyle);
                    break;
                case GridObjectType.Empty:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // The width and height of the tilemap on local space
        int tilemapWidth = grid.Width;
        int tilemapHeight = grid.Height;

        // Translate the lower right corner of the tilemap to world space
        Vector3 firstCellWorldSpace = gridRenderer.GetWorldPosition(new GridCell(0, 0));
        Vector3 lastCellWorldSpace = gridRenderer.GetWorldPosition(new GridCell(tilemapWidth, tilemapHeight));;

        // Translate the origin (top left corner) of the grid to world space
        float originWorldSpaceX = firstCellWorldSpace.x;
        float originWorldSpaceY = firstCellWorldSpace.y;

        // Get the width and height of the tilemap in world space
        float widthWorldSpace = lastCellWorldSpace.x;
        float heightWorldSpace = lastCellWorldSpace.y;

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

    /// <summary>
    /// Regenerates the grid by reinitializing it based on the current state of the tilemap.
    /// This method compresses the tilemap bounds, recreates the grid structure, and populates it
    /// with grid objects that are updated according to the tile types present on the tilemap.
    /// </summary>
    public void RegenerateGrid()
    {
        InitializeGrid();
    }

    /// <summary>
    /// Toggles the visibility state of the grid.
    /// This method inverts the current visibility status, effectively showing or hiding the grid based on its previous state.
    /// </summary>
    public void ToggleVisibility()
    {
        isGridVisible = !isGridVisible;
    }
}