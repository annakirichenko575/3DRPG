using UnityEngine;

namespace Enemy.StateMachine
{
    public class EnemyPerceptionBoss
    {
        private Transform enemyTransform;
        private Transform target;
        private float viewRadius;
        private float viewAngle;
        private LayerMask playerMask;
        private LayerMask obstacleMask;

        public void Initialize(Transform enemy, Transform player,
            float viewRadius = 20f, float viewAngle = 120f,
            LayerMask playerMask = default, LayerMask obstacleMask = default)
        {
            enemyTransform = enemy;
            target = player;
            this.viewRadius = viewRadius;
            this.viewAngle = viewAngle;
            this.playerMask = playerMask;
            this.obstacleMask = obstacleMask;
        }

        public bool PlayerInSight(out Transform player)
        {
            Collider[] hits = Physics.OverlapSphere(enemyTransform.position, viewRadius, playerMask);

            foreach (var hit in hits)
            {
                player = hit.transform;
                Vector3 dirToPlayer = (player.position - enemyTransform.position).normalized;
                if (Vector3.Angle(enemyTransform.forward, dirToPlayer) < viewAngle / 2f)
                {
                    if (!Physics.Raycast(enemyTransform.position, dirToPlayer, Vector3.Distance(enemyTransform.position, player.position), obstacleMask))
                    {
                        return true;
                    }
                }
            }

            player = null;
            return false;
        }
    }
}
