using UnityEngine;
using UnityEngine.AI;

namespace Enemy.StateMachine
{
    public class AggressiveState : IEnemyState
    {
        private BossStateMachine bossStateMachine;
        private Animator animator;
        private NavMeshAgent navMeshAgent;
        private Transform player;
        private Transform transform;

        private float waitTime;
        private const float startWaitTime = 4f;
        private float runSpeed = 5f;

        public AggressiveState(BossStateMachine bossStateMachine, Animator animator, NavMeshAgent navMeshAgent)
        {
            this.bossStateMachine = bossStateMachine;
            this.animator = animator;
            this.navMeshAgent = navMeshAgent;
            this.transform = bossStateMachine.transform;
            this.player = bossStateMachine.Player;
        }

        public void Enter()
        {
            waitTime = startWaitTime;
            bossStateMachine.Move(runSpeed);
            animator.SetBool("isAgressive", true);
        }

        public void Update()
        {
            if (bossStateMachine.PlayerInStrongAttackDistance())
            {
                bossStateMachine.ChangeState(BossStates.StrongAttack);
                return;
            }

            if (bossStateMachine.PlayerInAttackDistance())
            {
                bossStateMachine.ChangeState(BossStates.Attack);
                return;
            }

            if (bossStateMachine.PlayerInSight(out Transform player))
            {
                navMeshAgent.SetDestination(player.position);
            }
            else
            {
                bossStateMachine.ChangeState(BossStates.Idle);
            }
        }


        public void Exit()
        {
            bossStateMachine.Stop();
            animator.SetBool("isAgressive", false);
        }
    }
}

