using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotation : MonoBehaviour
{
    public float mouseSensitivi = 100f;
    public Transform playerBody; //поворот тела перса с камерой
    
    private float xRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //при запуске курсор исчезает, при нажатии esc появляется
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivi * Time.deltaTime; //ввод по оси X
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivi * Time.deltaTime; //ввод по оси Y

        xRotation -= mouseY; //уменьшаем угол вращения в зависимости от движения мыши
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); //чтобы не вращаться вечно, максимум на 90 градусов

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); //применяем вращение к камере
        playerBody.Rotate(Vector3.up * mouseX); //можем двигаться бесконечно, применяем к персу
    }
}
