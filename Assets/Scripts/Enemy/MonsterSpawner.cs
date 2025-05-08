using Infrastructure;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fairyPrefab;
    [SerializeField] private GameObject wolfbossPrefab;
    [SerializeField] private Transform[] fairySpawnPoints;
    [SerializeField] private Transform[] wolfbossSpawnPoints;
    [SerializeField] private int spawnCount = 2;
    [SerializeField] private Transform wolfRunawayPoint;
    
    private WolfFactory simpleWolfFactory;
    private WolfFactory strongWolfFactory;

    public void Construct(PlayerFactory playerFactory)
    {
        simpleWolfFactory = new SimpleWolfFactory(playerFactory, wolfbossPrefab,
            wolfbossSpawnPoints, wolfRunawayPoint);
        strongWolfFactory = new StrongWolfFactory(playerFactory, wolfbossPrefab,
            wolfbossSpawnPoints, wolfRunawayPoint);
    
        SpawnMonsters(fairyPrefab, new List<Transform>(fairySpawnPoints));

        WolfFactory[] wolfFactories = new[] { simpleWolfFactory, strongWolfFactory };
        Spawn(wolfFactories, wolfbossSpawnPoints.ToList());
    }

    private void Spawn(WolfFactory[] wolfFactories, List<Transform> availableSpawnPoints)
    {
        for (int i = 0; i < spawnCount && availableSpawnPoints.Count > 0; i++)
        {
            int pointIndex = GetRandomIndex(availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[pointIndex];
            int wolfIndex = GetRandomIndex(wolfFactories.Length);
            wolfFactories[wolfIndex].CreateWolf(at: spawnPoint.position);
            availableSpawnPoints.RemoveAt(pointIndex);
        }
    }

    private void SpawnMonsters(GameObject monsterPrefab, List<Transform> availableSpawnPoints)
    {
        for (int i = 0; i < spawnCount && availableSpawnPoints.Count > 0; i++)
        {
            int randomIndex = GetRandomIndex(availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[randomIndex];
            Instantiate(monsterPrefab, spawnPoint.position, Quaternion.identity);

            availableSpawnPoints.RemoveAt(randomIndex);
        }
    }

    private int GetRandomIndex(int count) => Random.Range(0, count);
}