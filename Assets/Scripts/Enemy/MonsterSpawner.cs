using Infrastructure;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fairyPrefab;
    [SerializeField] private Transform[] fairySpawnPoints;

    [SerializeField] private GameObject wolfbossPrefab;
    [SerializeField] private Transform[] wolfbossSpawnPoints;
    [SerializeField] private Transform[] wolfWaypoints;
    [SerializeField] private Transform wolfRunawayPoint;
    [SerializeField] private int spawnCount = 2;

    [SerializeField] private GameObject extraBossPrefab;
    [SerializeField] private Transform extraBossSpawnPoint;
    [SerializeField] private Transform[] bossWaypoints;

    [SerializeField] private GameObject sceneBossToRemove;

    private WolfFactory simpleWolfFactory;
    private WolfFactory strongWolfFactory;
    private BossFactory extraBossFactory;

    public void Construct(PlayerFactory playerFactory)
    {
        if (playerFactory == null)
        {
            return;
        }

        simpleWolfFactory = new SimpleWolfFactory(playerFactory, wolfbossPrefab, wolfWaypoints, wolfRunawayPoint);
        strongWolfFactory = new StrongWolfFactory(playerFactory, wolfbossPrefab, wolfWaypoints, wolfRunawayPoint);
        extraBossFactory = new SimpleBossFactory(playerFactory, extraBossPrefab, extraBossSpawnPoint, bossWaypoints);

        SpawnMonsters(fairyPrefab, new List<Transform>(fairySpawnPoints));

        WolfFactory[] wolfFactories = new[] { simpleWolfFactory, strongWolfFactory };
        SpawnWolves(wolfFactories, wolfbossSpawnPoints.ToList());

        SpawnExtraBoss();
    }

    private void SpawnWolves(WolfFactory[] wolfFactories, List<Transform> availableSpawnPoints)
    {
        for (int i = 0; i < spawnCount && availableSpawnPoints.Count > 0; i++)
        {
            int pointIndex = GetRandomIndex(availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[pointIndex];

            int wolfIndex = GetRandomIndex(wolfFactories.Length);
            wolfFactories[wolfIndex].CreateWolf(spawnPoint.position);

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

    private void SpawnExtraBoss()
    {
        if (extraBossFactory != null)
        {
            var newBoss = extraBossFactory.CreateBoss();

            if (newBoss != null)
            {

                if (sceneBossToRemove != null)
                {
                    Destroy(sceneBossToRemove);
                }
            }
            else
            {
                Debug.LogWarning("CreateBoss returned null. Scene boss not removed.");
            }
        }
        else
        {
            Debug.LogWarning("Extra boss factory is not initialized!");
        }
}

    private int GetRandomIndex(int count) => Random.Range(0, count);
}
