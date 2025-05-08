using Infrastructure.States;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.StateMachine
{
    public class ChasingState : IEnemyState
    {
        private readonly EnemyStateMachine stateMachine;
        private WolfBehaviour enemyBrain;
        private EnemyPerception perceptions;
        private Animator animator;
        private NavMeshAgent navMeshAgent;
        private Transform transform;
        private Transform player; 

        private float waitTime;
        private float speedRun = 5f;
        private float startWaitTime = 4f;

        public ChasingState(EnemyStateMachine stateMachine, WolfBehaviour enemyBrain, EnemyPerception perceptions, Animator animator, NavMeshAgent navMeshAgent)
        {
            this.stateMachine = stateMachine;
            this.enemyBrain = enemyBrain;
            this.perceptions = perceptions;
            transform = enemyBrain.transform;
            this.animator = animator;
            this.navMeshAgent = navMeshAgent;
        }

        public void Enter()
        {
            player = enemyBrain.Player; 
            waitTime = startWaitTime;
            enemyBrain.Move(speedRun);

            if (player != null)
                navMeshAgent.SetDestination(player.position);
        }

        public void Update()
        {
            animator.SetBool("isChasing", navMeshAgent.velocity.magnitude > 0.1f);

            if (perceptions.PlayerInSight(out Transform player))
            {
                navMeshAgent.SetDestination(player.position);
            }

            if (perceptions.PlayerInAttackDistance())
            {
                stateMachine.Enter<AttackState>();
            }
            else if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                enemyBrain.Stop();
                waitTime -= Time.deltaTime;
                if (waitTime <= 0)
                {
                    stateMachine.Enter<PatrolState>();
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
