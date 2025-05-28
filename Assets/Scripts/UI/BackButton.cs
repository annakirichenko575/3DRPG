using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    [SerializeField] GameObject _startBtn;
    [SerializeField] GameObject _settingsBtn;
    [SerializeField] GameObject _musicText;
    [SerializeField] GameObject _musicSlider;
    [SerializeField] GameObject _backButton;

    public void ChangeUI(){
        _startBtn.SetActive(true);
        _settingsBtn.SetActive(true);
        _musicSlider.SetActive(false);
        _musicText.SetActive(false);
        _backButton.SetActive(false);
    }
}
