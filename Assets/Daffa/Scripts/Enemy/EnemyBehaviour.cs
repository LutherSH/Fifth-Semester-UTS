using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if (GameManager.gameManager._enemyHealth.Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Eenmy Killed");
        Destroy(gameObject);
    }

    /*private void EnemyTakeDmg(int dmg)
    {
        GameManager.gameManager._enemyHealth.DmgUnit(dmg);
    }*/
}