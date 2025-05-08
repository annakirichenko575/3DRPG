using Enemy.StateMachine;
using Infrastructure;
using UnityEngine;

public class StrongWolfFactory : WolfFactory
{
    private int damage = 30;
    private float attackInterval = 3f;
    private float attackType = 1;

    public StrongWolfFactory(PlayerFactory playerFactory, GameObject wolfPrefab,
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