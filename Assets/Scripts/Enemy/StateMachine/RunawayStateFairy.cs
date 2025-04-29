using UnityEngine;
using UnityEngine.AI;

namespace Enemy.StateMachine
{
    public class RunawayStateFairy : IEnemyState
    {
        private FairyStateMachine enemyBrain;
        private Animator animator;
        private NavMeshAgent navMeshAgent;
        private Vector3 runawayPosition;

        public RunawayStateFairy(FairyStateMachine enemyBrain, Animator animator, NavMeshAgent navMeshAgent, Vector3 runawayPosition)
        {
            this.enemyBrain = enemyBrain;
            this.animator = animator;
            this.navMeshAgent = navMeshAgent;
            this.runawayPosition = runawayPosition;
        }

        public void Enter()
        {
            enemyBrain.Move(enemyBrain.SpeedRun);
            navMeshAgent.SetDestination(runawayPosition);
        }

        public void Update()
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                enemyBrain.ChangeState(FairyStates.Patrol);
            }
        }

        public void Exit()
        {
            enemyBrain.Stop();
        }
    }
}
