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
     private float snappiness = 10f;
    private float returnSpeed = 5f;

    
    private float maxRecoilRotation;
    private float currentRecoilMultiplier = 1f;


    public void Initialize(GunData gunData)
    {
        recoilX = gunData.recoilX;
        recoilY = gunData.recoilY;
        recoilZ = gunData.recoilZ;
        snappiness = gunData.snappiness;
        returnSpeed = gunData.returnSpeed;
        maxRecoilRotation = gunData.maxRecoilRotation;
        
        currentRotation = Vector3.zero;
        targetRotation = Vector3.zero;
    }

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.deltaTime);
        
        currentRotation = Vector3.ClampMagnitude(currentRotation, maxRecoilRotation);
        
        transform.localRotation = Quaternion.Euler(currentRotation);
    }
    
    public void RecoilFire()
    {
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
}
