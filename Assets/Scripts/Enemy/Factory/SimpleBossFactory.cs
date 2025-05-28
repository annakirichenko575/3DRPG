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
            Debug.LogError("BossBehaviour is null!");
        }

        bossBehaviour.Construct(playerFactory, waypoints);

        BossWeaponController weaponController = bossBehaviour.GetComponentInChildren<BossWeaponController>();
        if (weaponController != null)
        {
            weaponController.InitializeWeapon();
        }
        else
        {
            Debug.LogWarning("BossWeaponController not found!");
        }

        return bossBehaviour;
    }
}



