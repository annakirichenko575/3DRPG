using Player;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private HealthPoints healthPoints;

    private void Awake()
    {
        healthPoints.OnHit += HealthChanged;
        healthPoints.OnHeal += HealthChanged;
        healthPoints.OnDie += HealthChanged;
    }

    private void HealthChanged()
    {
        healthBar.fillAmount = (float)healthPoints.Health / healthPoints.MaxHealth;

    }
}
