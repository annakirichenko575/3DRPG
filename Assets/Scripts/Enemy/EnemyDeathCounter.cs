using Infrastructure.Services;
using UnityEngine;

namespace Enemy
{
    public class EnemyDeathCounter : IService
    {
        private const string SoundFXPath = "Audio/SoundFX";

        private int enemiesToKill = 5;
        private AudioSource audioSource;
        private int enemiesKilled = 0;

        private AudioSource VictorySource => audioSource == null ? CreateVictorySource() : audioSource;

        public void EnemyKilled()
        {
            enemiesKilled++;

            if (enemiesKilled > 0 && enemiesKilled % enemiesToKill == 0) 
            {
                PlayVictorySound();
            }
        }

        private void PlayVictorySound()
        {
            VictorySource.Play();
        }

        private AudioSource CreateVictorySource()
        {
            GameObject prefab = Resources.Load<GameObject>(SoundFXPath);
            GameObject instance = Object.Instantiate(prefab);
            audioSource = instance.GetComponent<AudioSource>();
            return audioSource;
        }

    }
}