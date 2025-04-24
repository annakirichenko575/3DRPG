using UnityEngine;
using UnityEngine.AI;

namespace Enemy.StateMachine
{
    public class ChasingState : IEnemyState
    {
        private WolfStateMachine enemyBrain;
        private Animator animator;
        private NavMeshAgent navMeshAgent;
        private Transform player;
        private Transform transform;
        private float waitTime;

        private float speedRun = 5;

        //private float timeToRotate = 1;
        private float startWaitTime = 4;

        public ChasingState(WolfStateMachine enemyBrain,
            Animator animator, NavMeshAgent navMeshAgent)
        {
            this.enemyBrain = enemyBrain;
            transform = enemyBrain.transform;
            this.animator = animator;
            this.navMeshAgent = navMeshAgent;
            this.player = enemyBrain.Player;
        }

        public void Enter()
        {
            waitTime = startWaitTime;
            enemyBrain.Move(speedRun);
            navMeshAgent.SetDestination(player.position);
        }

        public void Update()
        {
            animator.SetBool("isChasing", navMeshAgent.velocity.magnitude > 0.1f);
            if (enemyBrain.PlayerInSight(out Transform player))
            {
                navMeshAgent.SetDestination(player.position);
            }

            if (enemyBrain.PlayerInAttackDistance())
            {
                enemyBrain.ChangeState(WolfStates.Attack);
            }
            else if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                enemyBrain.Stop();
                waitTime -= Time.deltaTime;
                if (waitTime <= 0)
                {
                    enemyBrain.ChangeState(WolfStates.Patrol);
                }
            }
        }

        public void Exit()
        {
            enemyBrain.Stop();
            animator.SetBool("isChasing", false);
        }
    }
}
