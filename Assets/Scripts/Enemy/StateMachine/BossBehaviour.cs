using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Infrastructure;
using Infrastructure.Services;
using Infrastructure.States;

namespace Enemy.StateMachine
{
    public class BossBehaviour : MonoBehaviour
    {
        [SerializeField] private float viewRadius = 20f;
        [SerializeField] private float viewAngle = 120f;
        [SerializeField] private LayerMask playerMask;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private float attackDistance = 5f;
        [SerializeField] private float strongAttackDistance = 3f;
        [SerializeField] private Transform[] waypoints;

        private EnemyPerceptionBoss perception;
        private EnemyStateMachine stateMachine;
        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private HealthPoints healthPoints;
        private Transform player;
        private bool wasAttacked;
        private float attackedResetTimer;

        public Transform Player => player;
        public bool WasAttacked { get; private set; }
        public float HealthPercent => healthPoints != null
            ? (float)healthPoints.Health / healthPoints.MaxHealth
            : 1f;

        [SerializeField] private BossWeaponController bossWeaponController;
        public BossWeaponController WeaponController => bossWeaponController;

        public void Construct(PlayerFactory playerFactory, Transform[] waypoints)
        {
            this.player = playerFactory.Player;

            if (player == null)
            {
                return;
            }

            this.waypoints = waypoints;

            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            healthPoints = GetComponent<HealthPoints>();

            if (perception == null)
                perception = new EnemyPerceptionBoss();

            perception.Initialize(transform, player, viewRadius, viewAngle, playerMask, obstacleMask);

            if (healthPoints != null)
            {
                healthPoints.OnHit += () => WasAttacked = true;
                healthPoints.OnHit += OnEnemyHit;
            }

            InitStateMachine();

            if (!navMeshAgent.enabled)
                navMeshAgent.enabled = true;

            stateMachine.Enter<IdleState>();
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
                [typeof(IdleState)] = new IdleState(stateMachine, this, animator, navMeshAgent),
                [typeof(AggressiveState)] = new AggressiveState(stateMachine, this, animator, navMeshAgent),
                [typeof(AttackStateBoss)] = new AttackStateBoss(stateMachine, this, animator, navMeshAgent, WeaponController),
                [typeof(StrongAttackState)] = new StrongAttackState(stateMachine, this, animator, navMeshAgent, WeaponController),
            };

            stateMachine.Initialize(states);
        }

        private void OnEnemyHit()
        {
            if (healthPoints.IsDeath)
                return;

            wasAttacked = true;
            attackedResetTimer = 0;

            if (!WasAttacked)
            {
                WasAttacked = true;
                stateMachine.Enter<AggressiveState>();
            }
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

        public bool PlayerInSight(out Transform target) => perception.PlayerInSight(out target);
        public bool PlayerInAttackDistance() => Vector3.Distance(transform.position, player.position) <= attackDistance;
        public bool PlayerInStrongAttackDistance() => Vector3.Distance(transform.position, player.position) <= strongAttackDistance;
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

        public Vector3 GetWaypointPositionByIndex(int i) => waypoints[i].position;
        public int WaypointsCount() => waypoints.Length;
    }
}

