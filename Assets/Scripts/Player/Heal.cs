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
    

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            HealthPoints healthPoints = other.GetComponent<HealthPoints>();
            healthPoints.Heal(heal);
        }
    }
}
