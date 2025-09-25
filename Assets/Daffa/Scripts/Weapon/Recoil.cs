using UnityEngine;

public class Recoil : MonoBehaviour
{
    // Rotations
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    // Hipfire Recoil
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    // Settings
    [SerializeField] private float snappiness = 10f;
    [SerializeField] private float returnSpeed = 5f;

    // Reference ke WeaponSway
    private WeaponSway weaponSway;
    
    // Tambahan: batasan recoil
    [SerializeField] private float maxRecoilRotation = 30f;

    void Start()
    {
        weaponSway = GetComponent<WeaponSway>();
        
        // Inisialisasi rotasi
        currentRotation = Vector3.zero;
        targetRotation = Vector3.zero;
    }

    void Update()
    {
        // Smooth movement menuju target rotation
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, snappiness * Time.deltaTime);
        
        // Batasi rotasi maksimum
        currentRotation = Vector3.ClampMagnitude(currentRotation, maxRecoilRotation);
        
        // Gabungkan rotasi sway dan recoil dengan cara yang lebih aman
        ApplyCombinedRotation();
    }
    
    private void ApplyCombinedRotation()
    {
        if (weaponSway != null)
        {
            // Gunakan rotasi identitas sebagai base, lalu aplikasikan sway, lalu recoil
            Quaternion finalRotation = Quaternion.identity;
            finalRotation = finalRotation * weaponSway.SwayRotation;
            finalRotation = finalRotation * Quaternion.Euler(currentRotation);
            
            transform.localRotation = finalRotation;
        }
        else
        {
            transform.localRotation = Quaternion.Euler(currentRotation);
        }
    }

    public void RecoilFire()
    {
        // Tambahkan recoil dengan randomness yang terkontrol
        float randomY = Random.Range(-recoilY, recoilY);
        float randomZ = Random.Range(-recoilZ, recoilZ);
        
        targetRotation += new Vector3(recoilX, randomY, randomZ);
        
        // Pastikan tidak melebihi batas maksimum
        targetRotation = Vector3.ClampMagnitude(targetRotation, maxRecoilRotation);
    }
}
