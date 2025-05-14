using UnityEngine;
using UnityEngine.AI;
using Infrastructure.States;

namespace Enemy.StateMachine
{
    public class ChasingStateFairy : IEnemyState
    {
        private const string ChasingStateName = "isChasing";

        private readonly EnemyStateMachine stateMachine;
        private FairyBehaviour enemyBrain;
        private Animator animator;
        private NavMeshAgent navMeshAgent;
        private Transform transform;
        private Transform player;

        private float waitTime;

        public ChasingStateFairy(EnemyStateMachine stateMachine, FairyBehaviour enemyBrain, Animator animator, NavMeshAgent navMeshAgent)
        {
            this.stateMachine = stateMachine;
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
                stateMachine.Enter<AttackStateFairy>(); // Исправлено
            }
            else if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                waitTime -= Time.deltaTime;
                if (waitTime <= 0)
                {
                    stateMachine.Enter<PatrolStateFairy>(); // Исправлено
                    Debug.Log("Now patroling");
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
