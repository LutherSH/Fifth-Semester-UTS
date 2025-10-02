using UnityEditor.Rendering;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    // Rotations
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    // Hipfire Recoil
    private float recoilX;
    private float recoilY;
    private float recoilZ;

    // Settings
    private float snappiness;
    private float returnSpeed;
    private float currentRecoilMultiplier;
    //private float maxRecoilRotation;

    // Track initialized status
    private bool isInitialized = false;
    
    public void Initialize(WeaponConfig config)
    {
        recoilX = config.recoilX;
        recoilY = config.recoilY;
        recoilZ = config.recoilZ;

        snappiness = config.snappiness;
        returnSpeed = config.returnSpeed;
        isInitialized = true;


        currentRotation = Vector3.zero;
        targetRotation = Vector3.zero;
    }

    void Update()
    {
        if (!isInitialized) return;

        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.deltaTime);

        //currentRotation = Vector3.ClampMagnitude(currentRotation, maxRecoilRotation);

        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilFire()
    {
        if (!isInitialized)
        {
            Debug.LogWarning("Recoil not initialized! Cannot apply recoil.");
            return;
        }
        
        float randomY = Random.Range(-recoilY, recoilY);
        float randomZ = Random.Range(-recoilZ, recoilZ);

        targetRotation += new Vector3(recoilX * currentRecoilMultiplier,
                                    randomY * currentRecoilMultiplier,
                                    randomZ * currentRecoilMultiplier);
    }

    public void SetAiming(bool aiming, float recoilMultiplier)
    {
        currentRecoilMultiplier = aiming ? recoilMultiplier : 1f;
    }
    
    // Reset recoil when weapon is disabled
    private void OnDisable()
    {
        if (isInitialized)
        {
            currentRotation = Vector3.zero;
            targetRotation = Vector3.zero;
        }
    }
}
