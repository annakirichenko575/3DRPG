using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using Infrastructure.Services;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMainMenu : MonoBehaviour
{
    public void LoadMenu(){
        SceneManager.LoadScene("mainMenu");
    }
}
