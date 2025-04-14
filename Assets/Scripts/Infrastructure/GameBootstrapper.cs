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

        private static void LevelInitialize()
        {
            bool isFirstStart = AllServices.Container.Single<SceneLoader>().IsFirstStart;
            if (isFirstStart == false)
            {
                AllServices.Container.Single<PlayerRepository>().LoadData();
            }

            AllServices.Container.Single<PlayerFactory>().Initialize(isFirstStart);
        }

        private void Registrate()
        {
            PlayerRepository playerRepository = new PlayerRepository();
            AllServices.Container.RegisterSingle<PlayerRepository>(playerRepository);

            PlayerFactory playerFactory = new PlayerFactory(playerRepository);
            AllServices.Container.RegisterSingle<PlayerFactory>(playerFactory);

            SceneLoader sceneLoader = new SceneLoader(playerFactory);
            AllServices.Container.RegisterSingle<SceneLoader>(sceneLoader);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                PlayerPrefs.DeleteAll();
            }
        }
    }
}
