using UnityEngine;
using UnityEngine.AI;

namespace Enemy.StateMachine
{
    public class PatrolState : IEnemyState
    {
        private const string PatrolStateName = "isPatroling";

        private EnemyStateMachine enemyBrain;
        private Animator animator;
        private NavMeshAgent navMeshAgent;
        private int currentWaypointIndex;
        private float waitTime;

        private float startWaitTime = 4;
        private float speedWalk = 4;

        public PatrolState(EnemyStateMachine enemy, Animator animator, NavMeshAgent navMeshAgent)
        {
            this.enemyBrain = enemy;
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
            if (enemyBrain.PlayerInSight(out Transform player))
            {
                //Attack();
                //or
                //Chasing();
                enemyBrain.ChangeState(new ChasingState(enemyBrain, animator, navMeshAgent, player));
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