using System;
using UnityEngine;
using UnityEngine.AI;
using Player;
using Infrastructure.Services;
using Infrastructure;
using System.Collections.Generic;

namespace Enemy.StateMachine
{
    public enum BossStates
    {
        Idle,        
        Aggressive, 
        Attack,     
        StrongAttack
    }
    public class BossStateMachine : MonoBehaviour
    {
        [SerializeField] private float viewRadius = 20;
        [SerializeField] private float viewAngle = 120;
        [SerializeField] private LayerMask playerMask;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private float attackDistance = 5f;
        [SerializeField] private float strongAttackDistance = 3f;
        [SerializeField] private Transform[] waypoints;

        private Dictionary<BossStates, IEnemyState> states;
        private NavMeshAgent navMeshAgent;
        private IEnemyState currentState;
        private Animator animator;
        private Transform player;

        public Transform Player => player;
        private Enemy.HealthPoints healthPoints;
        private bool wasAttacked;
        private float attackedResetTimer;
        public bool WasAttacked { get; private set; } = false;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            states = new Dictionary<BossStates, IEnemyState>();
            healthPoints = GetComponent<HealthPoints>();
            healthPoints.OnHit += () => WasAttacked = true;

            if (healthPoints != null)
                healthPoints.OnHit += OnEnemyHit;
        }

        private void Start()
        {
            player = AllServices.Container.Single<PlayerFactory>().Player;

            if (!navMeshAgent.enabled)
            {
                navMeshAgent.enabled = true;
            }

            states.Add(BossStates.Idle, new IdleState(this, animator, navMeshAgent));
            states.Add(BossStates.Aggressive, new AggressiveState(this, animator, navMeshAgent));
            states.Add(BossStates.Attack, new AttackStateBoss(this, animator, navMeshAgent));
            states.Add(BossStates.StrongAttack, new StrongAttackState(this, animator, navMeshAgent));

            ChangeState(BossStates.Idle);
        }


        private void Update()
        {
            currentState?.Update();
        }

        public void ChangeState(BossStates state)
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

        public bool PlayerInStrongAttackDistance() =>
            Vector3.Distance(transform.position, player.position) <= strongAttackDistance;

        public Vector3 GetWaypointPositionByIndex(int i) =>
            waypoints[i].position;

        public int WaipointsCount() =>
            waypoints.Length;

        private void OnEnemyHit()
        {
            if (healthPoints != null && healthPoints.IsDeath)
                return;

            wasAttacked = true;
            attackedResetTimer = 0;

            if (!WasAttacked)
            {
                WasAttacked = true;
                ChangeState(BossStates.Aggressive);
            }
        }
    }
}
