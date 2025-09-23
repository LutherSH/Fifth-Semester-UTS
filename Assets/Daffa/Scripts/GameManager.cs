using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // This script is a Singleton
    // If you dont know what a Singleton is you're cooked 
    public static GameManager gameManager { get; private set; }
    public UnitHealth _playerHealth = new UnitHealth(100, 100);
    public UnitHealth _enemyHealth = new UnitHealth(100, 100);
    void Awake()
    {
        if (gameManager != null && gameManager != this)
        {
            Destroy(this);
        }
        else
        {
            gameManager = this;
        }
    }
}