using UnityEngine;

public class MouseRotation : MonoBehaviour
{
    [SerializeField] private float mouseSensitivi = 100f;
    [SerializeField] private Transform playerBody; //������� ���� ����� � �������
    [SerializeField] private float xRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //��� ������� ������ ��������, ��� ������� esc ����������
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivi * Time.deltaTime; //���� �� ��� X
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivi * Time.deltaTime; //���� �� ��� Y

        xRotation -= mouseY; //��������� ���� �������� � ����������� �� �������� ����
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); //����� �� ��������� �����, �������� �� 90 ��������

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); //��������� �������� � ������
        playerBody.Rotate(Vector3.up * mouseX); //����� ��������� ����������, ��������� � �����
    }
}
