using UnityEngine;
using UnityEngine.AI;
using Infrastructure.States;
using System.Collections;

namespace Enemy.StateMachine
{
    public class AttackStateBoss : IEnemyState
    {
        private readonly EnemyStateMachine stateMachine;
        private readonly BossBehaviour boss;
        private readonly Animator animator;
        private readonly NavMeshAgent navMeshAgent;

        private Transform player;
        private int damage = 40;
        private float attackInterval = 3f;
        private Player.HealthPoints playerHealth;
        private Coroutine attackCoroutine;

        public AttackStateBoss(EnemyStateMachine stateMachine, BossBehaviour boss, Animator animator, NavMeshAgent navMeshAgent)
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
            animator.SetBool("isAttacking", true);

            playerHealth = player.GetComponent<Player.HealthPoints>();

            if (attackCoroutine == null && playerHealth != null)
            {
                attackCoroutine = boss.StartCoroutine(PeriodicAttack());
            }
        }

        public void Update()
        {
            if (boss.PlayerInStrongAttackDistance())
            {
                stateMachine.Enter<StrongAttackState>();
                return;
            }

            if (!boss.PlayerInAttackDistance())
            {
                stateMachine.Enter<AggressiveState>();
            }
        }

        public void Exit()
        {
            animator.SetBool("isAttacking", false);

            if (attackCoroutine != null)
            {
                boss.StopCoroutine(attackCoroutine);
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



