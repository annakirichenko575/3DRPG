using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMagController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public float startWaitTime = 4;
    public float timeToRotate = 2;
    public float speedWalk = 6;
    public float speedRun = 9;

    public float viewRadius = 15;
    public float viewAngle = 90;

    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1f;
    public int edgeIterations = 4;
    public float edgeDistance = 0.5f;

    public Transform[] waypoints;
    int m_CurrentWaypointIndex;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;

    float m_WaitTime;
    float m_TimeToRotate;
    bool m_PlayerInRange;
    bool m_PlayerNear;
    bool m_IsPatrol;
    bool m_CaughtPlayer;
    bool m_IsAttacking;

    private Animator animator;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float attackRadius = 10f;
    public float bulletSpeed = 20f;
    private float nextFireTime = 2f;
    public float fireRate = 1f;

    //доделать, чтоб в начале при патрулировании не активировалась изначально пуля
    private float attackCooldown = 2f; 
    private float lastAttackTime = 0f;

    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_IsAttacking = false;
        m_PlayerInRange = false;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;

        m_CurrentWaypointIndex = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (firePoint == null)
        {
            FindFirePoint();
        }

        EnviromentView();

        if (m_PlayerInRange && Vector3.Distance(transform.position, m_PlayerPosition) <= attackRadius)
        {
            m_IsAttacking = true;
            m_IsPatrol = false;
            navMeshAgent.SetDestination(transform.position);

            if (Time.time >= nextFireTime)
            {
                Attack();
                nextFireTime = Time.time + 1f / fireRate;
                lastAttackTime = Time.time; 
            }
        }
        else if (m_PlayerInRange)
        {
            m_IsAttacking = false;
            m_IsPatrol = false;
            Chasing();
        }
        else
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                m_IsAttacking = false;
                m_IsPatrol = true;
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

    private void UpdateAnimations()
    {
        animator.SetBool("isPatroling", m_IsPatrol && navMeshAgent.velocity.magnitude > 0.1f);
        animator.SetBool("isChasing", !m_IsPatrol && navMeshAgent.velocity.magnitude > 0.1f);
        animator.SetBool("isAttacking", m_PlayerInRange && Vector3.Distance(transform.position, m_PlayerPosition) <= 10f);
    }

    private void Chasing()
    {
        m_PlayerNear = false;
        playerLastPosition = Vector3.zero;
        if (!m_CaughtPlayer)
        {
            Move(speedRun);
            navMeshAgent.SetDestination(m_PlayerPosition);
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(speedWalk);
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }

    private void Patroling()
    {
        if (m_PlayerNear)
        {
            if (m_TimeToRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false;
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (m_WaitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }



    private void Attack()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Vector3 spawnPosition = transform.position + Vector3.up * 2f; 
            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

            if (bulletRigidbody != null)
            {
                Vector3 direction = (m_PlayerPosition - spawnPosition).normalized;
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
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_PlayerInRange = true;
                    m_IsPatrol = false;
                }
                else
                {
                    m_PlayerInRange = false;
                }
                if (Vector3.Distance(transform.position, player.position) > viewRadius)
                {
                    m_PlayerInRange = false;
                }
            }
            if (m_PlayerInRange)
            {
                m_PlayerPosition = player.transform.position;
            }
        }
    }
}

