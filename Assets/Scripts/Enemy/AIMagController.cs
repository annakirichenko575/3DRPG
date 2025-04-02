using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMagController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private float startWaitTime = 4;
    [SerializeField] private float timeToRotate = 2;
    [SerializeField] private float speedWalk = 6;
    [SerializeField] private float speedRun = 9;

    [SerializeField] private float viewRadius = 15;
    [SerializeField] private float viewAngle = 90;

    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;

    [SerializeField] private Transform[] waypoints;
    private int mgCurrentWaypointIndex;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 mgPlayerPosition;

    private float mgWaitTime;
    private float mgTimeToRotate;
    private bool mgPlayerInRange;
    private bool mgPlayerNear;
    private bool mgIsPatrol;
    private bool mgCaughtPlayer;
    private bool mgIsAttacking;

    private Animator animator;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float attackRadius = 8f;
    [SerializeField] private float bulletSpeed = 20f;
    private float nextFireTime = 2f;
    [SerializeField] private float fireRate = 0.5f;

    private float attackCooldown = 2f;
    private float lastAttackTime = 0f;

    void Start()
    {
        mgPlayerPosition = Vector3.zero;
        mgIsPatrol = true;
        mgCaughtPlayer = false;
        mgIsAttacking = false;
        mgPlayerInRange = false;
        mgWaitTime = startWaitTime;
        mgTimeToRotate = timeToRotate;

        mgCurrentWaypointIndex = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        navMeshAgent.SetDestination(waypoints[mgCurrentWaypointIndex].position);

        animator = GetComponent<Animator>();
        DisableExistingBullets();
    }

    void Update()
    {
        if (firePoint == null)
        {
            FindFirePoint();
        }

        EnviromentView();

        if (mgPlayerInRange && Vector3.Distance(transform.position, mgPlayerPosition) <= attackRadius)
        {
            mgIsAttacking = true;
            mgIsPatrol = false;
            navMeshAgent.SetDestination(transform.position);

            if (Time.time >= nextFireTime)
            {
                Attack();
                nextFireTime = Time.time + 1f / fireRate;
                lastAttackTime = Time.time;
            }
        }
        else if (mgPlayerInRange)
        {
            mgIsAttacking = false;
            mgIsPatrol = false;
            Chasing();
        }
        else
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                mgIsAttacking = false;
                mgIsPatrol = true;
                Patroling();
            }
        }

        UpdateAnimations();
    }

    void FindFirePoint()
    {
        firePoint = transform.Find("FirePoint");

        if (firePoint == null)
        {
            Debug.LogError("FirePoint not found! Assign it manually in the Inspector.");
        }
    }

    void UpdateAnimations()
    {
        bool isInAttackRange = Vector3.Distance(transform.position, mgPlayerPosition) <= attackRadius;

        if (isInAttackRange)
        {
            animator.SetBool("isAttacking", true);
            animator.SetBool("isPatroling", false);
            animator.SetBool("isChasing", false);
        }
        else if (mgPlayerInRange)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("isPatroling", false);
            animator.SetBool("isChasing", true);
        }
        else
        {
            if (mgIsPatrol && navMeshAgent.velocity.magnitude > 0.1f)
            {
                animator.SetBool("isPatroling", true);
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", false);
            }
        }
    }

    void Chasing()
    {
        mgPlayerNear = false;
        playerLastPosition = Vector3.zero;
        if (!mgCaughtPlayer)
        {
            Move(speedRun);
            navMeshAgent.SetDestination(mgPlayerPosition);
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (mgWaitTime <= 0 && !mgCaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                mgIsPatrol = true;
                mgPlayerNear = false;
                Move(speedWalk);
                mgTimeToRotate = timeToRotate;
                mgWaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[mgCurrentWaypointIndex].position);
            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                {
                    Stop();
                    mgWaitTime -= Time.deltaTime;
                }
            }
        }
    }

    void Patroling()
    {
        if (!mgPlayerNear)
        {
            if (!navMeshAgent.hasPath)
            {
                navMeshAgent.SetDestination(waypoints[mgCurrentWaypointIndex].position);
            }

            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (mgWaitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    mgWaitTime = startWaitTime;
                }
                else
                {
                    mgWaitTime -= Time.deltaTime;
                }
            }
        }
    }

    void Attack()
    {
        if (!mgPlayerInRange || Vector3.Distance(transform.position, mgPlayerPosition) > attackRadius)
        {
            return;
        }

        Vector3 directionToPlayer = (mgPlayerPosition - transform.position).normalized;
        directionToPlayer.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

        if (bulletPrefab != null && firePoint != null)
        {
            Vector3 spawnPosition = transform.position + Vector3.up * 2f;
            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            animator.SetBool("isAttacking", true);

            if (bulletRigidbody != null)
            {
                Vector3 direction = (mgPlayerPosition - spawnPosition).normalized;
                bulletRigidbody.velocity = direction * bulletSpeed;
            }
            else
            {
                Debug.LogError("Bullet prefab does not have a Rigidbody component.");
            }
        }
        else
        {
            Debug.LogError("Bullet prefab or fire point is not assigned in the Inspector.");
        }
    }

    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    public void NextPoint()
    {
        mgCurrentWaypointIndex = (mgCurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[mgCurrentWaypointIndex].position);
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (mgWaitTime <= 0)
            {
                mgPlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[mgCurrentWaypointIndex].position);
                mgWaitTime = startWaitTime;
                mgTimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                mgWaitTime -= Time.deltaTime;
            }
        }
    }

    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        bool playerDetected = false;

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    mgPlayerInRange = true;
                    mgPlayerPosition = player.transform.position;
                    playerDetected = true;
                }
                else
                {
                    mgPlayerInRange = false;
                }
            }
        }

        if (!playerDetected)
        {
            mgPlayerInRange = false;
        }
    }

    void DisableExistingBullets()
    {
        GameObject[] existingBullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in existingBullets)
        {
            bullet.SetActive(false);
        }
    }
}
