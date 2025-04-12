using Infrastructure.Services;
using SaveSystem;
using UnityEngine;

namespace Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {

        private void Awake()
        {
            SceneLoader sceneLoader = new SceneLoader();
            AllServices.Container.RegisterSingle<SceneLoader>(sceneLoader);

            PlayerRepository playerRepository = new PlayerRepository();
            AllServices.Container.RegisterSingle<PlayerRepository>(playerRepository);
            if (sceneLoader.IsFirstStart == false)
            {
                playerRepository.LoadData();
            }

            PlayerFactory playerFactory = new PlayerFactory(playerRepository, sceneLoader);
            AllServices.Container.RegisterSingle<PlayerFactory>(playerFactory);


            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            /*if (Input.GetKeyDown(KeyCode.C))
            {
                saverService.SetPlayerPosition(FindObjectOfType<Player.Movement>().transform.position);
                saverService.SaveData();
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                saverService.LoadData();
                FindObjectOfType<Player.Movement>().transform.position = saverService.Position;
            }*/
            if (Input.GetKeyDown(KeyCode.X))
            {
                PlayerPrefs.DeleteAll();
            }
        }
    }
}
