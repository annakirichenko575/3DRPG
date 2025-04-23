using Enemy.StateMachine;
using UnityEngine;
using UnityEngine.AI;

public class AIControllerPh : MonoBehaviour
{
    private EnemyStateMachine enemyBrain;

    [SerializeField] private NavMeshAgent navMeshAgent;
    
    

    [SerializeField] private Animator animator;

    [SerializeField] private Transform[] waypoints;


    
    private void Awake()
    {
        enemyBrain = GetComponent<EnemyStateMachine>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (enemyBrain.PlayerInSight(out Transform player))
        {
            //if (enemyBrain.PlayerInAttackDistance(player.position) == false)
                
            //Chasing();
        }
        else
        {
            

            //Patroling();
        }

    }
}