using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float walk = 3f;
    [SerializeField] private float run = 5f;

    [SerializeField] private Transform dwarf;

    private Vector3 inputDirection = Vector3.zero;
    private Rigidbody rigidbody;
    private float speed;

    private void Awake()
    {
        speed = walk;
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        Vector3 position = rigidbody.position + inputDirection * speed * Time.fixedDeltaTime;
        rigidbody.MovePosition(position);

        // ������� ������� Dwarf � ����������� �� ����������� ��������
        if (inputDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
            dwarf.rotation = Quaternion.Slerp(dwarf.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }
    }

    public bool IsIdle()
    {
        if (inputDirection.sqrMagnitude >= 0.1f)
        {
            Debug.Log(inputDirection.sqrMagnitude);
        }
        return inputDirection.sqrMagnitude < 0.1f;
    }

    public bool IsRun()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }

    private void PlayerInput()
    {
        speed = walk;
        if (IsRun())
        {
            speed = run; //y���������� �������� ��� ������� Shift
        }
        float horizontal = Input.GetAxis("Horizontal"); //� ���������� �������� ������ �� �������� �� 1 �� -1
        float vertical = Input.GetAxis("Vertical"); //� ���������� �������� ������ �� ��������� �� 1 �� -1
        inputDirection = new Vector3(horizontal, 0f, vertical); //���������� ������ ����������� �������� ��������
        inputDirection = transform.forward * inputDirection.z + transform.right * inputDirection.x;
    }
}
