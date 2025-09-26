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

    
    [SerializeField] private float maxRecoilRotation = 30f;

    void Start()
    {        
        currentRotation = Vector3.zero;
        targetRotation = Vector3.zero;
    }

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, snappiness * Time.deltaTime);
        
        currentRotation = Vector3.ClampMagnitude(currentRotation, maxRecoilRotation);
        
        ApplyCombinedRotation();
    }
    
    private void ApplyCombinedRotation()
    {
            Quaternion finalRotation = Quaternion.identity;
            finalRotation = finalRotation * Quaternion.Euler(currentRotation);
            
            transform.localRotation = finalRotation;
        
        
            transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilFire()
    {
        float randomY = Random.Range(-recoilY, recoilY);
        float randomZ = Random.Range(-recoilZ, recoilZ);
        
        targetRotation += new Vector3(recoilX, randomY, randomZ);
        
        targetRotation = Vector3.ClampMagnitude(targetRotation, maxRecoilRotation);
    }
}
