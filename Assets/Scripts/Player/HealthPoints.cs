using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;
using System;

namespace Player
{
    public class HealthPoints : MonoBehaviour
    {
        [SerializeField] public static int maxHealth = 100;
        [SerializeField] private float hitInvincibilityTime = 0.5f;

        private int health;
        private bool isDeath;
        private bool isInvincible;


        public event UnityAction OnHeal;
        public event UnityAction OnHit;
        public event UnityAction OnDie;

        public bool IsDeath => isDeath;
        public bool IsInvincible => isInvincible;
        public int Health => health;
        public int MaxHealth =>maxHealth;

        private void Start()
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

        public void Heal(int heal)
        {
            if (isDeath)
                return;

            health += heal;
            HealthClamp();
            OnHeal.Invoke();
        }

        public void Hit(int damage)
        {
            if (isDeath || isInvincible)
                return;

            health -= damage;
            HealthClamp();
            
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

        private void HealthClamp()
        {
            health = Math.Clamp(health, 0, maxHealth);
        }

        private IEnumerator InvincibilityRoutine()
        {
            isInvincible = true;
            yield return new WaitForSeconds(hitInvincibilityTime);
            isInvincible = false;
        }
    }
}