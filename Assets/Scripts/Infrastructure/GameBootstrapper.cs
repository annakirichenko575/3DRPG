using Infrastructure.Services;
using SaveSystem;
using UnityEngine;

namespace Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        private void Awake()
        {
            SaverService saver = new SaverService();
            AllServices.Container.RegisterSingle<SaverService>(saver);
            saver.Load();

            DontDestroyOnLoad(this);
        }
    }
}
