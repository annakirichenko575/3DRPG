using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player; 

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime); 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out HealthPoints playerHealth))
            {
                playerHealth.Hit((int)damage); 
            }
            Destroy(gameObject); 
        }
    }
}
