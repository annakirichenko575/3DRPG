using Enemy;
using Enemy.Factory;
using Infrastructure;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fairyPrefab;
    [SerializeField] private Transform[] fairySpawnPoints;
    [SerializeField] private Transform[] fairyWaypoints;
    [SerializeField] private Transform fairyRunawayPoint;

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
    private FairyFactory weakFairyFactory;
    private FairyFactory strongFairyFactory;
    private BossFactory extraBossFactory;

    public void Construct(PlayerFactory playerFactory, EnemyDeathCounter deathCounter)
    {
        if (playerFactory == null)
        {
            return;
        }

        simpleWolfFactory = new SimpleWolfFactory(playerFactory, wolfbossPrefab, wolfWaypoints, wolfRunawayPoint, deathCounter);
        strongWolfFactory = new StrongWolfFactory(playerFactory, wolfbossPrefab, wolfWaypoints, wolfRunawayPoint, deathCounter);
        weakFairyFactory = new WeakFairyFactory(playerFactory, fairyPrefab, fairyWaypoints, fairyRunawayPoint, deathCounter);
        strongFairyFactory = new StrongFairyFactory(playerFactory, fairyPrefab, fairyWaypoints, fairyRunawayPoint, deathCounter);
        extraBossFactory = new SimpleBossFactory(playerFactory, extraBossPrefab, extraBossSpawnPoint, bossWaypoints);

        FairyFactory[] fairyFactories = new[] { weakFairyFactory, strongFairyFactory};
        SpawnFairies(fairyFactories, fairySpawnPoints.ToList());

        WolfFactory[] wolfFactories = new[] { simpleWolfFactory, strongWolfFactory };
        SpawnWolves(wolfFactories, wolfbossSpawnPoints.ToList());

        PrepareBoss();
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

    private void SpawnFairies(FairyFactory[] fairyFactories, List<Transform> availableSpawnPoints)
    {
        for (int i = 0; i < spawnCount && availableSpawnPoints.Count > 0; i++)
        {
            int pointIndex = GetRandomIndex(availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[pointIndex];

            int fairyIndex = GetRandomIndex(fairyFactories.Length);
            fairyFactories[fairyIndex].CreateFairy(spawnPoint.position);
            

            availableSpawnPoints.RemoveAt(pointIndex);
        }
    }

    private void PrepareBoss()
    {
        if (sceneBossToRemove != null)
            sceneBossToRemove.SetActive(false);
    }

    public void SpawnExtraBoss()
    {
        if (sceneBossToRemove != null)
            sceneBossToRemove.SetActive(true); 

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
                Debug.LogWarning("CreateBoss returned null.");
            }
        }
        else
        {
            Debug.LogWarning("Extra boss factory is not initialized!");
        }

    }

    private int GetRandomIndex(int count) => Random.Range(0, count);
}
