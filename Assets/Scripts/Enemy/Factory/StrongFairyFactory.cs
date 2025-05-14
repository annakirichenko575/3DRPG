using Enemy.StateMachine;
using Infrastructure;
using UnityEngine;

public class StrongFairyFactory : FairyFactory
{
    public StrongFairyFactory(PlayerFactory playerFactory, GameObject fairyPrefab,
        Transform[] waypoints, Transform fairyRunawayPoint) 
        : base(playerFactory, fairyPrefab, waypoints, fairyRunawayPoint)
    {
    }

    protected override FairyBehaviour SetupFairy(FairyBehaviour fairyBehaviour)
    {
        // Параметры сильной феи
        fairyBehaviour.Construct(
            playerFactory,
            waypoints,
            fairyRunawayPoint,
            viewRadius: 15f,
            viewAngle: 120f,
            speedWalk: 5f,
            speedRun: 7f,
            attackRadius: 8f,
            attackDistance: 3f,
            bulletSpeed: 20f,
            fireRate: 0.5f,
            startWaitTime: 2f
        );
        return fairyBehaviour;
    }
}