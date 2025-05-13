using UnityEngine;
using UnityEngine.AI;
using Infrastructure.States;
using System.Collections;

namespace Enemy.StateMachine
{
    public class StrongAttackState : IEnemyState
    {
        private readonly EnemyStateMachine stateMachine;
        private readonly BossBehaviour boss;
        private readonly Animator animator;
        private readonly NavMeshAgent navMeshAgent;

        private Transform player;
        private int strongDamage = 50;
        private float strongAttackInterval = 6f;
        private Player.HealthPoints playerHealth;
        private Coroutine strongAttackCoroutine;

        public StrongAttackState(EnemyStateMachine stateMachine, BossBehaviour boss, Animator animator, NavMeshAgent navMeshAgent)
        {
            this.stateMachine = stateMachine;
            this.boss = boss;
            this.animator = animator;
            this.navMeshAgent = navMeshAgent;
            this.player = boss.Player;
        }

        public void Enter()
        {
            boss.Stop();
            animator.SetBool("isStrongAttacking", true);

            playerHealth = player.GetComponent<Player.HealthPoints>();

            if (strongAttackCoroutine == null && playerHealth != null)
            {
                strongAttackCoroutine = boss.StartCoroutine(PeriodicStrongAttack());
            }
        }

        public void Update()
        {
            if (!boss.PlayerInStrongAttackDistance())
            {
                stateMachine.Enter<AggressiveState>();
            }
        }

        public void Exit()
        {
            animator.SetBool("isStrongAttacking", false);

            if (strongAttackCoroutine != null)
            {
                boss.StopCoroutine(strongAttackCoroutine);
                strongAttackCoroutine = null;
            }
        }

        private IEnumerator PeriodicStrongAttack()
        {
            while (playerHealth != null && !playerHealth.IsDeath)
            {
                playerHealth.Hit(strongDamage);
                yield return new WaitForSeconds(strongAttackInterval);
            }

            strongAttackCoroutine = null;
        }
    }
}




