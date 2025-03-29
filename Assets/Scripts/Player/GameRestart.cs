using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestart : MonoBehaviour
{
    [SerializeField] HealthPoints _healthPoints;
    [SerializeField] GameObject _restartScreen;
    [SerializeField] GameObject _playerUI;

    private Scene _currentScene;

    void Start()
    {
        _restartScreen.SetActive(false);
        _playerUI.SetActive(true);
        _currentScene = SceneManager.GetActiveScene();
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

    public void RestartGame(){
        SceneManager.LoadScene(_currentScene.name);
    }
}
