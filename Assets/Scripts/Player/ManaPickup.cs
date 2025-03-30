using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class ManaPickup : MonoBehaviour
{
    [SerializeField] private GameObject manaPrefab; 
    [SerializeField] private Transform[] spawnPoints; 

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            SpawnNewMana();
            Destroy(gameObject); 
        }
    }

    void SpawnNewMana()
    {
        if (manaPrefab != null && spawnPoints.Length > 0)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(manaPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}

