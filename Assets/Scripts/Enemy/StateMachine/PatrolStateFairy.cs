using UnityEngine;
using UnityEngine.AI;
using Infrastructure.States;

namespace Enemy.StateMachine
{
    public class PatrolStateFairy : IEnemyState
    {
        private const string PatrolStateName = "isPatroling";

        private readonly EnemyStateMachine stateMachine;

        private FairyBehaviour enemyBrain;
        private Animator animator;
        private NavMeshAgent navMeshAgent;

        private int currentWaypointIndex;
        private float waitTime;
        private const float startWaitTime = 4f;

        public PatrolStateFairy(EnemyStateMachine stateMachine, FairyBehaviour enemy, Animator animator, NavMeshAgent navMeshAgent)
        {
            this.stateMachine = stateMachine;
            this.enemyBrain = enemy;
            this.animator = animator;
            this.navMeshAgent = navMeshAgent;
        }

        public void Enter()
        {
            animator.SetBool(PatrolStateName, true);
            waitTime = enemyBrain.StartWaitTime;
            currentWaypointIndex = 0;
            enemyBrain.Move(enemyBrain.SpeedWalk);
            SetNextPoint();
        }

        public void Update()
{

        if (GameModeManager.Instance.CurrentMode == GameMode.Peaceful)
        {
            if (enemyBrain.WasAttacked && enemyBrain.HealthPercent < 0.3f)
            {
                stateMachine.Enter<RunawayStateFairy>();
                Debug.Log("Now running away");
                return;
            }
        }
        else if (enemyBrain.PlayerInSight(out Transform player))
        {
            Debug.Log("Player detected, switching to Chase");
            stateMachine.Enter<ChasingStateFairy>();
            return;
        }

        Patrol();
    }

        public void Exit()
        {
            animator.SetBool(PatrolStateName, false);
        }

        private void Patrol()
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (waitTime <= 0)
                {
                    SetNextPoint();
                    enemyBrain.Move(enemyBrain.SpeedWalk);
                    waitTime = enemyBrain.StartWaitTime;
                    Debug.Log("Moving to next waypoint");
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
        }

        private void SetNextPoint()
        {
            if (enemyBrain.WaypointsCount() == 0)
            {
                Debug.LogWarning("No waypoints available");
                return;
            }

            currentWaypointIndex = (currentWaypointIndex + 1) % enemyBrain.WaypointsCount();
            var nextPos = enemyBrain.GetWaypointPositionByIndex(currentWaypointIndex);
            navMeshAgent.SetDestination(nextPos);
            Debug.Log($"Setting destination to waypoint {currentWaypointIndex} at {nextPos}");
        }
    }
}
