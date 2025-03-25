using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        private Vector3 moveInput;
        private bool canMove;
        private Vector2 mouseInput = Vector2.zero;

        public Vector2 MouseInput => mouseInput;
        public Vector3 MoveInput => moveInput;

        private void Update()
        {
            UpdateMouseInput();
            UpdateMoveInput();
        }

        private void UpdateMouseInput()
        {
            mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        private void UpdateMoveInput()
        {
            if (canMove)
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
                moveInput = new Vector3(horizontal, 0f, vertical);
            }
            else
            {
                moveInput = Vector3.zero;
            }
        }

        public bool IsIdle()
        {
            return moveInput.sqrMagnitude < 0.1f;
        }

        public bool IsRun()
        {
            return Input.GetKey(KeyCode.LeftShift);
        }

        public bool IsPhysicAttack()
        {
            return Input.GetMouseButtonDown(0);
        }

        public bool IsMagicAttack()
        {
            return Input.GetMouseButtonDown(1);
        }

        public void StopMove()
        {
            canMove = false;
        }

        public void ReleaseMove()
        {
            canMove = true;
        }
    }
}