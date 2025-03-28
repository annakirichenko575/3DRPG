using System.Collections;
using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Heal : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "heal") {
            HealthPoints.health += 10;
            if (HealthPoints.health >= HealthPoints.maxHealth){
                healthBar.fillAmount = 1f;
                HealthPoints.health = HealthPoints.maxHealth;
            }
            healthBar.fillAmount += 10f/HealthPoints.maxHealth;
        }
    }
}
