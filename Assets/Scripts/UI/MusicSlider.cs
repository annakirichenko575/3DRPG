using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class MusicSlider 
{    
    private static float _sliderValue;

    public static float GetValue() => _sliderValue;  
    public static void SetVariable(float value) => _sliderValue = value;
}
