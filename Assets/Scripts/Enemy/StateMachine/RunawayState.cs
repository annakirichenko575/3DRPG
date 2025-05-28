using Infrastructure.States;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.StateMachine
{
    public class RunawayState : IEnemyState
    {
        private const string ToIdleName = "ToIdle";
        private const string ToRunawayName = "ToRunaway";
        private readonly EnemyStateMachine stateMachine;
        private WolfBehaviour enemyBrain;
        private Animator animator;
        private NavMeshAgent navMeshAgent;

        private float speedRun = 5f;
        private float safeDistance = 15f;

        public RunawayState(EnemyStateMachine stateMachine, WolfBehaviour enemyStateMachine,
            Animator animator, NavMeshAgent navMeshAgent, Vector3 targetPosition)
        {
            this.stateMachine = stateMachine;
            this.enemyBrain = enemyStateMachine;
            this.animator = animator;
            this.navMeshAgent = navMeshAgent;
        }

        public void Enter()
        {
            animator.SetTrigger(ToRunawayName);
            enemyBrain.Move(speedRun);
            SetRunawayDestination();
        }

        public void Update()
        {
            if (Vector3.Distance(enemyBrain.transform.position, enemyBrain.Player.position) >= safeDistance)
            {
                animator.SetTrigger(ToIdleName);
                stateMachine.Enter<PatrolState>();
            }
            else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 1f)
            {
                SetRunawayDestination(); 
            }
        }

        public void Exit()
        {
            animator.SetTrigger(ToIdleName);
            enemyBrain.Stop();
        }

        private void SetRunawayDestination()
        {
            Vector3 directionAway = (enemyBrain.transform.position - enemyBrain.Player.position).normalized;
            Vector3 fleeTarget = enemyBrain.transform.position + directionAway * safeDistance;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(fleeTarget, out hit, 5f, NavMesh.AllAreas))
            {
                navMeshAgent.SetDestination(hit.position);
            }
            else
            {
                navMeshAgent.SetDestination(enemyBrain.transform.position + directionAway * 5f);
            }
        }
    }
}
