using UnityEngine;
using Enemy;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class MagicBullet : MonoBehaviour
    {
        [SerializeField] private int damage = 50;
        [SerializeField] private float lifetime = 3f;

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            Destroy(gameObject, lifetime);
        }

        public void Initialize(Vector3 direction, float speed)
        {
            rb.velocity = direction * speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.gameObject.name);
            Enemy.HealthPoints healthPoints = GetEnemyHealthPoints(other);

            if (healthPoints != null)
            {
                healthPoints.Hit(damage);
                Destroy(gameObject);
            }
            
            /*if (!other.CompareTag("Player")) 
            {
            }*/
        }

        private Enemy.HealthPoints GetEnemyHealthPoints(Collider other)
        {
            Enemy.HealthPoints healthPoints = other.GetComponent<Enemy.HealthPoints>();
            if (healthPoints == null)
            {
                healthPoints = other.GetComponentInParent<Enemy.HealthPoints>();
            }

            return healthPoints;
        }
    }
}