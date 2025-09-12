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
        // Lock Cursor At The Middle Of The Screen And Made It Invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Mouse Inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Look Up & Down
        xRotation -= mouseY;

        // Clamp
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        // Look Right & Left
        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
