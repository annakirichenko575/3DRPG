using UnityEngine;
using UnityEngine.UI;
using Infrastructure.Services;
using SaveSystem;

namespace Player
{
    public class ManaBar : MonoBehaviour
    {
        [SerializeField] private Image manaBar;

        private PlayerRepository playerRepository;

        public void Initialize(PlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
            UpdateMana();
        }

        public void UpdateMana()
        {
            manaBar.fillAmount = (float)playerRepository.Mana / PlayerRepository.MaxMana;
        }
    }
}
