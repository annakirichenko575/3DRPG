using Infrastructure.States;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.StateMachine
{
    public class PatrolState : IEnemyState
    {
        private const string PatrolStateName = "isPatroling";
        private readonly EnemyStateMachine stateMachine;
        private EnemyPerception perception;
        private WolfBehaviour enemyBrain;
        private Animator animator;
        private NavMeshAgent navMeshAgent;
        private int currentWaypointIndex;
        private float waitTime;

        private float startWaitTime = 4;
        private float speedWalk = 4;

        public PatrolState(EnemyStateMachine stateMachine, WolfBehaviour enemy, 
            EnemyPerception perception, Animator animator, NavMeshAgent navMeshAgent)
        {
            this.stateMachine = stateMachine;
            this.enemyBrain = enemy;
            this.perception = perception;
            this.animator = animator;
            this.navMeshAgent = navMeshAgent;
        }

        public void Enter()
        {
            animator.SetBool(PatrolStateName, true);
            waitTime = startWaitTime;
            currentWaypointIndex = 0;
            enemyBrain.Move(speedWalk);
            NextPoint();
        }

        public void Update()
        {
            if (GameModeManager.Instance.CurrentMode == GameMode.Peaceful)
            {
                Patroling(); 
                if (enemyBrain.HealthPercent < 0.3f)
                {
                    stateMachine.Enter<RunawayState>();
                }
                else if (enemyBrain.WasAttacked)
                {
                    return;
                }

                return;
            }

            if (perception.PlayerInSight(out Transform player))
            {
                stateMachine.Enter<ChasingState>();
            }
            else
            {
                animator.SetBool("isPatroling", navMeshAgent.velocity.magnitude > 0.1f);
                Patroling();
            }
        }

        public void Exit()
        {
            animator.SetBool(PatrolStateName, false);
        }

        private void Patroling()
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (waitTime <= 0)
                {
                    NextPoint();
                    enemyBrain.Move(speedWalk);
                    waitTime = startWaitTime;
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }

        }

        private void NextPoint()
        {
            if (navMeshAgent.hasPath)
                currentWaypointIndex = (currentWaypointIndex + 1) % enemyBrain.WaipointsCount();
            
            navMeshAgent.SetDestination(enemyBrain.GetWaypointPositionByIndex(currentWaypointIndex));
        }
    }
}