using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public float speedWalk = 6;
    public float speedRun = 9;
    public float startWaitTime = 4;
    public Transform[] waypoints;
    int m_CurrentWaypointIndex;

    private float m_WaitTime;
    private bool m_IsPatrol;
    private bool m_PlayerInRange;

    private ShootingController shootingController;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        m_WaitTime = startWaitTime;
        m_IsPatrol = true;

        shootingController = GetComponent<ShootingController>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    void Update()
    {
        if (m_PlayerInRange)
        {
            // Если игрок в радиусе атаки, стрелять
            shootingController.Attack();
            m_IsPatrol = false;
            navMeshAgent.SetDestination(transform.position); // Останавливаем AI
        }
        else
        {
            Patroling();
        }
    }

    private void Patroling()
    {
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (m_WaitTime <= 0)
            {
                NextPoint();
                m_WaitTime = startWaitTime;
            }
            else
            {
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    public void SetPlayerInRange(bool inRange)
    {
        m_PlayerInRange = inRange;
    }

    private void NextPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }
}
