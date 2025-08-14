using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovements : MonoBehaviour
{

    public float mouseSensitivity = 700f;

    float xRotation = 0f;
    float yRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;
    void Start()
    {
        // Ngelock cursor di tengah layar sama bikin invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Buat mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Liat Atas & Bawah
        xRotation -= mouseY;

        // Dikasi Clamp
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        // Liat Kanan & Kiri
        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
