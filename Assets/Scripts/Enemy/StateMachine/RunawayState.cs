using UnityEngine;
using UnityEngine.AI;

namespace Enemy.StateMachine
{
    public class RunawayState : IEnemyState
    {
        private const string ToIdleName = "ToIdle";
        private const string ToRunawayName = "ToRunaway";

        private WolfStateMachine enemyBrain;
        private Animator animator;
        private NavMeshAgent navMeshAgent;
        private Vector3 targetPosition;

        private float speedRun = 5;

        public RunawayState(WolfStateMachine enemyStateMachine, 
            Animator animator, NavMeshAgent navMeshAgent, Vector3 targetPosition)
        {
            this.enemyBrain = enemyStateMachine;
            this.animator = animator;
            this.navMeshAgent = navMeshAgent;
            this.targetPosition = targetPosition;
        }

        public void Enter()
        {
            enemyBrain.Move(speedRun);
            navMeshAgent.SetDestination(targetPosition);
            animator.SetTrigger(ToRunawayName);
        }

        public void Update()
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                animator.SetTrigger(ToIdleName);
                enemyBrain.Stop();
                if (enemyBrain.PlayerInAttackDistance())
                {
                    enemyBrain.ChangeState(WolfStates.Attack);
                }
            }
        }

        public void Exit()
        {
            enemyBrain.Stop();
            animator.SetTrigger(ToIdleName);
        }
    }
}