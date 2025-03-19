using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float walk = 3f;
    [SerializeField] private float run = 5f;
    [SerializeField] private Transform dwarf;

    private Vector3 inputDirection = Vector3.zero;
    private Rigidbody rigidbody;
    private float speed;
    private bool isAttacking = false; // ���� ��� �����

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
        if (inputDirection.sqrMagnitude != 0f) //���� ���� ����������� ��������
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection); //������� �������
            dwarf.rotation = Quaternion.Slerp(dwarf.rotation, targetRotation, Time.fixedDeltaTime * 10f); //������� �������
        }
    }

    public bool IsIdle()
    {
        return inputDirection.sqrMagnitude < 0.1f;
    }

    public bool IsRun()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }

    public bool IsAttacking() // ����� ��� �������� �����
    {
        return isAttacking;
    }

    private void StartAttack()
    {
        isAttacking = true;
        inputDirection = Vector3.zero; // ������������� ��������
    }

    private void CompleteAttack()
    {
        isAttacking = false; // ��������� �����
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
