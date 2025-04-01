using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fairyPrefab;
    [SerializeField] private GameObject wolfbossPrefab;
    [SerializeField] private Transform[] fairySpawnPoints;
    [SerializeField] private Transform[] wolfbossSpawnPoints;
    [SerializeField] private int spawnCount = 2; 

    private void Start()
    {
        SpawnMonsters(fairyPrefab, fairySpawnPoints);
        SpawnMonsters(wolfbossPrefab, wolfbossSpawnPoints);
    }

    private void SpawnMonsters(GameObject monsterPrefab, Transform[] spawnPoints)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(monsterPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
