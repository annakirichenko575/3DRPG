using System.Collections;
using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Heal : MonoBehaviour
{
    [SerializeField] private int heal = 10;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject healPrefab;
    [SerializeField] private Transform[] spawnPoints;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            HealthPoints healthPoints = other.GetComponent<HealthPoints>();
            if (healthPoints != null) 
            {
                healthPoints.Heal(heal);
                Destroy(gameObject); 
                SpawnNewHeal();
            }
        }
    }

    void SpawnNewHeal()
    {
        if (healPrefab != null && spawnPoints.Length > 0)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)]; 
            Instantiate(healPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
