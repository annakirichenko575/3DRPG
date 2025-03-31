using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;
using System;

namespace Enemy
{
    public class HealthPoints : MonoBehaviour
    {
        [SerializeField] public static int maxHealth = 100;
        [SerializeField] private float hitInvincibilityTime = 0.5f;
        [SerializeField] private Animator enemyAnimator;

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

        public void Heal(int heal)
        {
            if (isDeath)
                return;

            health += heal;
            HealthClamp();
            OnHeal?.Invoke();
            Debug.Log("EnemyGetHeal");
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
                OnDie?.Invoke();
                StartCoroutine(DieAnimationCoroutine());
                Debug.Log("EnemyDeath");
            }
            else
            {
                StartCoroutine(InvincibilityRoutine());
                OnHit?.Invoke();
                Debug.Log("EnemyGetHit");
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

        private IEnumerator DieAnimationCoroutine()
        {
            if (enemyAnimator != null)
            {
                enemyAnimator.SetBool("isDead", true); 
            }

            yield return new WaitForSeconds(3f);

            Destroy(gameObject);
        }
    }
}