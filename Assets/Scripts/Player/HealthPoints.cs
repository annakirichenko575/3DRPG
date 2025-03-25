using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class HealthPoints : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;

        private int health;
        private bool isDeath;


        public event UnityAction OnHit;
        public event UnityAction OnDie;

        public bool IsDeath => isDeath;


        private void Awake()
        {
            health = maxHealth;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Hit(20);
            }
        }

        public void Hit(int damage)
        {
            if (isDeath)
                return;

            health -= damage;
            health = Mathf.Clamp(health, 0, maxHealth);
            if (health == 0)
            {
                isDeath = true;
                OnDie.Invoke();
            }
            else
            {
                //тут вызов корутины хита
                OnHit.Invoke();
            }
        }
    }
}