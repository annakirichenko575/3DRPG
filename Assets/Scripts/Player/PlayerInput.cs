using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private float _currentMagicCooldown;
        [SerializeField] private float _maxMagicCooldown;

        private Vector3 moveInput;
        private bool canMove;
        private Vector2 mouseInput = Vector2.zero;
        private bool magicOnCooldown;
        private bool isMagickAttack;

        public Vector2 MouseInput => mouseInput;
        public Vector3 MoveInput => moveInput;
        public float FillAmount => _currentMagicCooldown / _maxMagicCooldown;
        public bool IsMagicAttack => isMagickAttack;


        private void Start()
        {
            magicOnCooldown = false;
            _currentMagicCooldown = 0f;
            _maxMagicCooldown = 2f;
        }

        private void Update()
        {
            UpdateMouseInput();
            UpdateMoveInput();
            DropCooldown();

            if (CheckMagicAttack())
                SetCooldown();
        }

        private void SetCooldown(){
            _currentMagicCooldown = _maxMagicCooldown;
            magicOnCooldown = true;
        }

        private void DropCooldown(){
            if (_currentMagicCooldown <= 0f)
                magicOnCooldown = false;
            else{
                if (_currentMagicCooldown > 0f)
                    _currentMagicCooldown -= Time.deltaTime;
                if (_currentMagicCooldown <= 0f)
                    _currentMagicCooldown = 0f;
            }
                
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
        private bool CheckMagicAttack()
        {
            isMagickAttack = false;
            if (magicOnCooldown == false && ManaSpend.mana >0)
                isMagickAttack = Input.GetMouseButtonDown(1);
            
            return isMagickAttack;
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