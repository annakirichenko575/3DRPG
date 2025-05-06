using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Infrastructure.States;

namespace Enemy.StateMachine
{
    public class AttackState : IEnemyState
    {
        private readonly GameStateMachine stateMachine;
        private WolfBehaviour enemyBrain;
        private EnemyPerception perceptions;
        private Animator animator;
        private Transform player; 

        private int damage = 20;
        private float attackInterval = 5f;
        private Player.HealthPoints playerHealth;
        private Coroutine attackCoroutine;

        public AttackState(GameStateMachine stateMachine, WolfBehaviour enemyBrain, EnemyPerception perceptions, Animator animator)
        {
            this.stateMachine = stateMachine;
            this.enemyBrain = enemyBrain;
            this.perceptions = perceptions;
            this.animator = animator;
        }

        public void Enter()
        {
            player = enemyBrain.Player; 
            if (player == null)
            {
                Debug.LogError("Player is NULL in AttackState.Enter()");
                return;
            }

            playerHealth = player.GetComponent<Player.HealthPoints>();

            enemyBrain.Stop();
            animator.SetBool("isAttacking", true);

            if (attackCoroutine == null && playerHealth != null)
            {
                attackCoroutine = enemyBrain.StartCoroutine(PeriodicAttack());
            }
        }

        public void Update()
        {
            if (!perceptions.PlayerInAttackDistance())
            {
                stateMachine.Enter<ChasingState>();
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

