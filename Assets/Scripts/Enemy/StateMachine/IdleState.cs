using UnityEngine;
using UnityEngine.AI;
using Infrastructure.States;

namespace Enemy.StateMachine
{
    public class IdleState : IEnemyState
    {
        private readonly EnemyStateMachine stateMachine;
        private readonly BossBehaviour boss;
        private readonly Animator animator;
        private readonly NavMeshAgent navMeshAgent;

        public IdleState(EnemyStateMachine stateMachine, BossBehaviour boss, Animator animator, NavMeshAgent navMeshAgent)
        {
            this.stateMachine = stateMachine;
            this.boss = boss;
            this.animator = animator;
            this.navMeshAgent = navMeshAgent;
        }

        public void Enter()
        {
            boss.Stop();
            animator.SetBool("isIdle", true);
        }

        public void Update()
        {
            Transform player;

            if (GameModeManager.Instance.CurrentMode == GameMode.Peaceful)
            {
                if (boss.WasAttacked && boss.PlayerInSight(out player))
                {
                    stateMachine.Enter<AggressiveState>();
                }
            }
            else
            {
                if (boss.PlayerInSight(out player))
                {
                    stateMachine.Enter<AggressiveState>();
                }
            }
        }

        public void Exit()
        {
            animator.SetBool("isIdle", false);
        }
    }
}


