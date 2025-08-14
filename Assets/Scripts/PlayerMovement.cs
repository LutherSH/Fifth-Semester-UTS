using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;
    bool isMoving;

    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Ground Check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        // Reset velocity defaultnya
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Dapetin Inputnya
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Moving Vector
        Vector3 move = transform.right * x + transform.forward * z; // right - red axis, forward - blue axis

        // Gerakin Playernya
        controller.Move(move * speed * Time.deltaTime);

        // Check apakah player bisa lompat
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Lompatnya
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Jatuhnya
        velocity.y += gravity * Time.deltaTime;

        // Eksekusi lompat
        controller.Move(velocity * Time.deltaTime);

        if (lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        lastPosition = gameObject.transform.position;
    }
}