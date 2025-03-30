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
            if (other.TryGetComponent(out Enemy.HealthPoints healthPoints))
            {
                healthPoints.Hit(damage);
            }
            
            if (!other.CompareTag("Player")) 
            {
                Destroy(gameObject);
            }
        }
    }
}