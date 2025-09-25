using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCollectible : MonoBehaviour
{
    [Header("Ammo Settings")]
    public int ammoAmount = 30;
    public string gunName; // "Pistol", "SMG", etc.
    public float rotationSpeed = 70f;
    public float floatAmplitude = 0.3f;
    public float floatFrequency = 1.5f;

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
        // Animasi floating dan rotation
        FloatAndRotate();
    }

    private void FloatAndRotate()
    {
        // Rotasi
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        
        // Floating movement
        float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectAmmo(other.gameObject);
        }
    }

    private void CollectAmmo(GameObject player)
    {
        bool ammoAdded = false;
        
        // Cari semua senjata di player
        Gun[] guns = player.GetComponentsInChildren<Gun>(true);
        
        foreach (Gun gun in guns)
        {
            // Cocokkan jenis senjata
            if (gun.gunData.gunName.Contains(gunName))
            {
                AddAmmoToGun(gun);
                ammoAdded = true;
            }
        }
        
        if (ammoAdded)
        {
            // Play effects
            PlayCollectionEffects();
            DisableCollectible();
            Destroy(gameObject, collectSound != null ? collectSound.length : 0.1f);
        }
        else
        {
            Debug.LogWarning("No matching gun found for ammo type: " + gunName);
        }
    }

    private void AddAmmoToGun(Gun gun)
    {
        // Method AddAmmo perlu ditambahkan di Gun.cs
        // Kita akan modifikasi Gun.cs setelah ini
        gun.AddAmmo(ammoAmount);
        Debug.Log($"Added {ammoAmount} ammo to {gun.gunData.gunName}");
    }

    private void PlayCollectionEffects()
    {
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
    }

    private void DisableCollectible()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null) renderer.enabled = false;
        
        Collider collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;
        
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
