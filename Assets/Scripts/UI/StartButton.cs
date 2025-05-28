using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using Infrastructure.Services;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartButton : MonoBehaviour
{
    public void GameStart(){
        SceneManager.LoadScene("DemoCharacter");
    }
}
