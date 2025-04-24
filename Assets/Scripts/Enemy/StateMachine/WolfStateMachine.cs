using System;
using UnityEngine;
using UnityEngine.AI;
using Player;
using Infrastructure.Services;
using Infrastructure;
using System.Collections.Generic;

namespace Enemy.StateMachine
{
    public enum WolfStates
    {
        Patrol,
        Chase,
        Attack,
        Runaway
    }

    public class WolfStateMachine : MonoBehaviour
    {
        [SerializeField] private float viewRadius = 15;
        [SerializeField] private float viewAngle = 90;
        [SerializeField] private LayerMask playerMask;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private float attackDistance = 2.5f;
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private Transform runawayPoint;

        private Dictionary<WolfStates, IEnemyState> states;
        private NavMeshAgent navMeshAgent;
        private IEnemyState currentState;
        private Animator animator;
        private Transform player;

        public Transform Player => player;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            states = new Dictionary<WolfStates, IEnemyState>
            {
                { WolfStates.Patrol, new PatrolState(this, animator, navMeshAgent) },
                { WolfStates.Chase, new ChasingState(this, animator, navMeshAgent) },
                { WolfStates.Attack, new AttackState(this, animator, navMeshAgent) },
                { WolfStates.Runaway , new RunawayState(this, animator, navMeshAgent, runawayPoint.position) }
            };
        }

        private void Start()
        {
            player = AllServices.Container.Single<PlayerFactory>().Player;
            if (!navMeshAgent.enabled)
            {
                navMeshAgent.enabled = true;
            }
            ChangeState(WolfStates.Patrol);
        }

        private void Update()
        {
            currentState?.Update();
            //ChangeState(WolfStates.Runaway);
            
        }

        public void ChangeState(WolfStates state)
        {
            currentState?.Exit();
            currentState = states[state];
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

        public bool PlayerInAttackDistance() =>
            Vector3.Distance(transform.position, player.position) <= attackDistance;

        public Vector3 GetWaypointPositionByIndex(int i) =>
            waypoints[i].position;

        public int WaipointsCount() =>
            waypoints.Length;
    }
}
