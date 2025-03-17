using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPresenter : MonoBehaviour
{
    [SerializeField] private Movement movement;

    private Animator animator;

    private void Awake()
    {
        // Получаем компонент Animator
        animator = GetComponent<Animator>();
    }

    private string currentState = "Idle"; // Текущее состояние анимации

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
        if (IsPlaying("SadIdle")) // Проверяем, не проигрывается ли уже эта анимация
            return;

        Debug.Log("ToSadIdle");

        ResetAllTriggers(); // Сбрасываем все триггеры
        animator.SetTrigger("ToSadIdle"); // Активируем триггер для Sad Idle
    }

    // Метод для переключения на анимацию "Walk"
    public void PlayWalk()
    {
        if (IsPlaying("Walking")) // Проверяем, не проигрывается ли уже эта анимация
            return;

        ResetAllTriggers(); // Сбрасываем все триггеры
        animator.SetTrigger("ToWalk"); // Активируем триггер для Walk
    }

    // Метод для переключения на анимацию "Run"
    public void PlayRun()
    {
        if (IsPlaying("Run")) // Проверяем, не проигрывается ли уже эта анимация
            return;

        ResetAllTriggers(); // Сбрасываем все триггеры
        animator.SetTrigger("ToRun"); // Активируем триггер для Run
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

    // Метод для проверки, проигрывается ли указанная анимация
    private bool IsPlaying(string stateName)
    {
        // Получаем информацию о текущем состоянии аниматора
        AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Проверяем, совпадает ли имя состояния и проигрывается ли анимация
        return currentStateInfo.IsName(stateName);
    }
}
