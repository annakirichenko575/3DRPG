using UnityEngine;
using UnityEngine.UI;
using Infrastructure.Services;
using SaveSystem;

namespace Player
{
    public class ManaBar : MonoBehaviour
    {
        [SerializeField] private Image manaBar;
        [SerializeField] private ManaPoints manaPoints;

        private PlayerRepository playerRepository;

        public void Initialize(PlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
            playerRepository.OnManaChanged += UpdateMana;
            UpdateMana();
        }

        public void UpdateMana()
        {
            manaBar.fillAmount = (float)playerRepository.Mana / PlayerRepository.MaxMana;
        }

        private void OnDestroy()
        {
            if (playerRepository != null)
                playerRepository.OnManaChanged -= UpdateMana;
        }
    }
}
