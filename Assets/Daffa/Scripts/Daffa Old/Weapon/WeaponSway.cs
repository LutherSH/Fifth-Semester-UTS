using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float smooth = 8f;
    [SerializeField] private float swayMultiplier = 2f;
    
    [Header("Clamp Settings")]
    [SerializeField] private float maxSwayAngle = 10f;

    // Public property untuk mengakses rotasi sway
    public Quaternion SwayRotation { get; private set; }
    
    // Variabel untuk smooth damping
    private Quaternion targetSwayRotation;

    private void Start()
    {
        SwayRotation = Quaternion.identity;
        targetSwayRotation = Quaternion.identity;
    }

    private void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;
        
        // Clamp nilai input untuk menghindari gerakan ekstrem
        mouseX = Mathf.Clamp(mouseX, -maxSwayAngle, maxSwayAngle);
        mouseY = Mathf.Clamp(mouseY, -maxSwayAngle, maxSwayAngle);

        // Calculate target rotation
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        targetSwayRotation = rotationX * rotationY;
        
        // Smooth rotation menggunakan Slerp
        SwayRotation = Quaternion.Slerp(SwayRotation, targetSwayRotation, smooth * Time.deltaTime);
    }
}
