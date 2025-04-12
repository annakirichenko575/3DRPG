using Infrastructure.Services;
using Player;
using SaveSystem;
using UnityEngine;

namespace Infrastructure
{
    public class PlayerFactory : IService //Controller
    {
        private HealthBar healthBar;
        private Movement movement;
        private HealthPoints healthPoints;
        private PlayerRepository playerRepository;
        private SceneLoader sceneLoader;

        public Movement Movement => movement;

        public PlayerFactory(PlayerRepository playerRepository, SceneLoader sceneLoader)
        {
            this.playerRepository = playerRepository;
            this.sceneLoader = sceneLoader;

            PlayerInitialize();
            UIInitialize(playerRepository);

            healthPoints.OnHit += healthBar.HealthChanged;
            healthPoints.OnHeal += healthBar.HealthChanged;
            healthPoints.OnDie += healthBar.HealthChanged;
        }

        private void UIInitialize(PlayerRepository playerRepository)
        {
            healthBar = GameObject.FindObjectOfType<Player.HealthBar>();
            healthBar.Initialize(playerRepository);
        }

        private void PlayerInitialize()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            movement = player.GetComponent<Movement>();

            if (sceneLoader.IsFirstStart)
            {
                playerRepository.SetPlayerPosition(movement.transform.position);
            }
            movement.Initialize(playerRepository.Position);

            healthPoints = player.GetComponent<HealthPoints>();
            healthPoints.Initialize(playerRepository);
        }
    }
}
