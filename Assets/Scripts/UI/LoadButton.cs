using Infrastructure;
using Infrastructure.Services;
using UnityEngine;

public class LoadButton : MonoBehaviour
{
    public void Load() 
    {
        AllServices.Container.Single<SceneLoader>().LoadSavedScene();
    }
}
