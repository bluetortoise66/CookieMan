using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    private Animator animator;
    private PlayerMovement player;

    private void OnEnable()
    {
        player.OnDirectionChanged += HandleDirectionState;
    }

    private void OnDisable()
    {
        player.OnDirectionChanged -= HandleDirectionState;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerMovement>();
    }

    private void HandleDirectionState(Vector2 direction)
    {
        animator.SetInteger(MoveX, (int)direction.x);
        animator.SetInteger(MoveY, (int)direction.y);
    }
}