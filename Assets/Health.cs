using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    Image healthBar;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float HP;
    void Start()
    {
        healthBar = GetComponent<Image>();
        HP = maxHealth;
    }

    void Update()
    {
        healthBar.fillAmount = HP/maxHealth;
    }
}
