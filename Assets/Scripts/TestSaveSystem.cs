using Infrastructure.Services;
using SaveSystem;
using UnityEngine;

public class TestSaveSystem : MonoBehaviour
{
    private SaverService saverService;

    private void Start()
    {
        saverService = AllServices.Container.Single<SaverService>();
        FindObjectOfType<Player.Movement>().transform.position = saverService.PlayerPosition;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            saverService.SetPlayerPosition(FindObjectOfType<Player.Movement>().transform.position);
            saverService.SaveData();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            saverService.Load();
            FindObjectOfType<Player.Movement>().transform.position = saverService.PlayerPosition;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            PlayerPrefs.DeleteAll();
        }

        Debug.LogWarning(saverService.PlayerPosition);
    }

}
