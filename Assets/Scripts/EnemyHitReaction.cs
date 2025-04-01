using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyHitReaction : MonoBehaviour
    {
        [SerializeField] private Animator enemyAnimator;
        [SerializeField] private HealthPoints healthPoints;

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

