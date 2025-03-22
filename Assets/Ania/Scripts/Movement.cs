using UnityEngine;

[RequireComponent(typeof(PlayerInput), typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float walk = 3f;
    [SerializeField] private float run = 5f;
    [SerializeField] private Transform dwarf;

    private Vector3 inputDirection = Vector3.zero;
    private Rigidbody rigidbody;
    private float speed;
    private PlayerInput playerInput;

    private void Awake()
    {
        speed = walk;
        rigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        speed = walk;
        if (playerInput.IsRun())
        {
            speed = run; 
        }
        //Debug.Log(playerInput.MoveInput);
        inputDirection = transform.forward * playerInput.MoveInput.z + transform.right * inputDirection.x;
        //Debug.Log(inputDirection);
        Debug.Log(transform.forward);
    }

    private void FixedUpdate()
    {
        MovePosition();
        ModelRotation();
    }

    private void MovePosition()
    {
        Vector3 position = rigidbody.position + inputDirection * speed * Time.fixedDeltaTime;
        rigidbody.MovePosition(position);
    }

    private void ModelRotation()
    {
        if (playerInput.IsIdle() == false)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
            dwarf.rotation = Quaternion.Slerp(dwarf.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }
    }
}
