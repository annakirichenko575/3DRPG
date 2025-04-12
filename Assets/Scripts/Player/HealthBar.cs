using Infrastructure.Services;
using SaveSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class HealthBar : MonoBehaviour //View
    {
        [SerializeField] private Image healthBar;
        [SerializeField] private HealthPoints healthPoints;

        private PlayerRepository playerRepository;

        public void Initialize(PlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
            HealthChanged();
        }

        public void HealthChanged()
        {
            healthBar.fillAmount = (float)playerRepository.Health / PlayerRepository.MaxHealth;
        }
    }
}