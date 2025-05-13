using Enemy.StateMachine;
using Infrastructure;
using UnityEngine;

public class SimpleBossFactory : BossFactory
{
    public SimpleBossFactory(PlayerFactory playerFactory, GameObject bossPrefab, Transform spawnPoint, Transform[] waypoints)
        : base(playerFactory, bossPrefab, spawnPoint, waypoints)
    {
    }

    protected override BossBehaviour SetupBoss(BossBehaviour bossBehaviour)
    {
        if (bossBehaviour == null)
        {
            Debug.LogError("BossBehaviour is null in SetupBoss!");
        }

        bossBehaviour.Construct(playerFactory, waypoints);
        return bossBehaviour;
    }
}



