using Enemy.StateMachine;
using Infrastructure;
using UnityEngine;

public abstract class FairyFactory
{
    private readonly GameObject fairyPrefab;
    protected readonly Transform[] waypoints;
    protected readonly PlayerFactory playerFactory;
    protected readonly Transform fairyRunawayPoint;

    public FairyFactory(PlayerFactory playerFactory, GameObject fairyPrefab,
        Transform[] waypoints, Transform fairyRunawayPoint)
    {
        this.playerFactory = playerFactory;
        this.fairyPrefab = fairyPrefab;
        this.waypoints = waypoints;
        this.fairyRunawayPoint = fairyRunawayPoint;
    }

    public FairyBehaviour CreateFairy(Vector3 at)
    {
        GameObject fairyObject = Object.Instantiate(fairyPrefab, at, Quaternion.identity);
        FairyBehaviour fairyBehaviour = fairyObject.GetComponent<FairyBehaviour>();
        fairyBehaviour = SetupFairy(fairyBehaviour);
        return fairyBehaviour;
    }

    protected abstract FairyBehaviour SetupFairy(FairyBehaviour fairyBehaviour);
}