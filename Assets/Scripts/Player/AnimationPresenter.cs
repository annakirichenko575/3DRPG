using UnityEngine;
using System.Collections;
using System;

namespace Player
{
    public class AnimationPresenter : MonoBehaviour
    {
        public event Action OnPhysicAttack;
        public event Action OnMagicAttack;

        private const string SadIdleState = "SadIdle";
        private const string WalkState = "Walking";
        private const string RunState = "Run";
        private const string PhysicAttackState = "PhysicAttack";
        private const string MagicAttackState = "MagicAttack";
        private const string HitState = "Hit";
        private const string DeathState = "Death";

        private const string ToSadIdleName = "ToSadIdle";
        private const string ToWalkName = "ToWalk";
        private const string ToRunName = "ToRun";
        private const string ToPhysicAttackName = "ToPhysicAttack";
        private const string ToMagicAttackName = "ToMagicAttack";
        private const string ToHitName = "ToHit";
        private const string ToDeathName = "ToDeath";

        private const float HitDelay = 0.5f;
        private const float PhysicDelay = 2f;
        private const float MagicDelay = 2f;


        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private HealthPoints healthPoints;

        private Animator animator;
        private bool IsIdle, IsWalk, IsRun, IsPhysicAttack, IsMagicAttack, IsHit, IsDeath;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            healthPoints.OnHit += Hit;
            healthPoints.OnDie += Die;
        }

        private void Update()
        {
            if (IsPhysicAttack || IsMagicAttack || IsHit || IsDeath)
                return;

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

            
            if (playerInput.IsPhysicAttack()) 
            {
                Play(PhysicAttackState); 
            }

            
            if (playerInput.IsMagicAttack)
            {
                Play(MagicAttackState);
            }
        }

        private void Hit()
        {
            Play(HitState);
        }

        private void Die()
        {
            Play(DeathState);
        }


        private void Play(string state)
        {
            if (IsPlaying(state)) 
                return;

            ResetAllTriggers(); 
            
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
                case MagicAttackState:
                    PlayMagicAttack();
                    break;
                case HitState:
                    PlayHit();
                    break;
                case DeathState:
                    PlayDeath();
                    break;
            }

            SetPlaying(state);
        }

        private void PlaySadIdle()
        {
            animator.SetTrigger(ToSadIdleName); 
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
            OnPhysicAttack?.Invoke();
            StartCoroutine(CompletePhysicAttack());
        }

        private void PlayMagicAttack()
        {
            animator.SetTrigger(ToMagicAttackName);
            OnMagicAttack?.Invoke();
            StartCoroutine(CompleteMagicAttack());
        }

        public void PlayHit()
        {
            animator.SetTrigger(ToHitName);
            StartCoroutine(CompleteHit());
        }

        private void PlayDeath()
        {
            animator.SetTrigger(ToDeathName);
            playerInput.StopMove();
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
                case MagicAttackState:
                    return IsMagicAttack;
                case HitState:
                    return IsHit;
                case DeathState:
                    return IsDeath;
            }
            return false;
        }

        private void SetPlaying(string stateName)
        {
            IsIdle = false;
            IsWalk = false;
            IsRun = false;
            IsPhysicAttack = false;
            IsMagicAttack = false;
            IsHit = false;
            IsDeath = false;
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
                case MagicAttackState:
                    IsMagicAttack = true;
                    playerInput.StopMove();
                    break;
                case HitState:
                    IsHit = true;
                    playerInput.StopMove();
                    break;
                case DeathState:
                    IsDeath = true;
                    playerInput.StopMove();
                    break;
            }
        }

        private IEnumerator CompletePhysicAttack()
        {
            yield return new WaitForSeconds(PhysicDelay);
            playerInput.ReleaseMove();
            IsPhysicAttack = false;
        }

        private IEnumerator CompleteMagicAttack()
        {
            yield return new WaitForSeconds(MagicDelay);
            playerInput.ReleaseMove();
            IsMagicAttack = false;
        }

        private IEnumerator CompleteHit()
        {
            yield return new WaitForSeconds(HitDelay);
            playerInput.ReleaseMove();
            IsHit = false;
        }
    }
}