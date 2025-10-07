using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] HealthBar _healthBar;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip healSound;
    public SceneManagerTG sceneManager;
    public bool isDead;

    void Start()
    {
        // Get audio source if not assigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        isDead = false;
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
        
        // Play heal sound
        if (healSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(healSound);
        }
    }
    
    private void Die()
    {
        isDead = true;
        sceneManager.ShowGameOver();
        Debug.Log("Player Died");
        Destroy(gameObject);
    }
}