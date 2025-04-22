using Enemy.StateMachine;
using UnityEngine;
using UnityEngine.AI;

public class AIControllerPh : MonoBehaviour
{
    private EnemyStateMachine enemyBrain;

    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private float timeToRotate = 2;
    

    [SerializeField] private Animator animator;

    [SerializeField] private Transform[] waypoints;

    private float speedRun = 5;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 phPlayerPosition;

    private float phTimeToRotate;
    
    private bool phCaughtPlayer;


    void Start()
    {
        phPlayerPosition = Vector3.zero;
        phCaughtPlayer = false;
        
        phTimeToRotate = timeToRotate;

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {

        if (enemyBrain.PlayerInSight(out Transform player) 
            && enemyBrain.ObstacleCheck(player.position))
        {
            if (enemyBrain.PlayerInAttackDistance(player.position) == false)
                animator.SetBool("isAttacking", true);
            //Chasing();
        }
        else
        {
            animator.SetBool("isAttacking", false);

            //Patroling();
        }

    }

    
    /*
    void Chasing()
    {
        playerLastPosition = Vector3.zero;
        if (!phCaughtPlayer)
        {
            Move(speedRun);
            navMeshAgent.SetDestination(phPlayerPosition);
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (phWaitTime <= 0 && !phCaughtPlayer 
                && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
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
    */
    

    


    
}