using UnityEngine;
using UnityEngine.AI;

namespace Enemy.StateMachine
{
    public class IdleState : IEnemyState
    {
        private BossStateMachine bossStateMachine;
        private Animator animator;
        private NavMeshAgent navMeshAgent;

        public IdleState(BossStateMachine bossStateMachine, Animator animator, NavMeshAgent navMeshAgent)
        {
            this.bossStateMachine = bossStateMachine;
            this.animator = animator;
            this.navMeshAgent = navMeshAgent;
        }

        public void Enter()
        {
            bossStateMachine.Stop();
            animator.SetBool("isIdle", true);
        }

        public void Update()
        {
            Transform player;

            if (GameModeManager.Instance.CurrentMode == GameMode.Peaceful)
            {
                if (bossStateMachine.WasAttacked && bossStateMachine.PlayerInSight(out player))
                {
                    bossStateMachine.ChangeState(BossStates.Aggressive);
                }
            }
            else
            {
                if (bossStateMachine.PlayerInSight(out player))
                {
                    bossStateMachine.ChangeState(BossStates.Aggressive);
                }
            }
        }


        public void Exit()
        {
            animator.SetBool("isIdle", false);
        }
    }
}

