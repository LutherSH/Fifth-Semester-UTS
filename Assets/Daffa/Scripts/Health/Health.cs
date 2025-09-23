using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("Events")]
    public System.Action onDamageTaken;
    public System.Action onHealed;
    public System.Action onDeath;

    public bool IsAlive => currentHealth > 0;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        if (!IsAlive) return;

        currentHealth = Mathf.Max(0, currentHealth - damageAmount);
        onDamageTaken?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        if (!IsAlive) return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + healAmount);
        onHealed?.Invoke();
    }

    public void RestoreFullHealth()
    {
        currentHealth = maxHealth;
        onHealed?.Invoke();
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }

    private void Die()
    {
        onDeath?.Invoke();
        Debug.Log(gameObject.name + " has died!");
        
        Destroy(gameObject);
    }
}
