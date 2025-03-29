using UnityEngine;
using Enemy;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class MagicBullet : MonoBehaviour
    {
        [SerializeField] private int damage = 30;
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
            if (other.TryGetComponent(out HealthPoints healthPoints))
            {
                healthPoints.Hit(damage);
                Destroy(gameObject);
            }
            else if (!other.CompareTag("Player")) // „тобы пул€ не исчезала при столкновении с игроком
            {
                Destroy(gameObject);
            }
        }
    }
}