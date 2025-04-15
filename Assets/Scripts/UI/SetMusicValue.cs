using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetMusicValue : MonoBehaviour
{
    [SerializeField] Slider _musicSliderUI;
    void Update()
    {
        MusicSlider.SetVariable(_musicSliderUI.value);
    }
}
