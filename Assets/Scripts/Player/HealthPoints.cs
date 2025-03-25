using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Player
{
    public class HealthPoints : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private float hitInvincibilityTime = 2f;

        private int health;
        private bool isDeath;
        private bool isInvincible;


        public event UnityAction OnHit;
        public event UnityAction OnDie;

        public bool IsDeath => isDeath;
        public bool IsInvincible => isInvincible;


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
            if (isDeath || isInvincible)
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
                StartCoroutine(InvincibilityRoutine());
                OnHit.Invoke();
            }
        }

        private IEnumerator InvincibilityRoutine()
        {
            isInvincible = true;
            yield return new WaitForSeconds(hitInvincibilityTime);
            isInvincible = false;
        }
    }
}