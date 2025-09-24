using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float smooth;
    [SerializeField] private float swayMultiplier;

    // Public property untuk mengakses rotasi sway
    public Quaternion SwayRotation { get; private set; }

    private void Start()
    {
        SwayRotation = Quaternion.identity;
    }

    private void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

        // Calculate user rotation
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        // Calculate sway rotation
        SwayRotation = Quaternion.Slerp(SwayRotation, targetRotation, smooth * Time.deltaTime);
    }
}
