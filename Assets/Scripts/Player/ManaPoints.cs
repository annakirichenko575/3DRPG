using UnityEngine;
using System;
using SaveSystem;
using UnityEngine.Events;

namespace Player
{
    public class ManaPoints : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;

        private PlayerRepository playerRepository;

        public event UnityAction OnWiz;
        public event UnityAction OnRecovered;

        public void Initialize(PlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        private void Update()
        {
            if (playerInput.IsMagicAttack && playerRepository.Mana > 0)
            {
                Wiz(10f);
                OnWiz?.Invoke();
            }
        }

        private void Wiz(float value)
        {
            playerRepository.SetPlayerMana(playerRepository.Mana - value);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("mana"))
            {
                Recover(40f);
                Destroy(other.gameObject);
            }
        }

        private void Recover(float value)
        {
            playerRepository.SetPlayerMana(playerRepository.Mana + value);
            OnRecovered?.Invoke();
        }
    }
}