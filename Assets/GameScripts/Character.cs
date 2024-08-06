using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Character : MonoBehaviour
{
    private AudioManager audioManager;
    private CharacterController characterController;
    private Animator animator;

    public float walkSpeed = 2f;     // Walking speed
    public float runSpeed = 6f;      // Running speed
    public float crouchSpeed = 1f;   // Crouching speed
    public float rotationSpeed = 10f; // Rotation speed
    public float jumpHeight = 2.0f;   // Jump height
    public float gravity = -9.81f;    // Gravity

    public TextMeshProUGUI runTimerText;   // UI Text for Run Timer
    public TextMeshProUGUI cooldownTimerText; // UI Text for Cooldown Timer

    private float currentSpeed;
    private Vector3 velocity;         // Velocity of the character
    private bool isGrounded;          // Check if the character is grounded
    private bool isRunning = false;
    private bool isCrouching = false;
    private bool isHiding = false;   // Track if the player is hiding
    private GameObject itemInRange;   // Item in range for pickup

    // Running management
    private float runDuration = 4f;   // Run duration
    private float runCooldown = 3f;   // Run cooldown
    private float runTimer = 0f;      // Timer for running
    private float cooldownTimer = 0f; // Timer for cooldown
    private bool canRun = true;       // Can the player run?

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentSpeed = walkSpeed; // Start with walk speed
    }

    void Update()
    {
        // Check if the character is on the ground
        isGrounded = characterController.isGrounded;
        animator.SetBool("IsGrounded", isGrounded); // Update the animator with the grounded state

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Slightly negative to ensure grounded check
        }

        HandleCrouching();
        HandleRunning();
        UpdateTimersUI();  // Update UI with running and cooldown timers

        // Update animator parameters
        animator.SetBool("isCrouching", isCrouching);
        animator.SetBool("IsRunning", isRunning);

        // Movement input
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = transform.TransformDirection(move); // Make movement relative to character's orientation

        // Move the character
        characterController.Move(move * Time.deltaTime * currentSpeed);

        // Jumping input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Set the jump velocity
            animator.SetTrigger("Jump"); // Set the jumping animation trigger
            audioManager.PlaySFX(audioManager.jump); // Play jump sound
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        // Rotate the character to face the direction of movement
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Set the animator speed parameter
        animator.SetFloat("Speed", move.magnitude * currentSpeed);

        // Check for item pickup input
        if (Input.GetKeyDown(KeyCode.E) && itemInRange != null)
        {
            Counter.Instance.IncreaseTotal();
            Destroy(itemInRange);
            itemInRange = null;
        }
    }

    private void HandleRunning()
    {
        // Handle running cooldown
        if (!canRun)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= runCooldown)
            {
                canRun = true;
                cooldownTimer = 0f;
            }
        }

        // Handle running input
        if (Input.GetKey(KeyCode.LeftShift) && canRun)
        {
            if (!isRunning)
            {
                audioManager.PlayLoopingSFX(audioManager.run); // Play run sound until stop
                isRunning = true;
                currentSpeed = runSpeed;
                runTimer = 0f; // Reset run timer
            }

            // Running time management
            runTimer += Time.deltaTime;
            if (runTimer >= runDuration)
            {
                isRunning = false;
                currentSpeed = walkSpeed;
                canRun = false;
                audioManager.StopLoopingSFX(); // Stop running sound
            }
        }
        else
        {
            if (isRunning)
            {
                isRunning = false;
                currentSpeed = walkSpeed;
                audioManager.StopLoopingSFX();
            }
            if (!isRunning && !isCrouching && characterController.velocity.magnitude > 0.1f)
            {
                // Play walk sound only if moving
                audioManager.PlayLoopingSFX(audioManager.walk);
            }
            else
            {
                audioManager.StopLoopingSFX();
            }
        }
    }

    private void HandleCrouching()
    {
        // Handle crouching input
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (!isCrouching)
            {
                audioManager.PlaySFX(audioManager.crouch); // Play crouch sound once
                isCrouching = true;
                isRunning = false; // Disable running when crouching
                currentSpeed = crouchSpeed;
            }
        }
        else
        {
            if (isCrouching)
            {
                isCrouching = false;
                isHiding = false;
            }
        }
    }

    private void UpdateTimersUI()
    {
        // Update the UI for running and cooldown timers
        if (runTimerText != null)
        {
            if (isRunning)
            {
                runTimerText.text = "Running Time: " + (runDuration - runTimer).ToString("F1") + "s";
            }
            else
            {
                runTimerText.text = "";
            }
        }

        if (cooldownTimerText != null)
        {
            if (!canRun)
            {
                cooldownTimerText.text = "Cooldown Time: " + (runCooldown - cooldownTimer).ToString("F1") + "s";
            }
            else
            {
                cooldownTimerText.text = "";
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            itemInRange = other.gameObject;
        }

        // Check if player is entering a trashcan
        if (other.CompareTag("Trashcan") && isCrouching == true) // Check for crouching state
        {
            isHiding = true; // Set hiding state
            currentSpeed = 0; // Stop movement when hiding
            Debug.Log("Player is hiding in a trashcan.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            itemInRange = null;
        }

        // Check if player is exiting a trashcan
        if (other.CompareTag("Trashcan"))
        {
            isHiding = false; // Reset hiding state
            isCrouching = false;
            Debug.Log("Player exited the trashcan.");
        }
    }

    // Method to check if the player is hiding
    public bool IsHiding()
    {
        return isHiding;
    }
}