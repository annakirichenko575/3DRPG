using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using Infrastructure.Services;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestart : MonoBehaviour
{
    [SerializeField] HealthPoints _healthPoints;
    [SerializeField] GameObject _restartScreen;
    [SerializeField] GameObject _playerUI;

    void Start()
    {
        _restartScreen.SetActive(false);
        _playerUI.SetActive(true);
    }

    void Update()
    {
        if (_healthPoints.IsDeath){
            _restartScreen.SetActive(true);
            _playerUI.SetActive(false);

            if (Input.GetKeyDown(KeyCode.R))
                RestartGame();
        }
    }

    public void RestartGame()
    {
        AllServices.Container.Single<SceneLoader>().Restart();
    }
}
