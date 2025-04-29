using UnityEngine;
using UnityEngine.AI;

namespace Enemy.StateMachine
{
    public class ChasingStateFairy : IEnemyState
    {
        private const string ChasingStateName = "isChasing";

        private FairyStateMachine enemyBrain;
        private Animator animator;
        private NavMeshAgent navMeshAgent;
        private Transform transform;
        private Transform player;

        private float waitTime;

        public ChasingStateFairy(FairyStateMachine enemyBrain, Animator animator, NavMeshAgent navMeshAgent)
        {
            this.enemyBrain = enemyBrain;
            this.animator = animator;
            this.navMeshAgent = navMeshAgent;
            this.transform = enemyBrain.transform;
        }

        public void Enter()
        {
            waitTime = enemyBrain.StartWaitTime;
            player = enemyBrain.Player;
            enemyBrain.Move(enemyBrain.SpeedRun);
            navMeshAgent.SetDestination(player.position);
            animator.SetBool(ChasingStateName, true);
        }

        public void Update()
        {
            animator.SetBool(ChasingStateName, navMeshAgent.velocity.magnitude > 0.1f);

            if (enemyBrain.PlayerInSight(out Transform targetPlayer))
            {
                navMeshAgent.SetDestination(targetPlayer.position);
            }

            if (enemyBrain.PlayerInAttackDistance())
            {
                enemyBrain.ChangeState(FairyStates.Attack);
            }
            else if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                enemyBrain.Stop();
                waitTime -= Time.deltaTime;
                if (waitTime <= 0)
                {
                    enemyBrain.ChangeState(FairyStates.Patrol);
                }
            }
        }

        public void Exit()
        {
            enemyBrain.Stop();
            animator.SetBool(ChasingStateName, false);
        }
    }
}
