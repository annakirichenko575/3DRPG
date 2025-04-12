using Infrastructure;
using Infrastructure.Services;
using SaveSystem;
using UnityEngine;

public class SaveButton : MonoBehaviour
{
    public void Save()
    {
        PlayerRepository saverService = AllServices.Container.Single<PlayerRepository>();
        PlayerFactory playerFactory = AllServices.Container.Single<PlayerFactory>();

        saverService.SetPlayerPosition(playerFactory.Movement.transform.position);

        AllServices.Container.Single<PlayerRepository>().SaveData();
    }
}
