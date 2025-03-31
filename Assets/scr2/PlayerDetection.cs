using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    private AIController aiController;

    void Start()
    {
        aiController = GetComponent<AIController>();
    }

    void Update()
    {
        DetectPlayer();
    }

    void DetectPlayer()
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
                    aiController.SetPlayerInRange(true);
                }
                else
                {
                    aiController.SetPlayerInRange(false);
                }
            }
        }
    }
}
