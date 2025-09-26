using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{    
    [SerializeField] private int maxHealth = 100;
    private UnitHealth enemyHealth;
    
    void Start()
    {
        // Setiap enemy memiliki health sendiri
        enemyHealth = new UnitHealth(maxHealth, maxHealth);
    }

    void Update()
    {
        // Cek kematian enemy berdasarkan health lokal
        if (enemyHealth.Health <= 0)
        {
            Die();
        }
    }

    // Method untuk enemy menerima damage
    public void EnemyTakeDamage(int damage)
    {
        enemyHealth.DmgUnit(damage);
        Debug.Log("Enemy took " + damage + " damage. Health: " + enemyHealth.Health + "/" + enemyHealth.MaxHealth);
    }

    private void Die()
    {
        Debug.Log("Enemy Killed");
        Destroy(gameObject);
    }
}