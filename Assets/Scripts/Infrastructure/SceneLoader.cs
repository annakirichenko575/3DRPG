using Infrastructure.Services;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class SceneLoader : IService
    {
        private const string Level1 = "DemoCharacter";

        private static bool isFirstStart = true;

        public bool IsFirstStart => isFirstStart; 

        public void Restart()
        {
            isFirstStart = true;
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }

        public void LoadSavedScene()
        {
            isFirstStart = false;
            SceneManager.LoadScene(Level1);
        }
    }
}
