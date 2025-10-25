using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 movementDirection;
    private Vector2 moveTarget;

    [SerializeField] private float moveSpeed = 3f;
    private bool isPlayerMoving = false;
    private float playerSize;

    private GridManager grid;

    private void Start()
    {
        playerSize = GetComponent<SpriteRenderer>().bounds.size.x;
        grid = GridManager.Instance;
    }

    private void Update()
    {
        // If the player is not moving, continue to the next frame
        if (!isPlayerMoving) return;

        // Draw a ray from the player to the target position
        Debug.DrawRay(transform.position, (Vector3)moveTarget - transform.position, Color.cyan);
        // Move the player towards the target position
        transform.position = Vector2.MoveTowards(
            transform.position, moveTarget, moveSpeed * Time.deltaTime);

        // When close enough to the target, get a new target
        // This is to ensure the player keep moving while the button is pressed
        if (Vector2.Distance(transform.position, moveTarget) < 0.1f)
        {
            moveTarget = GetTarget(movementDirection);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Every time the button is pressed, this method will be called continuously
        // InputActionPhase.Started is called when the button is pressed,
        // So on the next method call, this if statement will be ignored
        if (context.phase == InputActionPhase.Started)
        {
            isPlayerMoving = true;
            return;
        }

        // Same as above, but for when the button is released
        if (context.phase == InputActionPhase.Canceled)
        {
            isPlayerMoving = false;
            return;
        }

        // The code below is executed only when the button is held down,
        // which means the two if statements above are ignored

        // Ignore diagonal movement
        Vector2 newDirection = context.ReadValue<Vector2>();
        if (ShouldIgnoreDir(newDirection)) return;

        movementDirection = context.ReadValue<Vector2>();
        moveTarget = GetTarget(movementDirection);
    }

    /// <summary>
    /// Determines whether the given direction vector represents diagonal movement and should be ignored.
    /// </summary>
    /// <param name="dir">The direction vector to evaluate.</param>
    /// <returns>A boolean value indicating whether the direction should be ignored.
    /// Returns true if the direction represents diagonal movement; otherwise, false.</returns>
    private bool ShouldIgnoreDir(Vector2 dir)
    {
        // Ignore diagonal movement -> (1, 1), (1, -1), (-1, 1), (-1, -1)
        if ((Mathf.Abs(dir.x) == 1 && Mathf.Abs(dir.y) == 0)) return false;
        if ((Mathf.Abs(dir.x) == 0 && Mathf.Abs(dir.y) == 1)) return false;
        return true;
    }

    /// <summary>
    /// Calculates the next target position based on the given direction vector and current position.
    /// </summary>
    /// <param name="vector2">The direction vector indicating the desired movement direction.</param>
    /// <returns>A Vector3 representing the next target position. Returns the current position if there is no valid movement possible.</returns>
    private Vector3 GetTarget(Vector2 vector2)
    {
        // Perform a raycast to see if there is a tile in the way
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position, movementDirection,
            Mathf.Infinity, LayerMask.GetMask("Tilemap"));

        // return transform.position means the player needs to stay in the same position (stop moving)
        // if there is no tile in the way, stop moving
        if (hit.collider == null)
        {
            return transform.position;
        }

        // If the player is far from the tilemap edge, get the neighbor position
        if (Vector2.Distance(transform.position, hit.point) > grid.GetGridSize())
        {
            return grid.GetNeighborPositionFromDirVector(transform.position, movementDirection);
        }

        // Otherwise, stay in the same position aka stop
        return transform.position;
    }
}