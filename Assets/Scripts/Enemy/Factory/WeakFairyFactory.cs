using Enemy.StateMachine;
using Infrastructure;
using UnityEngine;

public class WeakFairyFactory : FairyFactory
{
    public WeakFairyFactory(PlayerFactory playerFactory, GameObject fairyPrefab,
        Transform[] waypoints, Transform fairyRunawayPoint) 
        : base(playerFactory, fairyPrefab, waypoints, fairyRunawayPoint)
    {
    }

    protected override FairyBehaviour SetupFairy(FairyBehaviour fairyBehaviour)
    {
        // Параметры слабой феи
        fairyBehaviour.Construct(
            playerFactory,
            waypoints,
            fairyRunawayPoint,
            viewRadius: 10f,
            viewAngle: 90f,
            speedWalk: 3f,
            speedRun: 5f,
            attackRadius: 6f,
            attackDistance: 2f,
            bulletSpeed: 15f,
            fireRate: 0.3f,
            startWaitTime: 4f
        );
        return fairyBehaviour;
    }
}