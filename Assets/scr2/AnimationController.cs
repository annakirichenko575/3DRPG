using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateAnimations(bool isPatrol, bool isChasing, bool isAttacking)
    {
        animator.SetBool("isPatroling", isPatrol);
        animator.SetBool("isChasing", isChasing);
        animator.SetBool("isAttacking", isAttacking);
    }
}

