using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueGame : MonoBehaviour
{
    [SerializeField] private GameObject _pauseCanvas;

    public void ResumeGame(){
        _pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
    }
}
