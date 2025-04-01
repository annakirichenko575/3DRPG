using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Enemy; 

public class HealthBarEnemy : MonoBehaviour
{
    [SerializeField] private Slider healthSliderEnemy;    
    [SerializeField] private Slider easeHealthSliderEnemy; 
    [SerializeField] private float lerpSpeed = 0.3f;

    private HealthPoints enemyHealth; 

    private void Start()
    {
        enemyHealth = GetComponentInParent<HealthPoints>(); 

        if (enemyHealth != null)
        {
            healthSliderEnemy.maxValue = enemyHealth.MaxHealth;
            healthSliderEnemy.value = enemyHealth.Health;
            easeHealthSliderEnemy.maxValue = enemyHealth.MaxHealth;
            easeHealthSliderEnemy.value = enemyHealth.Health;

            enemyHealth.OnHit += UpdateHealthBar;
            enemyHealth.OnHeal += UpdateHealthBar;
            enemyHealth.OnDie += HideHealthBar;
        }
    }

    private void Update()
    {
        if (healthSliderEnemy.value != enemyHealth.Health)
        {
            healthSliderEnemy.value = enemyHealth.Health;
        }

        if (easeHealthSliderEnemy.value != enemyHealth.Health)
        {
            easeHealthSliderEnemy.value = Mathf.Lerp(easeHealthSliderEnemy.value, enemyHealth.Health, lerpSpeed);
        }
    }

    private void UpdateHealthBar()
    {
        healthSliderEnemy.value = enemyHealth.Health;
    }



    private void HideHealthBar()
    {
        gameObject.SetActive(false); 
    }

    private void OnDestroy()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnHit -= UpdateHealthBar;
            enemyHealth.OnHeal -= UpdateHealthBar;
            enemyHealth.OnDie -= HideHealthBar;
        }
    }
}
