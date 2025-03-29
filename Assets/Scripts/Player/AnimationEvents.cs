using UnityEngine;

namespace Player
{
    public class AnimationEvents : MonoBehaviour
    {
        [SerializeField] private AttackZone attackZone;
        [SerializeField] private GameObject magicBulletPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float bulletSpeed = 20f;
        [SerializeField] private LayerMask enemyMask;

        private void PhysicAttack()
        {
            attackZone.Attack();
        }

        private void MagicAttack()
        {
            // ������� ���������� �����
            GameObject nearestEnemy = FindNearestEnemy();

            if (nearestEnemy != null && magicBulletPrefab != null && firePoint != null)
            {
                // ������� ����
                GameObject bullet = Instantiate(magicBulletPrefab, firePoint.position, firePoint.rotation);
                MagicBullet magicBullet = bullet.GetComponent<MagicBullet>();

                if (magicBullet != null)
                {
                    // ���������� ���� � �����
                    Vector3 direction = (nearestEnemy.transform.position - firePoint.position).normalized;
                    magicBullet.Initialize(direction, bulletSpeed);
                }
                else
                {
                    Debug.LogError("MagicBullet component is missing on the magic bullet prefab.");
                }
            }
        }

        private GameObject FindNearestEnemy()
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, 25f, enemyMask);
            GameObject nearestEnemy = null;
            float minDistance = Mathf.Infinity;

            foreach (Collider enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = enemy.gameObject;
                }
            }

            return nearestEnemy;
        }
    }
}