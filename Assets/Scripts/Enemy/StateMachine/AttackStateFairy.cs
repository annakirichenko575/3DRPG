using UnityEngine;
using UnityEngine.AI;

namespace Enemy.StateMachine
{
    public class AttackStateFairy : IEnemyState
    {
        private const string AttackStateName = "isAttacking";

        private FairyStateMachine enemyBrain;
        private Animator animator;
        private NavMeshAgent navMeshAgent;

        private Transform player;
        private Transform transform;

        public AttackStateFairy(FairyStateMachine enemyBrain, Animator animator, NavMeshAgent navMeshAgent)
        {
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
            if (player == null)
            {
                enemyBrain.ChangeState(FairyStates.Patrol);
                return;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer > enemyBrain.AttackRadius)
            {
                enemyBrain.ChangeState(FairyStates.Chase);
                return;
            }

            enemyBrain.Attack();
        }

        public void Exit()
        {
            animator.SetBool(AttackStateName, false);
        }
    }
}
