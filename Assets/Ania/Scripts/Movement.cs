using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float walk = 1f;
    [SerializeField] private float run = 2f;
    
    private Vector3 movement = Vector3.zero;
    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.LeftShift)) walk = (walk + run);
        //if (Input.GetKeyUp(KeyCode.LeftShift)) walk = (walk - run);
        float horizontal = Input.GetAxis("Horizontal"); //с клавиатуры получаем направ по горизонт от 1 до -1
        float vertical = Input.GetAxis("Vertical"); //с клавиатуры получаем направ по вертикали от 1 до -1
        movement = new Vector3(horizontal, 0f, vertical); //определяем вектор направления будущего движения
    }

    private void FixedUpdate()
    {
        Vector3 position = rigidbody.position + movement * walk * Time.fixedDeltaTime;
        rigidbody.MovePosition(position);
    }

}
