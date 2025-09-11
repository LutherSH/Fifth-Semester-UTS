using Cinemachine;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;
    public CinemachineVirtualCamera virtualCamera;
    [SerializeField] private AudioSource footstepSound;
    [SerializeField] private Transform cameraFollowTarget;

    [Header("Movements Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeedMultiplier = 2f;
    [SerializeField] private float sprintTransitSpeed = 5f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float jumpHeight = 2f;

    [Header("Crouch Settings")]
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float crouchSpeedMultiplier = 0.5f;
    [SerializeField] private float crouchTransitionSpeed = 10f;
    [SerializeField] private float standingCameraY = 0.5f;
    [SerializeField] private float crouchCameraY = 0.25f;

    private bool isCrouching = false;
    //private bool wantsToStand = false;
    private bool isTransitioning = false;
    private float originalHeight;
    private float targetHeight;
    private float targetCameraY;
    private float originalSpeedMultiplier;

    private float verticalVelocity;
    private float currentSpeed;
    private float currentSpeedMultiplier;
    private float xRotation;

    [Header("Footstep Settings")]                             
    [SerializeField] private LayerMask terrainLayerMask;
    [SerializeField] private float stepInterval = 1f;

    private float nextStepTimer = 0;

    [Header("Camera Bob")]
    /*[SerializeField] private float bobFrequency = 1f;            //This determine how fast the camera shake is
    [SerializeField] private float bobAmplitude = 1f;            //While this determine how strong the camera shake is
    
    private CinemachineBasicMultiChannelPerlin noiseComponent;
    */
    [Header("SFX")]                                             
    [SerializeField] private AudioClip[] groundFootstep;
    [SerializeField] private AudioClip[] grassFootstep;
    [SerializeField] private AudioClip[] gravelFootstep; 

    [Header("Input")]
    [SerializeField] private float mouseSensitivity;
    private float moveInput;                                    // Forward & Backward Keyboard
    private float turnInput;                                    // Right & Left Keyboard
    private float mouseX;
    private float mouseY;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalHeight = controller.height;
        targetHeight = originalHeight;
        targetCameraY = standingCameraY;

        if (cameraFollowTarget == null)
        {
            // Coba cari berdasarkan nama umum
            cameraFollowTarget = transform.Find("CameraTarget") ?? 
                               transform.Find("FollowTarget") ?? 
                               virtualCamera.transform;
        }

        //noiseComponent = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        // Locked cursor and making it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        InputManagement();
        HandleCrouch();
        Movement();
        PlayFootstepSound();
    }

    /*private void LateUpdate()
    {
        CameraBob();
    }*/
    
    private void Movement()
    {
        Turn();
        GroundMovement();
    }

    private void Turn()
    {
        mouseX *= mouseSensitivity * Time.deltaTime;
        mouseY *= mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90, 90);

        virtualCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);  // I dont even know wtf is quaternion even the person on youtube said the same

        transform.Rotate(Vector3.up * mouseX);
    }

    private void GroundMovement()
    {
        Vector3 move = new Vector3(turnInput, 0, moveInput);
        move = virtualCamera.transform.TransformDirection(move);                    // Move forward follow the cinemachine camera direction

        float targetMultiplier = 1f;

        if (isCrouching)
        {
            targetMultiplier = crouchSpeedMultiplier;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            targetMultiplier = sprintSpeedMultiplier;
        }

        currentSpeedMultiplier = Mathf.Lerp(currentSpeedMultiplier, targetMultiplier, sprintTransitSpeed * Time.deltaTime);

        currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed * currentSpeedMultiplier, sprintTransitSpeed * Time.deltaTime);

        move *= currentSpeed;

        move.y = VerticalForceCalculation();

        controller.Move(move * Time.deltaTime);
    }

    private void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isTransitioning)
        {
            if (isCrouching)
            {
                TryStandUp();
            }
            else
            {
                Crouch();
            }
        }

        controller.height = Mathf.Lerp(controller.height, targetHeight, crouchTransitionSpeed * Time.deltaTime);

        if (cameraFollowTarget != null)
        {
            Vector3 cameraPos = cameraFollowTarget.localPosition;
            cameraPos.y = Mathf.Lerp(cameraPos.y, targetCameraY, crouchTransitionSpeed * Time.deltaTime);
            cameraFollowTarget.localPosition = cameraPos;
        }

        /*if (wantsToStand && !CheckCeiling())
        {
            StandUp();
        }*/
    }

     private void TryStandUp()
    {
        if (!CheckCeiling())
        {
            StandUp();
        }
        else
        {
            //wantsToStand = true; 
        }
    }

    private void Crouch()
    {
        isCrouching = true;
        //wantsToStand = false;
        targetHeight = crouchHeight;
        targetCameraY = crouchCameraY;
        originalSpeedMultiplier = currentSpeedMultiplier; 
        currentSpeedMultiplier *= crouchSpeedMultiplier;
    }

    private void StandUp()
    {
        isCrouching = false;
        //wantsToStand = false;
        targetHeight = originalHeight;
        targetCameraY = standingCameraY;
        currentSpeedMultiplier = originalSpeedMultiplier; 
    }

     private bool CheckCeiling()
    {
        float raycastDistance = (originalHeight - crouchHeight) + 0.2f;
        Vector3 raycastStart = transform.position + Vector3.up * (controller.height / 2);
        return Physics.Raycast(raycastStart, Vector3.up, raycastDistance);
    }

    private void PlayFootstepSound()
    {
        if (controller.isGrounded && controller.velocity.magnitude > 0.1f)
        {
            if (Time.time >= nextStepTimer)                                         // The faster player move, the faster the footstep
            {
                AudioClip[] footstepClips = DetermineAudioClips();

                if (footstepClips.Length > 0)
                {
                    AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];

                    footstepSound.PlayOneShot(clip);
                }

                nextStepTimer = Time.time + (stepInterval / currentSpeedMultiplier);
            }
        }
    }

    private AudioClip[] DetermineAudioClips()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, 1.5f, terrainLayerMask))
        {
            string tag = hit.collider.tag;

            switch (tag)
            {
                case "Ground":
                    return groundFootstep;
                case "Grass":
                    return grassFootstep;
                case "Gravel":
                    return gravelFootstep;
                default:
                    return groundFootstep;
            }
        }
        return groundFootstep;
    }

    private float VerticalForceCalculation()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -1f;

            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * gravity * 2);
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        return verticalVelocity;
    }

    /*private void CameraBob()
    {
        if (controller.isGrounded && controller.velocity.magnitude > 0.1f)
        {
            noiseComponent.m_FrequencyGain = bobFrequency * currentSpeedMultiplier;
            noiseComponent.m_AmplitudeGain = bobAmplitude * currentSpeedMultiplier;
        }
        else
        {
            noiseComponent.m_FrequencyGain = 0.0f;
            noiseComponent.m_AmplitudeGain = 0.0f;
        }
    }*/

    private void InputManagement()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }
}