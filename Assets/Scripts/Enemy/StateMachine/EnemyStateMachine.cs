using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.StateMachine
{
    public class EnemyStateMachine : MonoBehaviour
    {
        [SerializeField] private float viewRadius = 15;
        [SerializeField] private float viewAngle = 90;
        [SerializeField] private LayerMask playerMask;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private float attackDistance = 6f;
        [SerializeField] private Transform[] waypoints;

        private NavMeshAgent navMeshAgent;
        private IEnemyState currentState;
        private Animator animator;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            animator = GetComponent<Animator>();
            if (!navMeshAgent.enabled)
            {
                navMeshAgent.enabled = true;
            }
            ChangeState(new PatrolState(this, animator, navMeshAgent));
        }

        private void Update()
        {
            currentState?.Update();
        }

        public void ChangeState(IEnemyState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }


        public void Move(float speed)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = speed;
        }

        public void Stop()
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.speed = 0;
        }

        public bool PlayerInSight(out Transform player) =>
            PlayerInRange(out player) && ObstacleCheck(player.position) == false;

        public bool PlayerInRange(out Transform player)
        {
            
            Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

            for (int i = 0; i < playerInRange.Length; i++)
            {
                player = playerInRange[i].transform;
                Vector3 dirToPlayer = (player.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
                { 
                    return true;
                }
            }
            player = null;
            return false;
        }

        public bool ObstacleCheck(Vector3 playerPosition)
        {
            Vector3 dirToPlayer = (playerPosition - transform.position).normalized;
            float dstToPlayer = Vector3.Distance(transform.position, playerPosition);
            return Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask);
        }

        public bool PlayerInAttackDistance(Vector3 playerPosition) =>
            Vector3.Distance(transform.position, playerPosition) <= attackDistance;

        public Vector3 GetWaypointPositionByIndex(int i) =>
            waypoints[i].position;

        public int WaipointsCount() =>
            waypoints.Length;
    }

    public class ChasingState : IEnemyState
    {
        private EnemyStateMachine enemyBrain;
        private Animator animator;
        private NavMeshAgent navMeshAgent;
        private Transform player;
        private Transform transform;
        private float waitTime;
        
        private float speedRun = 5;
        
        //private float timeToRotate = 1;
        private float startWaitTime = 4;

        public ChasingState(EnemyStateMachine enemyBrain, 
            Animator animator, NavMeshAgent navMeshAgent, Transform player)
        {
            this.enemyBrain = enemyBrain;
            transform = enemyBrain.transform;
            this.animator = animator;
            this.navMeshAgent = navMeshAgent;
            this.player = player;
        }

        public void Enter()
        {
            waitTime = startWaitTime;
            enemyBrain.Move(speedRun);
            
            animator.SetBool("isChasing", true);
        }

        public void Update()
        {
            animator.SetBool("isChasing", navMeshAgent.velocity.magnitude > 0.1f);
            if (enemyBrain.PlayerInSight(out Transform player))
            {
                navMeshAgent.SetDestination(player.position);
            }
            
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                enemyBrain.Stop();
                //if (Vector3.Distance(transform.position, player.position) >= 2.5f)
                if (enemyBrain.PlayerInAttackDistance(this.player.position)) 
                {
                    Debug.Log("Attack");
                    animator.SetBool("isAttacking", true);
                }
                else
                {
                    waitTime -= Time.deltaTime;
                    if (waitTime <= 0)
                    {
                        enemyBrain.ChangeState(new PatrolState(enemyBrain, animator, navMeshAgent));
                        Debug.Log("Patrol");
                    }
                    Debug.Log("wait");
                    animator.SetBool("isAttacking", false);
                }
            }
        }

        public void Exit()
        {
            animator.SetBool("isChasing", false);
        }
    }
}