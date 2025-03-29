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
            if (other.TryGetComponent(out Enemy.HealthPoints healthPoints))
            {
                healthPoints.Hit(damage);
            }
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