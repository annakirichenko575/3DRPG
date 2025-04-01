using Enemy;
using System;
using System.Collections;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(BoxCollider))]
    public class AttackZone : MonoBehaviour
    {
        private const float Duration = 0.2f;

        [SerializeField] private int damage = 50;
        
        private BoxCollider attackZone;

        private void Awake()
        {
            attackZone = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Enemy.HealthPoints healthPoints = GetEnemyHealthPoints(other);

            if (healthPoints != null)
            {
                healthPoints.Hit(damage);
            }
        }

        private Enemy.HealthPoints GetEnemyHealthPoints(Collider other)
        {
            Enemy.HealthPoints healthPoints = other.GetComponent<Enemy.HealthPoints>();
            if (healthPoints == null)
            {
                healthPoints = other.GetComponentInParent<Enemy.HealthPoints>();
            }

            return healthPoints;
        }

        public void Attack()
        {
            attackZone.enabled = true;
            StartCoroutine(AttackDuring());
        }

        private IEnumerator AttackDuring()
        {
            yield return new WaitForSeconds(Duration);
            attackZone.enabled = false;
        }
    }
}