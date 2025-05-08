using System;
using UnityEngine;
using UnityEngine.AI;
using Infrastructure;
using System.Collections.Generic;
using Infrastructure.States;

namespace Enemy.StateMachine
{
    public class WolfBehaviour : MonoBehaviour
    {
        public static readonly int AttackTypeHash = Animator.StringToHash("AttackType");

        [SerializeField] private EnemyPerception perceptions = new EnemyPerception();
        
        private Transform[] waypoints;
        private EnemyStateMachine stateMachine;
        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private Enemy.HealthPoints healthPoints;
        private Transform runawayPoint;

        private bool wasAttacked;
        private float attackedResetTimer;
        private int damage = 20;
        private float attackInterval = 5f;
        private float attackType;
        private PlayerFactory playerFactory;

        public Transform Player => playerFactory.Player;
        public bool WasAttacked { get; private set; } = false;
        public float HealthPercent => 
            healthPoints != null 
            ? (float)healthPoints.Health / healthPoints.MaxHealth 
            : 1f;

        public void Construct(PlayerFactory playerFactory,
            Transform[] waypoints, Transform runawayPoint, 
            int damage, float attackInterval, float attackType)
        {
            this.runawayPoint = runawayPoint;
            this.damage = damage;
            this.attackInterval = attackInterval;
            this.attackType = attackType;
            this.waypoints = waypoints;

            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            animator.SetFloat(AttackTypeHash, attackType);
            healthPoints = GetComponent<HealthPoints>();
            healthPoints.OnHit += () => WasAttacked = true;
            healthPoints.OnHit += OnEnemyHit;
        
            this.playerFactory = playerFactory;
            perceptions.Initialize(transform, Player);
            InitStateMachine();

            if (!navMeshAgent.enabled)
            {
                navMeshAgent.enabled = true;
            }
            stateMachine.Enter<PatrolState>();
        }

        private void Update()
        {
            stateMachine?.Update();
            UpdateAttackedTimer();
        }

        private void InitStateMachine()
        {
            stateMachine = new EnemyStateMachine();
            var states = new Dictionary<Type, IEnemyState>
            {
                [typeof(PatrolState)] = new PatrolState(stateMachine, this, perceptions, animator, navMeshAgent),
                [typeof(ChasingState)] = new ChasingState(stateMachine, this, perceptions, animator, navMeshAgent),
                [typeof(AttackState)] = new AttackState(stateMachine, this, perceptions, animator, damage, attackInterval),
                [typeof(RunawayState)] = new RunawayState(stateMachine, this, animator, navMeshAgent, runawayPoint.position),
            };
            stateMachine.Initialize(states);
        }


        public void Move(float speed)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = speed;
        }

        public void Stop()
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.speed = 0;
        }

        private void OnEnemyHit()
        {
            if (healthPoints.IsDeath)
                return; 

            wasAttacked = true;
            attackedResetTimer = 0;
        }

        private void UpdateAttackedTimer()
        {
            if (wasAttacked)
            {
                attackedResetTimer += Time.deltaTime;
                if (attackedResetTimer >= 5f)
                {
                    wasAttacked = false;
                    attackedResetTimer = 0;
                }
            }
        }

        private void OnDestroy()
        {
            if (healthPoints != null)
                healthPoints.OnHit -= OnEnemyHit;
        }


        public Vector3 GetWaypointPositionByIndex(int i) =>
            waypoints[i].position;

        public int WaipointsCount() =>
            waypoints.Length;
    }

}
