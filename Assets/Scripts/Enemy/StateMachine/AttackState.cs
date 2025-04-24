using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace Enemy.StateMachine
{
    public class AttackState : IEnemyState
    {
        private WolfStateMachine enemyBrain;
        private Animator animator;
        private NavMeshAgent navMeshAgent;
        private Transform player;

        // Перенесенные поля из EnemyAttack
        private int damage = 20;
        private float attackInterval = 5f;
        private Player.HealthPoints playerHealth;
        private Coroutine attackCoroutine;

        public AttackState(WolfStateMachine enemyBrain, Animator animator,
                         NavMeshAgent navMeshAgent)
        {
            this.enemyBrain = enemyBrain;
            this.animator = animator;
            this.navMeshAgent = navMeshAgent;
            this.player = enemyBrain.Player;
        }

        public void Enter()
        {
            enemyBrain.Stop();
            animator.SetBool("isAttacking", true);
            playerHealth = player.GetComponent<Player.HealthPoints>(); 
            if (attackCoroutine == null && playerHealth != null)
            {
                attackCoroutine = enemyBrain.StartCoroutine(PeriodicAttack());
            }
            
        }

        public void Update()
        {
            if (enemyBrain.PlayerInAttackDistance() == false)
            {
                enemyBrain.ChangeState(WolfStates.Chase);
            }
        }

        public void Exit()
        {
            animator.SetBool("isAttacking", false);
            if (attackCoroutine != null)
            {
                enemyBrain.StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }

        private IEnumerator PeriodicAttack()
        {
            while (playerHealth != null && !playerHealth.IsDeath)
            {
                playerHealth.Hit(damage);
                yield return new WaitForSeconds(attackInterval);
            }
            attackCoroutine = null;
        }
    }
}
