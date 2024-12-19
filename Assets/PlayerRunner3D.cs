using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunner3D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float gravity = -9.81f;
    public float normal_gravity = -9.81f;
    bool isFalling = false;
    private int currentLane = 1; // Default lane (0 = left, 1 = center, 2 = right)
    public float laneWidth = 1.0f; // Distance between lanes
    public float laneSwitchSpeed = 10.0f; // Speed at which the character moves between lanes
    
    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    Vector3 moveDirection;
    string desiredDirection = "forward";

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool fallTriggered = false;
    
    [Header("Animation")]
    public Animator animator;
    
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        
        animator.SetTrigger("idle");
        animator.SetBool("isIdling", true);
    }

    private void Update()
    {
        HandleMovement();
        HandleTurn();
    }

    private void HandleMovement()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        animator.SetBool("isGrounded", isGrounded);

        if (isGrounded && velocity.y < 0)
        {
            fallTriggered = false;
            velocity.y = -2f; // Keeps the player grounded
            gravity = normal_gravity;
        }
        
        // Falling detection
        isFalling = !isGrounded && velocity.y < 0;
        animator.SetBool("isFalling", isFalling);
        
        // we make sure that runs once while falling
        if (!fallTriggered && isFalling)
        {
            fallTriggered = true;
            gravity *= 2;
            Debug.LogWarning("GRAVITY INCREASING!");
        }
        
        
        //Debug.Log(isFalling);
        
        // Get input for lane switching
        if (Input.GetKeyDown(KeyCode.LeftArrow) && currentLane > 0)
        {
            currentLane--;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && currentLane < 2)
        {
            currentLane++;
        }
        
        // Target position based on the current lane
        float targetXPosition = currentLane * laneWidth - laneWidth; // -laneWidth, 0, +laneWidth for lanes 0, 1, 2
        float moveX = Mathf.MoveTowards(characterController.transform.position.x, targetXPosition, laneSwitchSpeed * 1000f);
        float moveZ = 1f;

        animator.SetTrigger("walk");
        animator.SetBool("isIdling", false);
        animator.SetBool("isWalking", true);
        
        Vector3 movement = new Vector3(moveX - characterController.transform.position.x, 0, moveZ);
        Debug.LogWarning(UpdateMoveDirection());
        characterController.Move(movement * moveSpeed * Time.deltaTime);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            animator.SetTrigger("jump");
        }

        // Apply gravity ??used delta time twice??
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
        
    }

    Vector3 UpdateMoveDirection()
    {
        Vector3 tempDir = moveDirection;
        Vector3 newMoveDirection = Vector3.zero;

        if (desiredDirection == "forward")
        {
            newMoveDirection = new Vector3(0, 0, 1);
        }
        
        if (desiredDirection == "left")
        {
            newMoveDirection = new Vector3(-1, 0, 0);
        }
        return newMoveDirection;
    }
    
    void HandleTurn()
    {
        // turn left
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("speed: " + velocity);
            Debug.Log("move dir: " + moveDirection);
            
            desiredDirection = "left";
        }
    }
    
}
