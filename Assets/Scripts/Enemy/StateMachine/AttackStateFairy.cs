using UnityEngine;
using UnityEngine.AI;
using Infrastructure.States;

namespace Enemy.StateMachine
{
    public class AttackStateFairy : IEnemyState
    {
        private const string AttackStateName = "isAttacking";

        private readonly EnemyStateMachine stateMachine;
        private FairyBehaviour enemyBrain;
        private Animator animator;
        private NavMeshAgent navMeshAgent;

        private Transform player;
        private Transform transform;

        public AttackStateFairy(EnemyStateMachine stateMachine, FairyBehaviour enemyBrain, Animator animator, NavMeshAgent navMeshAgent)
        {
            this.stateMachine = stateMachine;
            this.enemyBrain = enemyBrain;
            this.animator = animator;
            this.navMeshAgent = navMeshAgent;
            this.transform = enemyBrain.transform;
        }

        public void Enter()
        {
            player = enemyBrain.Player;
            enemyBrain.Stop();
            animator.SetBool(AttackStateName, true);
        }

        public void Update()
        {
            if (player == null || !player.gameObject.activeInHierarchy)
            {
                stateMachine.Enter<PatrolStateFairy>(); // Исправлено
                return;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > enemyBrain.AttackRadius)
            {
                stateMachine.Enter<ChasingStateFairy>(); // Исправлено
                Debug.Log("Now chasing");
            }
            else
            {
                enemyBrain.Attack();
            }
        }
        public void Exit()
        {
            animator.SetBool(AttackStateName, false);
        }
    }
}