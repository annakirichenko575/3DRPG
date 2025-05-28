using UnityEngine;
using UnityEngine.AI;
using Infrastructure.States;

namespace Enemy.StateMachine
{
    public class AggressiveState : IEnemyState
    {
        private readonly EnemyStateMachine stateMachine;
        private readonly BossBehaviour boss;
        private readonly Animator animator;
        private readonly NavMeshAgent navMeshAgent;

        private float waitTime;
        private const float startWaitTime = 4f;
        private float runSpeed = 5f;

        public AggressiveState(EnemyStateMachine stateMachine, BossBehaviour boss, Animator animator, NavMeshAgent navMeshAgent)
        {
            this.stateMachine = stateMachine;
            this.boss = boss;
            this.animator = animator;
            this.navMeshAgent = navMeshAgent;
        }

        public void Enter()
        {
            waitTime = startWaitTime;
            boss.Move(runSpeed);
            animator.SetBool("isAgressive", true);
        }

        public void Update()
        {
            if (boss.PlayerInStrongAttackDistance())
            {
                stateMachine.Enter<StrongAttackState>();
                return;
            }

            if (boss.PlayerInAttackDistance())
            {
                stateMachine.Enter<AttackStateBoss>();
                return;
            }

            if (boss.PlayerInSight(out Transform player))
            {
                navMeshAgent.SetDestination(player.position);
            }
            else
            {
                stateMachine.Enter<IdleState>();
            }
        }

        public void Exit()
        {
            boss.Stop();
            animator.SetBool("isAgressive", false);
        }
    }
}


