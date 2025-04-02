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
        SpawnMonsters(fairyPrefab, new List<Transform>(fairySpawnPoints));
        SpawnMonsters(wolfbossPrefab, new List<Transform>(wolfbossSpawnPoints));
    }

    private void SpawnMonsters(GameObject monsterPrefab, List<Transform> availableSpawnPoints)
    {
        for (int i = 0; i < spawnCount && availableSpawnPoints.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[randomIndex];
            Instantiate(monsterPrefab, spawnPoint.position, Quaternion.identity);

            availableSpawnPoints.RemoveAt(randomIndex);
        }
    }
}
