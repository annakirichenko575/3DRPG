using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] GameObject _startBtn;
    [SerializeField] GameObject _settingsBtn;
    [SerializeField] GameObject _musicText;
    [SerializeField] GameObject _musicSlider;
    [SerializeField] GameObject _backButton;

    public void ChangeUI(){
        _startBtn.SetActive(false);
        _settingsBtn.SetActive(false);
        _musicSlider.SetActive(true);
        _musicText.SetActive(true);
        _backButton.SetActive(true);
    }
}
