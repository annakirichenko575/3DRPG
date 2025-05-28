using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private GameObject _pauseCanvas;
    void Start()
    {
        Time.timeScale = 1f;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (_pauseCanvas.activeSelf == false) {
                _pauseCanvas.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
}
