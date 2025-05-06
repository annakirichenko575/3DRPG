using System;
using UnityEngine;
using UnityEngine.AI;
using Infrastructure.Services;
using Infrastructure;
using System.Collections.Generic;
using Infrastructure.States;

namespace Enemy.StateMachine
{
    public class WolfBehaviour : MonoBehaviour
    {
        [SerializeField] private EnemyPerception perceptions = new EnemyPerception();
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private Transform runawayPoint;

        private GameStateMachine stateMachine;
        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private Transform player;
        private Enemy.HealthPoints healthPoints;

        public Transform Player => player;

        private bool wasAttacked;
        private float attackedResetTimer;

        public bool WasAttacked { get; private set; } = false;
        public float HealthPercent => healthPoints != null ? (float)healthPoints.Health / healthPoints.MaxHealth : 1f;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            healthPoints = GetComponent<HealthPoints>();
            healthPoints.OnHit += () => WasAttacked = true;

            if (healthPoints != null)
                healthPoints.OnHit += OnEnemyHit;
        }

        private void Start()
        {
            player = AllServices.Container.Single<PlayerFactory>().Player;
            perceptions.Initialize(transform, player);
            InitStateMachine();

            if (!navMeshAgent.enabled)
            {
                navMeshAgent.enabled = true;
            }
            stateMachine.Enter<PatrolState>();
        }

        private void Update()
        {
            stateMachine?.Update();
            UpdateAttackedTimer();
        }

        private void InitStateMachine()
        {
            stateMachine = new GameStateMachine();
            var states = new Dictionary<Type, IEnemyState>
            {
                [typeof(PatrolState)] = new PatrolState(stateMachine, this, perceptions, animator, navMeshAgent),
                [typeof(ChasingState)] = new ChasingState(stateMachine, this, perceptions, animator, navMeshAgent),
                [typeof(AttackState)] = new AttackState(stateMachine, this, perceptions, animator),
                [typeof(RunawayState)] = new RunawayState(stateMachine, this, animator, navMeshAgent, runawayPoint.position),
            };
            stateMachine.Initialize(states);
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

        private void OnEnemyHit()
        {
            if (healthPoints.IsDeath)
                return; 

            wasAttacked = true;
            attackedResetTimer = 0;
        }

        private void UpdateAttackedTimer()
        {
            if (wasAttacked)
            {
                attackedResetTimer += Time.deltaTime;
                if (attackedResetTimer >= 5f)
                {
                    wasAttacked = false;
                    attackedResetTimer = 0;
                }
            }
        }

        private void OnDestroy()
        {
            if (healthPoints != null)
                healthPoints.OnHit -= OnEnemyHit;
        }


        public Vector3 GetWaypointPositionByIndex(int i) =>
            waypoints[i].position;

        public int WaipointsCount() =>
            waypoints.Length;
    }


    [Serializable]
    public class EnemyPerception
    {
        [SerializeField] private LayerMask playerMask;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private float viewRadius = 15;
        [SerializeField] private float viewAngle = 90;
        [SerializeField] private float attackDistance = 2.5f;

        private Transform transform;
        private Transform player;
        
        public void Initialize(Transform transform, Transform player)
        {
            this.transform = transform;
            this.player = player;
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

    }

}
