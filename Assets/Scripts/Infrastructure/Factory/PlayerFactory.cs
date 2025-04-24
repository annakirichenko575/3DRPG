using Infrastructure.Services;
using Player;
using SaveSystem;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Infrastructure
{
    public class PlayerFactory : IService //Controller
    {
        private HealthBar healthBar;
        private Movement movement;
        private HealthPoints healthPoints;
        private PlayerRepository playerRepository;
        private SceneLoader sceneLoader;
        private ManaBar manaBar;
        private ManaPoints manaPoints;

        public Movement Movement => movement;
        public Transform Player  => movement.transform;

        public PlayerFactory(PlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        public void Initialize(bool isFirstStart)
        {
            PlayerInitialize(isFirstStart);
            UIInitialize(playerRepository);

            healthPoints.OnHit += healthBar.HealthChanged;
            healthPoints.OnHeal += healthBar.HealthChanged;
            healthPoints.OnDie += healthBar.HealthChanged;

            manaPoints.OnWiz += manaBar.UpdateMana;
            manaPoints.OnRecovered += manaBar.UpdateMana;
        }

        public void CleanUp()
        {
            healthPoints.OnHit -= healthBar.HealthChanged;
            healthPoints.OnHeal -= healthBar.HealthChanged;
            healthPoints.OnDie -= healthBar.HealthChanged;

            manaPoints.OnWiz -= manaBar.UpdateMana;
            manaPoints.OnRecovered -= manaBar.UpdateMana;
        }

        private void UIInitialize(PlayerRepository playerRepository)
        {
            healthBar = GameObject.FindObjectOfType<Player.HealthBar>();
            healthBar.Initialize(playerRepository);

            manaBar = GameObject.FindObjectOfType<Player.ManaBar>();
            manaBar.Initialize(playerRepository);
        }

        private void PlayerInitialize(bool isFirstStart)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            movement = player.GetComponent<Movement>();

            if (isFirstStart)
            {
                playerRepository.SetPlayerPosition(movement.transform.position);
            }
            movement.Initialize(playerRepository.Position);

            healthPoints = player.GetComponent<HealthPoints>();
            healthPoints.Initialize(playerRepository);

            manaPoints = player.GetComponent<ManaPoints>();
            manaPoints.Initialize(playerRepository);
        }
    }
}
