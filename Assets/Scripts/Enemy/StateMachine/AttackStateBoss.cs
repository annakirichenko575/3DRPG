using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace Enemy.StateMachine
{
    public class AttackStateBoss : IEnemyState
    {
        private BossStateMachine bossStateMachine;
        private Animator animator;
        private NavMeshAgent navMeshAgent;
        private Transform player;

        private int damage = 40; 
        private float attackInterval = 3f; 
        private Player.HealthPoints playerHealth;
        private Coroutine attackCoroutine;

        public AttackStateBoss(BossStateMachine bossStateMachine, Animator animator, NavMeshAgent navMeshAgent)
        {
            this.bossStateMachine = bossStateMachine;
            this.animator = animator;
            this.navMeshAgent = navMeshAgent;
            this.player = bossStateMachine.Player;
        }

        public void Enter()
        {
            bossStateMachine.Stop();
            animator.SetBool("isAttacking", true);

            playerHealth = player.GetComponent<Player.HealthPoints>();

            if (attackCoroutine == null && playerHealth != null)
            {
                attackCoroutine = bossStateMachine.StartCoroutine(PeriodicAttack());
            }
        }

        public void Update()
        {
            if (bossStateMachine.PlayerInStrongAttackDistance())
            {
                bossStateMachine.ChangeState(BossStates.StrongAttack);
                return;
            }

            if (!bossStateMachine.PlayerInAttackDistance())
            {
                bossStateMachine.ChangeState(BossStates.Aggressive);
            }
        }


        public void Exit()
        {
            animator.SetBool("isAttacking", false);

            if (attackCoroutine != null)
            {
                bossStateMachine.StopCoroutine(attackCoroutine);
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


