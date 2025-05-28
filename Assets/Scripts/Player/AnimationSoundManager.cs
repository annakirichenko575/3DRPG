using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Player
{
    public class AnimationSoundManager : MonoBehaviour
    {
        [SerializeField] private HealthPoints healthPoints;
        [SerializeField] private AnimationPresenter animationPresenter;
        [SerializeField] private AudioClip hitSound;
        [SerializeField] private AudioClip deathSound;
        [SerializeField] private AudioClip attackSound;
        [SerializeField] private AudioClip magAttackSound;

        private void Awake()
        {
            if (healthPoints != null)
            {
                healthPoints.OnHit += PlayHitSound;
                healthPoints.OnDie += PlayDeathSound;
            }
            if (animationPresenter != null)
            {
                animationPresenter.OnPhysicAttack += PlayPhysicAttackSound;
                animationPresenter.OnMagicAttack += PlayMagicAttackSound;
            }
        }

        private void PlayHitSound()
        {
            PlaySound(hitSound);
        }

        private void PlayDeathSound()
        {
            PlaySound(deathSound);
        }

        private void PlayPhysicAttackSound()
        {
            PlaySound(attackSound);
        }

        private void PlayMagicAttackSound()
        {
            PlaySound(magAttackSound);
        }

        private void PlaySound(AudioClip clip)
        {
            if (clip != null)
            {
                AudioSource.PlayClipAtPoint(clip, transform.position);
            }
        }
    }
}

