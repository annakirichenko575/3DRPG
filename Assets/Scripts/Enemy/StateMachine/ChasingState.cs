using UnityEngine;
using UnityEngine.AI;

namespace Enemy.StateMachine
{
    public class ChasingState : IEnemyState
    {
        private WolfStateMachine enemyBrain;
        private Animator animator;
        private NavMeshAgent navMeshAgent;
        private Transform transform;
        private Transform player; // Теперь получаем позже!

        private float waitTime;
        private float speedRun = 5f;
        private float startWaitTime = 4f;

        public ChasingState(WolfStateMachine enemyBrain, Animator animator, NavMeshAgent navMeshAgent)
        {
            this.enemyBrain = enemyBrain;
            transform = enemyBrain.transform;
            this.animator = animator;
            this.navMeshAgent = navMeshAgent;
        }

        public void Enter()
        {
            player = enemyBrain.Player; // <-- Здесь получаем игрока!
            waitTime = startWaitTime;
            enemyBrain.Move(speedRun);

            if (player != null)
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
