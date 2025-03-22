using UnityEngine;
using System.Collections;
using System;

namespace Player
{
    public class AnimationPresenter : MonoBehaviour
    {
        private const string SadIdleState = "SadIdle";
        private const string WalkState = "Walking";
        private const string RunState = "Run";
        private const string PhysicAttackState = "PhysicAttack";
        private const string HitState = "Hit";
        private const string DeathState = "Death";

        private const string ToSadIdleName = "ToSadIdle";
        private const string ToWalkName = "ToWalk";
        private const string ToRunName = "ToRun";
        private const string ToPhysicAttackName = "ToPhysicAttack";
        [SerializeField] private PlayerInput playerInput;

        private Animator animator;
        private bool IsIdle, IsWalk, IsRun, IsPhysicAttack;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (IsPhysicAttack)
                return;

            //вызвать смерть так же как атаку

            if (playerInput.IsIdle())
            {
                Play(SadIdleState);
            }
            else
            {
                if (playerInput.IsRun())
                {
                    Play(RunState);
                }
                else
                {
                    Play(WalkState);
                }
            }

            // Проверка на нажатие левой кнопки мыши
            if (playerInput.IsPhysicAttack()) // Левая кнопка мыши
            {
                Play(PhysicAttackState); // Включаем анимацию атаки
            }

            //считывание пробела для хита
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Play(HitState);
            }

            //считывание x для смерти
            if (Input.GetKeyDown(KeyCode.X))
            {
                Play(DeathState);
            }
        }

        private void PlayDeath()
        {
            //stop move
        }

        public void PlayHit()
        {
            //throw new NotImplementedException();
        }

        private void Play(string state)
        {
            if (IsPlaying(state)) // Проверяем, не проигрывается ли уже эта анимация
                return;

            ResetAllTriggers(); // Сбрасываем все триггеры
            
            switch (state)
            {
                case SadIdleState:
                    PlaySadIdle();
                    break;
                case WalkState:
                    PlayWalk();
                    break;
                case RunState:
                    PlayRun();
                    break;
                case PhysicAttackState:
                    PlayPhysicAttack();
                    break;
            }

            SetPlaying(SadIdleState);
        }

        private void PlaySadIdle()
        {
            animator.SetTrigger(ToSadIdleName); // Активируем триггер для Sad Idle
        }

        private void PlayWalk()
        {
            animator.SetTrigger(ToWalkName);
        }

        private void PlayRun()
        {
            animator.SetTrigger(ToRunName);
        }

        private void PlayPhysicAttack()
        {
            animator.SetTrigger(ToPhysicAttackName);
            StartCoroutine(CompletePhysicAttack());
        }

        private void ResetAllTriggers()
        {
            foreach (var trigger in animator.parameters)
            {
                if (trigger.type == AnimatorControllerParameterType.Trigger)
                {
                    animator.ResetTrigger(trigger.name);
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
                    return IsPhysicAttack;
            }
            return false;
        }

        private void SetPlaying(string stateName)
        {
            IsIdle = false;
            IsWalk = false;
            IsRun = false;
            IsPhysicAttack = false;
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
                    IsPhysicAttack = true;
                    playerInput.StopMove();
                    break;
            }
        }

        private IEnumerator CompletePhysicAttack()
        {
            yield return new WaitForSeconds(2f);
            playerInput.ReleaseMove();
            IsPhysicAttack = false;
        }
    }
}