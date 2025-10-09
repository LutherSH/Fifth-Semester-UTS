using UnityEngine;

public class WeaponAnim : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float positionalSway;
    [SerializeField] private float rotationalSway;
    [SerializeField] private float swaySmoothness;

    [Header("Aim Positions")]
    [SerializeField] private Vector3 hipfirePosition;
    [SerializeField] private Vector3 adsPosition;

    private Vector3 initialPosition = Vector3.zero;
    private Quaternion initialRotation = Quaternion.identity;
    private bool isAiming = false;

    private Vector3 currentTargetPosition;
    private float aimTransitionSpeed;

    public void Initialize(WeaponConfig config)
    {
        positionalSway = config.positionalSway;
        rotationalSway = config.rotationalSway;
        swaySmoothness = config.swaySmoothness;
        aimTransitionSpeed = config.aimTransitionSpeed;
    }

    void Start()
    {
        initialPosition = hipfirePosition;
        initialRotation = transform.localRotation;
        currentTargetPosition = initialPosition;
        transform.localPosition = initialPosition;
    }

    private void Update()
    {
        HandleAimTransition();
        
        if (!isAiming)
        {
            ApplySway();
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, currentTargetPosition, Time.deltaTime * aimTransitionSpeed);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, initialRotation, Time.deltaTime * aimTransitionSpeed);
        }
    }

    private void HandleAimTransition()
    {
        currentTargetPosition = isAiming ? adsPosition : hipfirePosition;
    }

    private void ApplySway()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 positionOffset = new Vector3(mouseX, mouseY, 0) * positionalSway;
        Quaternion rotationOffset = Quaternion.Euler(new Vector3(-mouseY, -mouseX, mouseX) * rotationalSway);

        transform.localPosition = Vector3.Lerp(
            transform.localPosition, 
            currentTargetPosition - positionOffset, 
            Time.deltaTime * swaySmoothness
        );
        
        transform.localRotation = Quaternion.Lerp(
            transform.localRotation, 
            initialRotation * rotationOffset, 
            Time.deltaTime * swaySmoothness
        );
    }

    public void SetAiming(bool aiming)
    {
        isAiming = aiming;
        
        // Update target position
        currentTargetPosition = aiming ? adsPosition : hipfirePosition;
    }
}
