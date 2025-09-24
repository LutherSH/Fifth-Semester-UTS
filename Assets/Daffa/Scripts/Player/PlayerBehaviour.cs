using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] HealthBar _healthBar;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            PlayerTakeDmg(20);
            Debug.Log("Player Health: " + GameManager.gameManager._playerHealth.Health);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayerHeal(10);
            Debug.Log("Player Health: " + GameManager.gameManager._playerHealth.Health);
        }

        if (GameManager.gameManager._playerHealth.Health <= 0)
        {
            Die();
        }
    }

    public void PlayerTakeDmg(int dmg)
    {
        GameManager.gameManager._playerHealth.DmgUnit(dmg);
        _healthBar.SetHealth(GameManager.gameManager._playerHealth.Health);
    }

    public void PlayerHeal(int healing)
    {
        GameManager.gameManager._playerHealth.HealUnit(healing);
        _healthBar.SetHealth(GameManager.gameManager._playerHealth.Health);
    }
    
    private void Die()
    {
        Debug.Log("Player Died");
        // Game over logic here
        Destroy(gameObject);
    }
}