using Enemy.StateMachine;
using Infrastructure;
using UnityEngine;

public abstract class WolfFactory
{
    private readonly GameObject wolfPrefab;
    protected readonly Transform[] waypoints;
    protected readonly PlayerFactory playerFactory;
    protected readonly Transform wolfRunawayPoint;

    public WolfFactory(PlayerFactory playerFactory, GameObject wolfPrefab,
        Transform[] waypoints, Transform wolfRunawayPoint)
    {
        this.playerFactory = playerFactory;
        this.wolfPrefab = wolfPrefab;
        this.waypoints = waypoints;
        this.wolfRunawayPoint = wolfRunawayPoint;
    }

    public WolfBehaviour CreateWolf(Vector3 at)
    {
        GameObject wolfObject = Object.Instantiate(wolfPrefab, at, Quaternion.identity);
        WolfBehaviour wolfBehaviour = wolfObject.GetComponent<WolfBehaviour>();
        wolfBehaviour = SetupWolf(wolfBehaviour);
        return wolfBehaviour;
    }

    protected abstract WolfBehaviour SetupWolf(WolfBehaviour wolfBehaviour);
}
