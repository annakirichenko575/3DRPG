using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        private Vector3 moveInput;
        private bool canMove;

        public Vector3 MoveInput => moveInput;

        private void Update()
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