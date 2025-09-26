using UnityEngine;

public class WeaponAnim : MonoBehaviour
{
    [SerializeField] private float positionalSway = 0.1f;
    [SerializeField] private float rotationalSway = 0.1f;
    [SerializeField] private float swaySmoothness = 1f;

    private Vector3 initialPosition = Vector3.zero;
    private Quaternion initialRotation = Quaternion.identity;

    private void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        ApplySway();
    }
    private void ApplySway()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 positionOffset = new Vector3(mouseX, mouseY, 0) * positionalSway;

        Quaternion rotationOffset = Quaternion.Euler(new Vector3(-mouseY, -mouseX, mouseX) * rotationalSway);

        transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition - positionOffset, Time.deltaTime * swaySmoothness);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, initialRotation * rotationOffset, Time.deltaTime * swaySmoothness);
    }
}
