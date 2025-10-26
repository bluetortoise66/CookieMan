using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    
    private bool isPlayerMoving = false;
    private Vector2 moveTarget;
    private Vector2 currentInputDirection;
    private Vector2 previousInputDirection;

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

        if (!grid.HasReachedCellCenterInDirection(previousInputDirection, transform.position))
        {
            moveTarget = GetTarget(previousInputDirection);
        }
        else
        {
            moveTarget = GetTarget(currentInputDirection);
        }

        Move();

        // // When close enough to the target, get a new target
        // // This is to ensure the player keep moving while the button is pressed
        // if (Vector2.Distance(transform.position, moveTarget) < 0.1f)
        // {
        //     moveTarget = GetTarget(movementDirection);
        // }
    }

    /// <summary>
    /// Moves the player towards the target position based on the defined movement speed.
    /// This method ensures that the player's position is updated every frame towards the
    /// current movement target while drawing a visual ray towards that target for debugging purposes.
    /// </summary>
    private void Move()
    {
        // Draw a ray from the player to the target position
        Debug.DrawRay(transform.position, (Vector3)moveTarget - transform.position, Color.cyan);
        // Move the player towards the target position
        transform.position = Vector2.MoveTowards(
            transform.position, moveTarget, moveSpeed * Time.deltaTime);
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

        // Check player direction
        HandleDirectionSwitch(newDirection);
    }

    /// <summary>
    /// Updates the player's direction based on new input. This handles two cases:
    /// 1. For a 90-degree turn, it preserves the previous direction to ensure clean, grid-aligned movement.
    /// 2. For a 180-degree reversal, it forces an immediate switch for responsive controls.
    /// </summary>
    /// <param name="newDirection">The new movement direction input by the player as a Vector2.</param>
    private void HandleDirectionSwitch(Vector2 newDirection)
    {
        // First, assume this is a standard 90-degree turn.
        // We save the direction we were moving in so we can complete the move to the next cell center,
        // and set the new direction as our next intended move.
        previousInputDirection = currentInputDirection;
        currentInputDirection = newDirection;

        // This is a special correction for 180-degree reversals.
        // If the player is trying to move in the exact opposite direction,
        // we override the previous logic and force both the previous and current
        // directions to be the new one. This prevents a delay and makes the reversal feel instant.
        if (ShouldSwitchDirection())
        {
            previousInputDirection = newDirection;
            currentInputDirection = newDirection;
        }
    }

    /// <summary>
    /// Determines if the player has reversed their direction along the same axis (e.g., from Right to Left).
    /// It returns false for no change in direction or for a standard 90-degree turn.
    /// </summary>
    /// <returns>True only if the player has made a 180-degree direction reversal; otherwise, false.</returns> 
    private bool ShouldSwitchDirection()
    {
        // If the direction is exactly the same as it was last frame, we don't need to do any more complex math.
        // Just stop now and report that no switch occurred (return false).
        if (previousInputDirection == currentInputDirection) return false;

        // If the player switched from moving on the horizontal axis to the vertical axis (or vice-versa),
        // then this is a normal 90-degree turn, not a reversal. So, return false.
        if (Mathf.Abs(previousInputDirection.x) != Mathf.Abs(currentInputDirection.x) ||
            Mathf.Abs(previousInputDirection.y) != Mathf.Abs(currentInputDirection.y)) return false;

        return true;
    }

    /// <summary>
    /// Determines whether the given direction vector represents diagonal movement and should be ignored.
    /// </summary>
    /// <param name="dir">The direction vector to evaluate.</param>
    /// <returns>A boolean value indicating whether the direction should be ignored.
    /// Returns true if the direction represents a diagonal movement; otherwise, false.</returns>
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
    /// <param name="direction">The direction vector indicating the desired movement direction.</param>
    /// <returns>A Vector3 representing the next target position. Returns the current position if there is no valid movement possible.</returns>
    private Vector3 GetTarget(Vector2 direction)
    {
        // // Perform a raycast to see if there is a tile in the way
        // RaycastHit2D hit = Physics2D.Raycast(
        //     transform.position, movementDirection,
        //     Mathf.Infinity, LayerMask.GetMask("Tilemap"));
        //
        // // return transform.position means the player needs to stay in the same position (stop moving)
        // // if there is no tile in the way, stop moving
        // if (hit.collider == null)
        // {
        //     return transform.position;
        // }
        //
        // // If the player is far from the tilemap edge, get the neighbor position
        // if (Vector2.Distance(transform.position, hit.point) > grid.GetGridSize())
        // {
        //     return grid.GetNeighborPositionFromDirVector(transform.position, movementDirection);
        // }

        // Check the player direction to prevent invalid direction (0, 0)
        if (direction == Vector2.zero) return transform.position;

        // If the player is allowed to move in the provided direction, get the neighbor position
        if (grid.IsNeighborCellWalkable(transform.position, direction))
        {
            return grid.GetNeighborPosition(transform.position, direction);
        }

        // Otherwise, return the current cell position
        return grid.GetCellPosition(transform.position);
    }
}
