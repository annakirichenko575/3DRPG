using Enemy.StateMachine;
using Infrastructure;
using UnityEngine;

namespace Enemy.Factory
{
    public abstract class WolfFactory
    {
        protected readonly Transform[] waypoints;
        protected readonly PlayerFactory playerFactory;
        protected readonly Transform wolfRunawayPoint;
        private readonly GameObject wolfPrefab;
        private readonly EnemyDeathCounter deathCounter;

        public WolfFactory(PlayerFactory playerFactory, GameObject wolfPrefab,
            Transform[] waypoints, Transform wolfRunawayPoint, EnemyDeathCounter deathCounter)
        {
            this.playerFactory = playerFactory;
            this.wolfPrefab = wolfPrefab;
            this.waypoints = waypoints;
            this.wolfRunawayPoint = wolfRunawayPoint;
            this.deathCounter = deathCounter;
        }

        public WolfBehaviour CreateWolf(Vector3 at)
        {
            GameObject wolfObject = Object.Instantiate(wolfPrefab, at, Quaternion.identity);
            WolfBehaviour wolfBehaviour = wolfObject.GetComponent<WolfBehaviour>();
            wolfObject.GetComponent<HealthPoints>().OnDie += deathCounter.EnemyKilled;
            wolfBehaviour = SetupWolf(wolfBehaviour);
            return wolfBehaviour;
        }

        protected abstract WolfBehaviour SetupWolf(WolfBehaviour wolfBehaviour);
    }
}