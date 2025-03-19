using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float walk = 3f;
    [SerializeField] private float run = 5f;
    [SerializeField] private Transform dwarf;

    private Vector3 inputDirection = Vector3.zero;
    private Rigidbody rigidbody;
    private float speed;
    private bool isAttacking = false; // Флаг для атаки

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

        // Поворот объекта Dwarf в зависимости от направления движения
        if (inputDirection.sqrMagnitude != 0f) //если есть направление движения
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection); //целевой поворот
            dwarf.rotation = Quaternion.Slerp(dwarf.rotation, targetRotation, Time.fixedDeltaTime * 10f); //плавный поворот
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

    public bool IsAttacking() // Метод для проверки атаки
    {
        return isAttacking;
    }

    private void StartAttack()
    {
        isAttacking = true;
        inputDirection = Vector3.zero; // Останавливаем движение
    }

    private void CompleteAttack()
    {
        isAttacking = false; // Завершаем атаку
    }

    private void PlayerInput()
    {
        speed = walk;
        if (IsRun())
        {
            speed = run; //yвеличиваем скорость при нажатии Shift
        }
        float horizontal = Input.GetAxis("Horizontal"); //с клавиатуры получаем направ по горизонт от 1 до -1
        float vertical = Input.GetAxis("Vertical"); //с клавиатуры получаем направ по вертикали от 1 до -1
        inputDirection = new Vector3(horizontal, 0f, vertical); //определяем вектор направления будущего движения
        inputDirection = transform.forward * inputDirection.z + transform.right * inputDirection.x;
    }
}
