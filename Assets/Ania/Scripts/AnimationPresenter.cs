using UnityEngine;

public class AnimationPresenter : MonoBehaviour
{
    private const string SadIdleState = "SadIdle";
    private const string WalkState = "Walking";
    private const string RunState = "Run";
    private const string ToSadIdleName = "ToSadIdle";
    private const string ToWalkName = "ToWalk";
    private const string ToRunName = "ToRun";

    [SerializeField] private Movement movement;

    private Animator animator;
    private bool IsIdle, IsWalk, IsRun;

    private void Awake()
    {
        // Получаем компонент Animator
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
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
        }
        return false;
    }

    private void SetPlaying(string stateName)
    {
        IsIdle = false;
        IsWalk = false;
        IsRun = false;
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
        }
    }
}
