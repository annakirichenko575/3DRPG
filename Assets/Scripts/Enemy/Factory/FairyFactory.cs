using Enemy.StateMachine;
using Infrastructure;
using UnityEngine;

namespace Enemy.Factory
{
    public abstract class FairyFactory
    {
        protected readonly Transform[] waypoints;
        protected readonly PlayerFactory playerFactory;
        protected readonly Transform fairyRunawayPoint;

        private readonly GameObject fairyPrefab;
        private readonly EnemyDeathCounter deathCounter;

        public FairyFactory(PlayerFactory playerFactory, GameObject fairyPrefab,
            Transform[] waypoints, Transform fairyRunawayPoint, EnemyDeathCounter deathCounter)
        {
            this.playerFactory = playerFactory;
            this.fairyPrefab = fairyPrefab;
            this.waypoints = waypoints;
            this.fairyRunawayPoint = fairyRunawayPoint;
            this.deathCounter = deathCounter;
        }

        public FairyBehaviour CreateFairy(Vector3 at)
        {
            GameObject fairyObject = Object.Instantiate(fairyPrefab, at, Quaternion.identity);
            FairyBehaviour fairyBehaviour = fairyObject.GetComponent<FairyBehaviour>();
            fairyObject.GetComponent<HealthPoints>().OnDie += deathCounter.EnemyKilled;
            fairyBehaviour = SetupFairy(fairyBehaviour);
            return fairyBehaviour;
        }

        protected abstract FairyBehaviour SetupFairy(FairyBehaviour fairyBehaviour);
    }
}