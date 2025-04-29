using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace Enemy.StateMachine
{
    public class StrongAttackState : IEnemyState
    {
        private BossStateMachine bossStateMachine;
        private Animator animator;
        private NavMeshAgent navMeshAgent;
        private Transform player;

        private int strongDamage = 50; 
        private float strongAttackInterval = 6f; 
        private Player.HealthPoints playerHealth;
        private Coroutine strongAttackCoroutine;

        public StrongAttackState(BossStateMachine bossStateMachine, Animator animator, NavMeshAgent navMeshAgent)
        {
            this.bossStateMachine = bossStateMachine;
            this.animator = animator;
            this.navMeshAgent = navMeshAgent;
            this.player = bossStateMachine.Player;
        }

        public void Enter()
        {
            bossStateMachine.Stop();
            animator.SetBool("isStrongAttacking", true);

            playerHealth = player.GetComponent<Player.HealthPoints>();

            if (strongAttackCoroutine == null && playerHealth != null)
            {
                strongAttackCoroutine = bossStateMachine.StartCoroutine(PeriodicStrongAttack());
            }
        }

        public void Update()
        {
            if (!bossStateMachine.PlayerInStrongAttackDistance())
            {
                bossStateMachine.ChangeState(BossStates.Aggressive);
            }
        }

        public void Exit()
        {
            animator.SetBool("isStrongAttacking", false);

            if (strongAttackCoroutine != null)
            {
                bossStateMachine.StopCoroutine(strongAttackCoroutine);
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



