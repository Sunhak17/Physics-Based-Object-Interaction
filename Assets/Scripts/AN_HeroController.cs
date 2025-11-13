using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AN_HeroController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Character movement speed")]
    public float MoveSpeed = 30f;
    [Tooltip("Jump force strength")]
    public float JumpForce = 200f;
    [Tooltip("Camera rotation sensitivity (Arrow Keys)")]
    public float CameraRotationSpeed = 100f;
    
    [Header("Physics Customization")]
    [Tooltip("Custom mass for the player")]
    public float CustomMass = 1f;
    [Tooltip("Air drag resistance")]
    public float CustomDrag = 0.5f;
    [Tooltip("Rotation drag resistance")]
    public float CustomAngularDrag = 0.05f;
    
    [Header("Bounce Settings")]
    [Tooltip("Bounce force when hitting walls")]
    public float BounceForce = 5f;
    [Tooltip("Enable bounce effect")]
    public bool EnableBounce = true;
    
    bool jumpFlag = true; 
    bool isGrounded = false;

    CharacterController character;
    Rigidbody rb;
    Vector3 moveVector;
    Transform Cam;
    float cameraYRotation = 0f;
    float cameraXRotation = 0f;
    float inputHorizontal = 0f;
    float inputVertical = 0f;
    [Header("Camera Follow")]
    [Tooltip("How quickly the player rotates to match camera yaw when using arrow keys")]
    public float cameraFollowSpeed = 8f;

    void Start()
    {
        character = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        Cam = Camera.main.GetComponent<Transform>();

        // Apply custom physics properties
        if (rb != null)
        {
            rb.mass = CustomMass;
            rb.drag = CustomDrag;
            rb.angularDrag = CustomAngularDrag;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
    // Movement input is taken from WASD 
    inputHorizontal = (Input.GetKey(KeyCode.D) ? 1f : 0f) - (Input.GetKey(KeyCode.A) ? 1f : 0f);
    inputVertical = (Input.GetKey(KeyCode.W) ? 1f : 0f) - (Input.GetKey(KeyCode.S) ? 1f : 0f);

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            cameraYRotation -= CameraRotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            cameraYRotation += CameraRotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            cameraXRotation -= CameraRotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            cameraXRotation += CameraRotationSpeed * Time.deltaTime;
        }

        cameraXRotation = Mathf.Clamp(cameraXRotation, -85f, 60f);

        Cam.rotation = Quaternion.Euler(cameraXRotation, cameraYRotation, 0);

        // If player is rotating the camera with arrow keys, make the body follow camera yaw
        bool camRotating = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow);
        if (camRotating)
        {
            float currentYaw = transform.eulerAngles.y;
            float newYaw = Mathf.LerpAngle(currentYaw, cameraYRotation, Time.deltaTime * cameraFollowSpeed);
            transform.rotation = Quaternion.Euler(0f, newYaw, 0f);
        }
        else if (inputHorizontal != 0f || inputVertical != 0f)
        {
            // Rotate to face movement direction relative to camera yaw
            float targetAngle = Mathf.Atan2(inputHorizontal, inputVertical) * Mathf.Rad2Deg + cameraYRotation;
            float angle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, Time.deltaTime * 5f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        // Jump
        if (Input.GetButtonDown("Jump") && jumpFlag == true)
        {
            rb.AddForce(Vector3.up * JumpForce);
        }
    }

    void FixedUpdate()
    {
        
        Vector3 forward = Cam.forward;
        Vector3 right = Cam.right;
        
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

    // Calculate move direction using WASD input (arrow keys reserved for camera)
    Vector3 moveDirection = forward * inputVertical + right * inputHorizontal;
        
        moveVector = moveDirection * MoveSpeed + Vector3.up * rb.velocity.y;
        rb.velocity = moveVector;
    }
    
    // Ground detection and wind zones
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("Platform") || other.CompareTag("Untagged"))
        {
            jumpFlag = true;
            isGrounded = true;
        }

        if (other.CompareTag("WindZone"))
        {
            Vector3 windDirection = other.transform.forward;
            rb.AddForce(windDirection * 10f, ForceMode.Force);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        jumpFlag = false;
        isGrounded = false;
    }

    // Bounce effect when hitting walls
    private void OnCollisionEnter(Collision collision)
    {
        if (EnableBounce && collision.gameObject.CompareTag("Wall"))
        {
            Vector3 bounceDirection = Vector3.Reflect(rb.velocity.normalized, collision.contacts[0].normal);
            rb.AddForce(bounceDirection * BounceForce, ForceMode.Impulse);
        }

        // Speed booster
        if (collision.gameObject.CompareTag("SpeedBoost"))
        {
            Vector3 boostDirection = rb.velocity.normalized;
            rb.AddForce(boostDirection * 20f, ForceMode.Impulse);
        }

        // Wind zone push
        if (collision.gameObject.CompareTag("WindZone"))
        {
            Vector3 windDirection = collision.transform.forward;
            rb.AddForce(windDirection * 15f, ForceMode.Force);
        }
    }

}
