using UnityEngine;

public class MonsterAnimator : MonoBehaviour
{
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    private static readonly int IsDefault = Animator.StringToHash("IsDefault");
    private static readonly int IsFrightened = Animator.StringToHash("IsFrightened");
    private static readonly int IsEaten = Animator.StringToHash("IsEaten");
    
    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void HandleDirectionState(Vector2 direction)
    {
        animator.SetInteger(MoveX, (int)direction.x);
        animator.SetInteger(MoveY, (int)direction.y);
    }

    public void SetDefault(bool isDefault)
    {
        animator.SetBool(IsDefault, isDefault);
    }
    
    public void SetFrightened(bool isFrightened)
    {
        animator.SetBool(IsFrightened, isFrightened);
    }

    public void SetEaten(bool isEaten)
    {
        animator.SetBool(IsEaten, isEaten);
    }
}