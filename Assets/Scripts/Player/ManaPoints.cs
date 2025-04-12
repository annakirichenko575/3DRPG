using UnityEngine;
using System;
using SaveSystem;

namespace Player
{
    public class ManaPoints : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        private PlayerRepository playerRepository;

        public void Initialize(PlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        private void Update()
        {
            if (playerInput.IsMagicAttack && playerRepository.Mana > 0)
            {
                playerRepository.SetPlayerMana(playerRepository.Mana - 10f);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("mana"))
            {
                playerRepository.SetPlayerMana(playerRepository.Mana + 40f);
                Destroy(other.gameObject);
            }
        }

        private void ManaClamp()
        {
            playerRepository.SetPlayerMana(Math.Clamp(playerRepository.Mana, 0, PlayerRepository.MaxMana));
        }
    }
}