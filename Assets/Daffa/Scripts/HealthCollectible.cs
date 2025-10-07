using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [Header("Health Settings")]
    public int healAmount = 25;
    public float rotationSpeed = 50f;
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;

    [Header("Visual Effects")]
    public GameObject collectEffect;
    public AudioClip collectSound;

    private Vector3 startPosition;
    private AudioSource audioSource;

    void Start()
    {
        startPosition = transform.position;
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        FloatAndRotate();
    }

    private void FloatAndRotate()
    {
        // Rotation
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        
        // Floating movement
        float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectHealth(other.GetComponent<PlayerBehaviour>());
        }
    }

    private void CollectHealth(PlayerBehaviour player)
    {
        if (player != null)
        {
            // Heal player
            player.PlayerHeal(healAmount);
            
            // Play sound
            if (collectSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(collectSound);
            }
            
            // Spawn effect
            if (collectEffect != null)
            {
                Instantiate(collectEffect, transform.position, Quaternion.identity);
            }
            
            // Disable renderer dan collider terlebih dahulu
            DisableCollectible();
            
            // Destroy object setelah sound selesai
            Destroy(gameObject, collectSound != null ? collectSound.length : 0.1f);
        }
    }

    private void DisableCollectible()
    {
        // Non-aktifkan renderer dan collider
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null) renderer.enabled = false;
        
        Collider collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;
        
        // Non-aktifkan child objects (jika ada)
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
