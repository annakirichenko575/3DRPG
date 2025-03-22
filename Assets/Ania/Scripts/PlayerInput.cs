using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Vector3 moveInput;

    public Vector3 MoveInput => moveInput;

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        moveInput = new Vector3(horizontal, 0f, vertical);
    }

    public bool IsIdle()
    {
        return moveInput.sqrMagnitude < 0.1f;
    }

    public bool IsRun()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }

    public void Stop()
    {
        moveInput = Vector3.zero;
        
    }
}
