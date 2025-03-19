using UnityEngine;
using System.Collections;

public class AnimationPresenter : MonoBehaviour
{
    private const string SadIdleState = "SadIdle";
    private const string WalkState = "Walking";
    private const string RunState = "Run";
    private const string PhysicAttackState = "PhysicAttack"; // Новое состояние атаки
    private const string ToSadIdleName = "ToSadIdle";
    private const string ToWalkName = "ToWalk";
    private const string ToRunName = "ToRun";
    private const string ToPhysicAttackName = "ToPhysicAttack"; // Новый триггер для атаки

    [SerializeField] private Movement movement;

    private Animator animator;
    private bool IsIdle, IsWalk, IsRun, IsPhysicAttack;

    private void Awake()
    {
        // Получаем компонент Animator
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (IsPhysicAttack) // Если атака активна, игнорируем другие анимации
            return;

        if (movement.IsIdle())
        {
            PlaySadIdle(); // Включаем анимацию Sad Idle
        }
        else
        {
            if (movement.IsRun())
            {
                PlayRun(); // Включаем анимацию Run
            }
            else
            {
                PlayWalk(); // Включаем анимацию Walk
            }
        }

        // Проверка на нажатие левой кнопки мыши
        if (Input.GetMouseButtonDown(0)) // Левая кнопка мыши
        {
            PlayPhysicAttack(); // Включаем анимацию атаки
        }
    }

    // Метод для переключения на анимацию "Sad Idle"
    public void PlaySadIdle()
    {     
        if (IsPlaying(SadIdleState)) // Проверяем, не проигрывается ли уже эта анимация
            return;
        
        ResetAllTriggers(); // Сбрасываем все триггеры
        animator.SetTrigger(ToSadIdleName); // Активируем триггер для Sad Idle
        SetPlaying(SadIdleState);
    }

    // Метод для переключения на анимацию "Walk"
    public void PlayWalk()
    {
        if (IsPlaying(WalkState)) // Проверяем, не проигрывается ли уже эта анимация
            return;

        ResetAllTriggers(); // Сбрасываем все триггеры
        animator.SetTrigger(ToWalkName); // Активируем триггер для Walk
        SetPlaying(WalkState);
    }

    // Метод для переключения на анимацию "Run"
    public void PlayRun()
    {
        if (IsPlaying(RunState)) // Проверяем, не проигрывается ли уже эта анимация
            return;

        ResetAllTriggers(); // Сбрасываем все триггеры
        animator.SetTrigger(ToRunName); // Активируем триггер для Run
        SetPlaying(RunState);
    }

    // Метод для переключения на анимацию "Physic Attack"
    public void PlayPhysicAttack()
    {
        if (IsPlaying(PhysicAttackState)) // Проверяем, не проигрывается ли уже эта анимация
            return;

        ResetAllTriggers(); // Сбрасываем все триггеры
        animator.SetTrigger(ToPhysicAttackName); // Активируем триггер для Physic Attack
        SetPlaying(PhysicAttackState);

        // Запускаем корутину для завершения атаки
        StartCoroutine(CompletePhysicAttack());
    }

    // Метод для сброса всех триггеров
    private void ResetAllTriggers()
    {
        foreach (var trigger in animator.parameters)
        {
            if (trigger.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(trigger.name); // Сбрасываем каждый триггер
            }
        }
    }

    private bool IsPlaying(string stateName)
    {
        switch (stateName)
        {
            case SadIdleState:
                return IsIdle;
            case WalkState:
                return IsWalk;
            case RunState:
                return IsRun;
            case PhysicAttackState:
                return IsPhysicAttack; // Проверка для атаки
        }
        return false;
    }

    private void SetPlaying(string stateName)
    {
        IsIdle = false;
        IsWalk = false;
        IsRun = false;
        IsPhysicAttack = false; // Сбрасываем флаг атаки
        switch (stateName)
        {
            case SadIdleState:
                IsIdle = true;
                break;
            case WalkState:
                IsWalk = true;
                break;
            case RunState:
                IsRun = true;
                break;
            case PhysicAttackState:
                IsPhysicAttack = true; // Устанавливаем флаг атаки
                break;
        }
    }

    // Корутина для завершения атаки
    private IEnumerator CompletePhysicAttack()
    {
        // Ждем завершения анимации атаки (время можно настроить)
        yield return new WaitForSeconds(2f); // Пример: 1 секунда на анимацию атаки

        IsPhysicAttack = false; // Сбрасываем флаг атаки
    }
}
