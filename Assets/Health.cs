using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private int _maxHealth = HealthPoints.maxHealth;
    [SerializeField] public static float _HP;
    
    void Start()
    {
        healthBar = GetComponent<Image>();
        _HP = _maxHealth;
    }

    void Update()
    {
        _HP = HealthPoints.health;
        healthBar.fillAmount = _HP/_maxHealth;      
    }
}
