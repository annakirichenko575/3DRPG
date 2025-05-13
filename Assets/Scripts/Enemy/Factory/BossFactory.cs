using UnityEngine;
using Infrastructure;
using Enemy.StateMachine;

public abstract class BossFactory
{
    protected readonly GameObject bossPrefab;
    protected readonly Transform spawnPoint;
    protected readonly PlayerFactory playerFactory;
    protected readonly Transform[] waypoints;

    public BossFactory(PlayerFactory playerFactory, GameObject bossPrefab, Transform spawnPoint, Transform[] waypoints)
    {
        this.playerFactory = playerFactory;
        this.bossPrefab = bossPrefab;
        this.spawnPoint = spawnPoint;
        this.waypoints = waypoints;
    }

    public BossBehaviour CreateBoss()
    {
        GameObject bossObject = Object.Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
        BossBehaviour bossBehaviour = bossObject.GetComponent<BossBehaviour>();
        bossBehaviour = SetupBoss(bossBehaviour);
        return bossBehaviour;
    }

    protected abstract BossBehaviour SetupBoss(BossBehaviour bossBehaviour);
}

