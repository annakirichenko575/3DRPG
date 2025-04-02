using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyHitReaction : MonoBehaviour
    {
        [SerializeField] private Animator enemyAnimator;
        [SerializeField] private HealthPoints healthPoints;

        private bool isHitReactionFinished = false; 

        private void Awake()
        {
            if (healthPoints != null)
            {
                healthPoints.OnHit += PlayHitReaction;
            }
        }

        private void PlayHitReaction()
        {
            if (enemyAnimator != null)
            {
                enemyAnimator.SetTrigger("isAttacked");
                isHitReactionFinished = false; 
            }
        }

        public void OnHitReactionFinished()
        {
            isHitReactionFinished = true;
        }

        private void Update()
        {
            if (isHitReactionFinished)
            {
                enemyAnimator.SetBool("isAttacking", true);
            }
        }

        private void OnDestroy()
        {
            if (healthPoints != null)
            {
                healthPoints.OnHit -= PlayHitReaction;
            }
        }
    }
}

