using System.Collections;
using UnityEngine;
using Player;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private int damage = 20;
    [SerializeField] private float attackInterval = 5f;
    [SerializeField] private string playerTag = "Player";

    private HealthPoints playerHealth;
    private Coroutine attackCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerHealth = other.GetComponent<HealthPoints>();

            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(PeriodicAttack());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }

    private IEnumerator PeriodicAttack()
    {
        while (playerHealth != null && !playerHealth.IsDeath)
        {
            playerHealth.Hit(damage);
            yield return new WaitForSeconds(attackInterval);
        }

        attackCoroutine = null;
    }
}
