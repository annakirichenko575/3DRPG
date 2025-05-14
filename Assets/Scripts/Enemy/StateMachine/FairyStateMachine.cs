using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Player;
using Infrastructure;
using Infrastructure.Services;

/*namespace Enemy.StateMachine
{
    public enum FairyStates
    {
        Patrol,
        Chase,
        Attack,
        Runaway
    }

    public class FairyStateMachine : MonoBehaviour
    {
        [SerializeField] private float viewRadius = 15f;
        [SerializeField] private float viewAngle = 90f;
        [SerializeField] private LayerMask playerMask;
        [SerializeField] private LayerMask obstacleMask;

        [SerializeField] private float startWaitTime = 4f;
        [SerializeField] private float timeToRotate = 2f;
        [SerializeField] private float speedWalk = 4f;
        [SerializeField] private float speedRun = 6f;

        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;
        private float attackRadius = 8f;
        private float bulletSpeed = 20f;
        private float fireRate = 0.5f;
        private float attackDistance = 2.5f;

        [SerializeField] private Transform[] waypoints;
        [SerializeField] private Transform runawayPoint;

        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private Transform player;
        private Enemy.HealthPoints healthPoints;

        private Dictionary<FairyStates, IEnemyState> states;
        private IEnemyState currentState;

        private int currentWaypointIndex;
        private float nextFireTime;
        private bool isPlayerInRange;
        private Vector3 playerPosition;

        public Transform Player => player;
        public float AttackRadius => attackRadius;
        public float SpeedWalk => speedWalk;
        public float SpeedRun => speedRun;
        public float StartWaitTime => startWaitTime;


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

            states = new Dictionary<FairyStates, IEnemyState>
            {
                { FairyStates.Patrol, new PatrolStateFairy(this, animator, navMeshAgent) },
                { FairyStates.Chase, new ChasingStateFairy(this, animator, navMeshAgent) },
                { FairyStates.Attack, new AttackStateFairy(this, animator, navMeshAgent) },
                { FairyStates.Runaway, new RunawayStateFairy(this, animator, navMeshAgent, runawayPoint.position) }
            };
        }

        private void Start()
        {
            player = AllServices.Container.Single<PlayerFactory>().Player;

            if (!navMeshAgent.enabled)
                navMeshAgent.enabled = true;

            DisableExistingBullets();
            ChangeState(FairyStates.Patrol);
        }

        private void Update()
        {
            currentState?.Update();
            EnviromentView();
            UpdateAttackedTimer();
        }

        public void ChangeState(FairyStates newState)
        {
            currentState?.Exit();
            currentState = states[newState];
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
            navMeshAgent.speed = 0f;
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

        public bool PlayerInAttackDistance()
        {
            if (player == null) return false;
            return Vector3.Distance(transform.position, player.position) <= attackDistance;
        }

        public void Attack()
        {
            if (Time.time < nextFireTime)
                return;

            if (bulletPrefab == null || firePoint == null || player == null)
                return;

            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            directionToPlayer.y = 0f;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), Time.deltaTime * 10f);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            if (bullet.TryGetComponent<Rigidbody>(out Rigidbody bulletRb))
            {
                bulletRb.velocity = directionToPlayer * bulletSpeed;
            }

            nextFireTime = Time.time + 1f / fireRate;
        }

        public Vector3 GetWaypointPositionByIndex(int index)
        {
            if (waypoints.Length == 0)
                return transform.position;

            index = Mathf.Clamp(index, 0, waypoints.Length - 1);
            return waypoints[index].position;
        }

        public int WaypointsCount()
        {
            return waypoints.Length;
        }

        public void NextWaypoint()
        {
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
            if (healthPoints != null && healthPoints.IsDeath)
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
    }
}
*/
