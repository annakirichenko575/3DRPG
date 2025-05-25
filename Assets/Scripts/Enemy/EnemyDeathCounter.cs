using Infrastructure.Services;
using UnityEngine;

namespace Enemy
{
    public class EnemyDeathCounter : IService
    {
        private const string SoundFXPath = "Audio/SoundFX";
        private readonly MonsterSpawner monsterSpawner;

        private int enemiesToKill = 5;
        private int killsToSpawnBoss = 3;
        private AudioSource audioSource;
        private int enemiesKilled = 0;
        private bool bossSpawned = false;

        private AudioSource VictorySource => audioSource == null ? CreateVictorySource() : audioSource;

        public EnemyDeathCounter(MonsterSpawner spawner)
        {
            this.monsterSpawner = spawner;
        }

        public void EnemyKilled()
        {
            enemiesKilled++;

            Debug.Log($"[EnemyDeathCounter] Enemy killed: {enemiesKilled}");

            if (!bossSpawned && enemiesKilled >= killsToSpawnBoss)
            {
                bossSpawned = true;
                Debug.Log("[EnemyDeathCounter] Spawning boss...");
                monsterSpawner.SpawnExtraBoss(); 
            }

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