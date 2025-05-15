using System;
using UnityEngine;
using UnityEngine.AI;
using Infrastructure;
using System.Collections.Generic;
using Infrastructure.States;

namespace Enemy.StateMachine
{
    public class FairyBehaviour : MonoBehaviour
    {
        [SerializeField] private EnemyPerception perceptions = new EnemyPerception();
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;
        
        private Transform[] waypoints;
        private EnemyStateMachine stateMachine;
        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private Enemy.HealthPoints healthPoints;
        private Transform runawayPoint;
        private PlayerFactory playerFactory;

        private float viewRadius;
        private float viewAngle;
        private float speedWalk;
        private float speedRun;
        private float attackRadius;
        private float attackDistance;
        private float bulletSpeed;
        private float fireRate;
        private float startWaitTime;

        private float nextFireTime;
        private bool wasAttacked;
        private float attackedResetTimer;

        [SerializeField] private LayerMask playerMask;
        [SerializeField] private LayerMask obstacleMask;
        private bool isPlayerInRange;
        private Vector3 playerPosition;

        public Transform Player => playerFactory.Player;
        public bool WasAttacked { get; private set; } = false;
        public float HealthPercent => healthPoints != null ? (float)healthPoints.Health / healthPoints.MaxHealth : 1f;
        public float AttackRadius => attackRadius;
        public float SpeedWalk => speedWalk;
        public float SpeedRun => speedRun;
        public float StartWaitTime => startWaitTime;

        public void Construct(
            PlayerFactory playerFactory,
            Transform[] waypoints,
            Transform runawayPoint,
            float viewRadius,
            float viewAngle,
            float speedWalk,
            float speedRun,
            float attackRadius,
            float attackDistance,
            float bulletSpeed,
            float fireRate,
            float startWaitTime)
        {
            this.playerFactory = playerFactory;
            this.waypoints = waypoints;
            this.runawayPoint = runawayPoint;
            this.viewRadius = viewRadius;
            this.viewAngle = viewAngle;
            this.speedWalk = speedWalk;
            this.speedRun = speedRun;
            this.attackRadius = attackRadius;
            this.attackDistance = attackDistance;
            this.bulletSpeed = bulletSpeed;
            this.fireRate = fireRate;
            this.startWaitTime = startWaitTime;

            InitializeComponents();
            InitStateMachine();

            if (!navMeshAgent.enabled)
                navMeshAgent.enabled = true;

            /*stateMachine.Enter<PatrolStateFairy>();*/

            Debug.Log($"Fairy constructed with {waypoints?.Length ?? 0} waypoints");
        }

        private void InitializeComponents()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            healthPoints = GetComponent<HealthPoints>();
            healthPoints.OnHit += () => WasAttacked = true;
            
            if (healthPoints != null)
            {
                healthPoints.OnHit += OnEnemyHit;
            }

            perceptions.Initialize(transform, Player);
        }

        private void InitStateMachine()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            if (navMeshAgent == null)
            {
                Debug.LogError("NavMeshAgent not found on Fairy!");
                return;
            }
            
            stateMachine = new EnemyStateMachine();
            var states = new Dictionary<Type, IEnemyState>
            {
                // Исправляем типы состояний на Fairy-версии
                [typeof(PatrolStateFairy)] = new PatrolStateFairy(stateMachine, this, animator, navMeshAgent),
                [typeof(ChasingStateFairy)] = new ChasingStateFairy(stateMachine, this, animator, navMeshAgent),
                [typeof(AttackStateFairy)] = new AttackStateFairy(stateMachine, this, animator, navMeshAgent),
                [typeof(RunawayStateFairy)] = new RunawayStateFairy(stateMachine, this, animator, navMeshAgent, runawayPoint.position),
            };
            stateMachine.Initialize(states);
            
            // Запускаем начальное состояние
            stateMachine.Enter<PatrolStateFairy>();
        }

        private void Update()
        {
            stateMachine?.Update();
            UpdateAttackedTimer();
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

        public bool PlayerInAttackDistance()
        {
            if (Player == null) return false;
            return Vector3.Distance(transform.position, Player.position) <= attackDistance;
        }

        public void Attack()
        {
            if (Time.time < nextFireTime || bulletPrefab == null || firePoint == null || Player == null)
                return;

            Vector3 directionToPlayer = (Player.position - transform.position).normalized;
            directionToPlayer.y = 0f;

            transform.rotation = Quaternion.Slerp(transform.rotation, 
                Quaternion.LookRotation(directionToPlayer), Time.deltaTime * 10f);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            if (bullet.TryGetComponent<Rigidbody>(out Rigidbody bulletRb))
            {
                bulletRb.velocity = directionToPlayer * bulletSpeed;
            }

            nextFireTime = Time.time + 1f / fireRate;
        }

        public bool PlayerInSight(out Transform playerTransform)
        {
            playerTransform = null;
            Collider[] playersInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

            foreach (var p in playersInRange)
            {
                Vector3 dirToPlayer = (p.transform.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
                {
                    float dstToPlayer = Vector3.Distance(transform.position, p.transform.position);
                    if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                    {
                        playerTransform = p.transform;
                        return true;
                    }
                }
            }

            return false;
        }

        public Vector3 GetWaypointPositionByIndex(int index)
        {
            if (waypoints.Length == 0)
                return transform.position;

            index = Mathf.Clamp(index, 0, waypoints.Length - 1);
            return waypoints[index].position;
        }

        public int WaypointsCount() => waypoints.Length;

        public void NextWaypoint()
        {
            int currentWaypointIndex = Array.IndexOf(waypoints, navMeshAgent.destination);
            currentWaypointIndex = (currentWaypointIndex + 1) % WaypointsCount();
            navMeshAgent.SetDestination(GetWaypointPositionByIndex(currentWaypointIndex));
        }

        private void EnviromentView()
        {
            Collider[] playersInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

            bool detected = false;

            foreach (var p in playersInRange)
            {
                Vector3 dirToPlayer = (p.transform.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
                {
                    float dstToPlayer = Vector3.Distance(transform.position, p.transform.position);
                    if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                    {
                        isPlayerInRange = true;
                        playerPosition = p.transform.position;
                        detected = true;
                        break;
                    }
                }
            }

            if (!detected)
            {
                isPlayerInRange = false;
            }
        }

        private void DisableExistingBullets()
        {
            GameObject[] existingBullets = GameObject.FindGameObjectsWithTag("Bullet");
            foreach (var bullet in existingBullets)
            {
                bullet.SetActive(false);
            }
        }

        private void OnEnemyHit()
        {
            if (healthPoints.IsDeath)
                return;

            wasAttacked = true;
            attackedResetTimer = 0;

            Debug.Log($"Fairy took damage! Current health: {healthPoints.Health}");
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
            {
                healthPoints.OnHit -= OnEnemyHit;
            }
        }
    }
}