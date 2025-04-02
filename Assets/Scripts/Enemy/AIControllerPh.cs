using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControllerPh : MonoBehaviour
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
    private int phCurrentWaypointIndex;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 phPlayerPosition;

    private float phWaitTime;
    private float phTimeToRotate;
    private bool phPlayerInRange;
    private bool phPlayerNear;
    private bool phIsPatrol;
    private bool phCaughtPlayer;

    private Animator animator;
    public bool IsPlayerInRange => phPlayerInRange;

    void Start()
    {
        phPlayerPosition = Vector3.zero;
        phIsPatrol = true;
        phCaughtPlayer = false;
        phPlayerInRange = false;
        phWaitTime = startWaitTime;
        phTimeToRotate = timeToRotate;

        phCurrentWaypointIndex = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        if (!navMeshAgent.enabled)
        {
            navMeshAgent.enabled = true;
        }
        navMeshAgent.SetDestination(waypoints[phCurrentWaypointIndex].position);

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        EnviromentView();

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
        {
            NextPoint();
        }

        if (!phIsPatrol)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }

        UpdateAnimations();
    }

    void UpdateAnimations()
    {
        animator.SetBool("isPatroling", phIsPatrol && navMeshAgent.velocity.magnitude > 0.1f);
        animator.SetBool("isChasing", !phIsPatrol && navMeshAgent.velocity.magnitude > 0.1f);
        animator.SetBool("isAttacking", phPlayerInRange && Vector3.Distance(transform.position, phPlayerPosition) <= 10f);
    }

    void Chasing()
    {
        phPlayerNear = false;
        playerLastPosition = Vector3.zero;
        if (!phCaughtPlayer)
        {
            Move(speedRun);
            navMeshAgent.SetDestination(phPlayerPosition);
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (phWaitTime <= 0 && !phCaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                phIsPatrol = true;
                phPlayerNear = false;
                Move(speedWalk);
                phTimeToRotate = timeToRotate;
                phWaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[phCurrentWaypointIndex].position);
            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                {
                    Stop();
                    phWaitTime -= Time.deltaTime;
                }
            }
        }
    }

    void Patroling()
    {
        if (!phPlayerNear)
        {
            if (!navMeshAgent.hasPath)
            {
                navMeshAgent.SetDestination(waypoints[phCurrentWaypointIndex].position);
            }

            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (phWaitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    phWaitTime = startWaitTime;
                }
                else
                {
                    phWaitTime -= Time.deltaTime;
                }
            }
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
        phCurrentWaypointIndex = (phCurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[phCurrentWaypointIndex].position);
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (phWaitTime <= 0)
            {
                phPlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[phCurrentWaypointIndex].position);
                phWaitTime = startWaitTime;
                phTimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                phWaitTime -= Time.deltaTime;
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
                    phPlayerInRange = true;
                    phIsPatrol = false;
                }
                else
                {
                    phPlayerInRange = false;
                }
                if (Vector3.Distance(transform.position, player.position) > viewRadius)
                {
                    phPlayerInRange = false;
                }
            }
            if (phPlayerInRange)
            {
                phPlayerPosition = player.transform.position;
            }
        }
    }
}