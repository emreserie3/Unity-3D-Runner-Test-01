using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerController3D : MonoBehaviour
{
    
    [Header("Movement Settings")]
    Vector3 velocity = Vector3.zero;
    public float jumpForce = 7f;
    public float gravity = -9.81f;
    public float forwardSpeed = 10f;
    public float laneSwitchSpeed = 10f;
    public float laneDistance = 4f; // Distance between lanes

    [Header("Input Settings")]
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    private int targetLane = 1; // 0 = Left, 1 = Middle, 2 = Right
    private float targetXPosition = 0;
    private CharacterController characterController;
    private Vector3 movement;
    
    [Header("Animation")]
    public Animator animator;

    private bool isGrounded;
    private bool isFalling;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        targetLane = 1; // Start in the middle lane
        UpdateTargetPosition();
        
        animator.SetTrigger("walk");
        animator.SetBool("isIdling", false);
        animator.SetBool("isWalking", true);
    }

    private void Update()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Keeps the player grounded
        }
        
        // Handle lane switching
        if (Input.GetKeyDown(leftKey))
        {
            MoveLane(false); // Move left
        }
        else if (Input.GetKeyDown(rightKey))
        {
            MoveLane(true); // Move right
        }

        // Update position
        UpdateTargetPosition();
        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            animator.SetTrigger("jump");
        }
        
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        animator.SetBool("isGrounded", isGrounded);
        
        // Falling detection
        isFalling = !isGrounded && velocity.y < 0;
        animator.SetBool("isFalling", isFalling);
        //Debug.Log(isFalling);
        
        // Calculate movement
        float currentXPosition = Mathf.Lerp(transform.position.x, targetXPosition, Time.deltaTime * laneSwitchSpeed);
        movement = new Vector3(currentXPosition - transform.position.x, velocity.y * Time.deltaTime, forwardSpeed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        //Debug.Log( "movespeed = " + (transform.position.z / Time.time));
        
    }

    private void FixedUpdate()
    {
        // Apply movement
        characterController.Move(movement);
    }

    private void MoveLane(bool goingRight)
    {
        if (goingRight)
        {
            targetLane = Mathf.Clamp(targetLane + 1, 0, 2);
        }
        else
        {
            targetLane = Mathf.Clamp(targetLane - 1, 0, 2);
        }
    }

    private void UpdateTargetPosition()
    {
        // Calculate the target position based on the lane
        targetXPosition = (targetLane - 1) * laneDistance;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Handle collision logic here
        if (hit.collider.CompareTag("Obstacle"))
        {
            Debug.Log("Hit an obstacle!");
            // Add game-over logic or reduce health
        }
    }
}
