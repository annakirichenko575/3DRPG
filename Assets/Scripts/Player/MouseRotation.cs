using UnityEngine;

namespace Player
{
    public class MouseRotation : MonoBehaviour
    {
        [SerializeField] private float mouseSensitivi = 100f;
        [SerializeField] private Transform playerBody; 
        [SerializeField] private float xRotation = 0f;
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private HealthPoints healthPoints;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked; 
        }

        private void Update()
        {
            if (healthPoints.IsDeath)
            {
                xRotation = 47f;
                XRotate(xRotation);
                return;
            }

            float mouseX = playerInput.MouseInput.x * mouseSensitivi * Time.deltaTime;
            float mouseY = playerInput.MouseInput.y * mouseSensitivi * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);

            YRotate(mouseX);
            XRotate(xRotation);
        }

        private void YRotate(float Yaw)
        {
            playerBody.Rotate(Vector3.up * Yaw);
        }

        private void XRotate(float pitch)
        {
            transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
    }
}