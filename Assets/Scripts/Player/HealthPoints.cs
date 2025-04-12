using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;
using System;
using SaveSystem;

namespace Player
{
    public class HealthPoints : MonoBehaviour //Model
    {
        [SerializeField] private float hitInvincibilityTime = 2f;

        private bool isDeath;
        private bool isInvincible;
        private PlayerRepository playerRepository;

        public event UnityAction OnHeal;
        public event UnityAction OnHit;
        public event UnityAction OnDie;

        public bool IsDeath => isDeath;
        public bool IsInvincible => isInvincible;

        public void Initialize(PlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        public void Heal(int heal)
        {
            if (isDeath)
                return;

            playerRepository.SetPlayerHealth(playerRepository.Health + heal);
            HealthClamp();
            OnHeal.Invoke();
        }

        public void Hit(int damage)
        {
            if (isDeath || isInvincible)
            {
                return;
            }

            playerRepository.SetPlayerHealth(playerRepository.Health - damage);

            HealthClamp();

            if (playerRepository.Health == 0)
            {
                isDeath = true;
                OnDie?.Invoke();
            }
            else
            {
                StartCoroutine(InvincibilityRoutine());
                OnHit?.Invoke();
            }
        }


        private void HealthClamp()
        {
            playerRepository.SetPlayerHealth(Math.Clamp(playerRepository.Health, 0, PlayerRepository.MaxHealth));
        }

        private IEnumerator InvincibilityRoutine()
        {
            isInvincible = true;
            yield return new WaitForSeconds(hitInvincibilityTime);
            isInvincible = false;
        }
    }
}