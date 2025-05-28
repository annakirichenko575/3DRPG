using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Levitation : MonoBehaviour
{
    private GameObject _potion;
    private Vector3 _upperPosition;
    private Vector3 _lowerPosition;
    private Vector3 _delta = new Vector3(0f, 0.002f, 0f);

    private bool isFlyingUp;

    void Start()
    {   
        _potion = this.gameObject;
        _upperPosition = new Vector3(_potion.transform.position.x,_potion.transform.position.y + 0.15f, _potion.transform.position.z);
        _lowerPosition = new Vector3(_potion.transform.position.x,_potion.transform.position.y - 0.05f, _potion.transform.position.z);
        if (this.tag == "mana")
            isFlyingUp = false;
        else    
            isFlyingUp = true;
    }

    void Update()
    {
        SetToUp();
        SetToDown();
        Fly();
    }

    private void SetToUp(){
        if (_potion.transform.position == _lowerPosition)
            isFlyingUp = true;
    }

    private void SetToDown(){
        if (_potion.transform.position == _upperPosition)
            isFlyingUp = false;
    }

    private void Fly(){
        if (isFlyingUp == false && _potion.transform.position != _lowerPosition)
            _potion.transform.position -= _delta;
        if (isFlyingUp == true && _potion.transform.position != _upperPosition)
            _potion.transform.position += _delta;
    }
}
