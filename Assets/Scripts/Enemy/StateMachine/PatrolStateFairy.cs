using UnityEngine;
using UnityEngine.AI;

namespace Enemy.StateMachine
{
    public class PatrolStateFairy : IEnemyState
    {
        private const string PatrolStateName = "isPatroling";

        private FairyStateMachine enemyBrain;
        private Animator animator;
        private NavMeshAgent navMeshAgent;

        private int currentWaypointIndex;
        private float waitTime;
        private const float startWaitTime = 4f;

        public PatrolStateFairy(FairyStateMachine enemy, Animator animator, NavMeshAgent navMeshAgent)
        {
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
                Patrol();

                if (enemyBrain.WasAttacked && enemyBrain.HealthPercent < 0.3f)
                {
                    enemyBrain.ChangeState(FairyStates.Runaway);
                }

                return;
            }
            if (enemyBrain.PlayerInSight(out Transform player))
            {
                enemyBrain.ChangeState(FairyStates.Chase);
            }
            else
            {
                animator.SetBool(PatrolStateName, navMeshAgent.velocity.magnitude > 0.1f);
                Patrol();
            }
        }

        public void Exit()
        {
            animator.SetBool(PatrolStateName, false);
        }

        private void Patrol()
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (waitTime <= 0)
                {
                    SetNextPoint();
                    enemyBrain.Move(enemyBrain.SpeedWalk);
                    waitTime = enemyBrain.StartWaitTime;
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
        }

        private void SetNextPoint()
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % enemyBrain.WaypointsCount();
            navMeshAgent.SetDestination(enemyBrain.GetWaypointPositionByIndex(currentWaypointIndex));
        }
    }
}
