using UnityEngine;
using UnityEngine.AI;

namespace Enemy.StateMachine
{
    public class RunawayStateFairy : IEnemyState
    {
        private const string RunawayAnimBool = "ToRunaway";

        private readonly FairyStateMachine fairy;
        private readonly Animator animator;
        private readonly NavMeshAgent navMeshAgent;

        private const float runawayDistance = 10f;
        private const float checkInterval = 1f;
        private float checkTimer;

        public RunawayStateFairy(FairyStateMachine fairy, Animator animator, NavMeshAgent navMeshAgent, Vector3 targetPosition)
        {
            this.fairy = fairy;
            this.animator = animator;
            this.navMeshAgent = navMeshAgent;
        }

        public void Enter()
        {
            animator.SetBool(RunawayAnimBool, true);
            fairy.Move(fairy.SpeedRun);
            RunAwayFromPlayer();
        }

        public void Update()
        {
            checkTimer += Time.deltaTime;

            if (checkTimer >= checkInterval)
            {
                RunAwayFromPlayer();
                checkTimer = 0f;
            }

            if (!fairy.WasAttacked || fairy.HealthPercent > 0.5f)
            {
                fairy.ChangeState(FairyStates.Patrol);
            }
        }

        public void Exit()
        {
            animator.SetBool(RunawayAnimBool, false);
        }

        private void RunAwayFromPlayer()
        {
            if (fairy.Player == null)
                return;

            Vector3 directionAway = (fairy.transform.position - fairy.Player.position).normalized;
            Vector3 newDestination = fairy.transform.position + directionAway * runawayDistance;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(newDestination, out hit, 5f, NavMesh.AllAreas))
            {
                navMeshAgent.SetDestination(hit.position);
            }
        }
    }
}
