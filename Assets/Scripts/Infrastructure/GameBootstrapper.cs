using Enemy;
using Infrastructure.Services;
using SaveSystem;
using UnityEngine;

namespace Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {

        private void Awake()
        {
            Registrate();
            LevelInitialize();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                PlayerPrefs.DeleteAll();
            }
        }

        private void Registrate()
        {
            EnemyDeathCounter deathCounter = new EnemyDeathCounter();
            AllServices.Container.RegisterSingle<EnemyDeathCounter>(deathCounter);

            PlayerRepository playerRepository = new PlayerRepository();
            AllServices.Container.RegisterSingle<PlayerRepository>(playerRepository);

            PlayerFactory playerFactory = new PlayerFactory(playerRepository);
            AllServices.Container.RegisterSingle<PlayerFactory>(playerFactory);

            SceneLoader sceneLoader = new SceneLoader(playerFactory);
            AllServices.Container.RegisterSingle<SceneLoader>(sceneLoader);


        }

        private void LevelInitialize()
        {
            bool isFirstStart = AllServices.Container.Single<SceneLoader>().IsFirstStart;
            if (isFirstStart == false)
            {
                AllServices.Container.Single<PlayerRepository>().LoadData();
            }

            PlayerFactory playerFactory = AllServices.Container.Single<PlayerFactory>();
            playerFactory.Initialize(isFirstStart);

            MonsterSpawner[] spawners = FindObjectsOfType<MonsterSpawner>();
            for (int i = 0; i < spawners.Length; i++)
            {
                spawners[i].Construct(playerFactory, AllServices.Container.Single<EnemyDeathCounter>());
            }
        }

    }
}
