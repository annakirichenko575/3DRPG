using Enemy.StateMachine;
using Infrastructure;
using UnityEngine;

public class SimpleWolfFactory : WolfFactory
{
    private int damage = 20;
    private float attackInterval = 5f;
    private float attackType = 0;

    public SimpleWolfFactory(PlayerFactory playerFactory, GameObject wolfPrefab,
        Transform[] waypoints, Transform wolfRunawayPoint)
        : base(playerFactory, wolfPrefab, waypoints, wolfRunawayPoint)
    {
    }

    protected override WolfBehaviour SetupWolf(WolfBehaviour wolfBehaviour)
    {
        wolfBehaviour.Construct(playerFactory, waypoints, wolfRunawayPoint, 
            damage, attackInterval, attackType);
        return wolfBehaviour;
    }
}
