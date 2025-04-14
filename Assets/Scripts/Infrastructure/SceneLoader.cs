using Infrastructure.Services;
using SaveSystem;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class SceneLoader : IService
    {
        private const string Level1 = "DemoCharacter";

        private static bool isFirstStart = true;
        private PlayerFactory playerFactory;

        public bool IsFirstStart => isFirstStart; 

        public SceneLoader(PlayerFactory playerFactory) 
        {
            this.playerFactory = playerFactory;
        }

        public void Restart()
        {
            isFirstStart = true;
            Scene currentScene = SceneManager.GetActiveScene();
            LoadScene(currentScene.name);
        }

        public void LoadSavedScene()
        {
            isFirstStart = false;
            LoadScene(Level1);

        }

        private void LoadScene(string name)
        {
            playerFactory.CleanUp();
            SceneManager.LoadScene(name);   
        }
    }
}
