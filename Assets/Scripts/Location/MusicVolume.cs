using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVolume : MonoBehaviour
{
    [SerializeField] AudioSource _gameMusic;

    void Awake()
    {
        _gameMusic.volume = MusicSlider.GetValue();
    }
}
