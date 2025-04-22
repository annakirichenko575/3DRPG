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
            UpdateAnimations();

        }

        void UpdateAnimations()
        {
           // 
            //animator.SetBool("isChasing", !isPatrol && navMeshAgent.velocity.magnitude > 0.1f);
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

        public bool PlayerInSight(out Transform player)
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
            //animator.SetBool("isPatroling", isPatrol && navMeshAgent.velocity.magnitude > 0.1f);
            animator.SetBool(PatrolStateName, true);
            waitTime = startWaitTime;
            currentWaypointIndex = 0;
            enemyBrain.Move(speedWalk);

        }

        public void Update()
        {
            //TODO:
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
            {
                NextPoint(); 
            }
            Patroling();
        }

        public void Exit()
        {
            animator.SetBool(PatrolStateName, false);
        }

        private void Patroling()
        {

            if (!navMeshAgent.hasPath)
            {
                navMeshAgent.SetDestination(enemyBrain.GetWaypointPositionByIndex(currentWaypointIndex));
            }

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
            currentWaypointIndex = (currentWaypointIndex + 1) % enemyBrain.WaipointsCount();
            navMeshAgent.SetDestination(enemyBrain.GetWaypointPositionByIndex(currentWaypointIndex));
        }
    }
}