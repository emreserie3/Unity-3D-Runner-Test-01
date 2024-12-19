using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float gravity = -9.81f;
    bool isFalling = false;
    
    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    
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
    }

    private void HandleMovement()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        animator.SetBool("isGrounded", isGrounded);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Keeps the player grounded
        }
        
        // Falling detection
        isFalling = !isGrounded && velocity.y < 0;
        animator.SetBool("isFalling", isFalling);
        //Debug.Log(isFalling);
        
        // Get input for movement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (moveX == 0 && moveZ == 0)
        {
            animator.SetTrigger("idle");
            animator.SetBool("isIdling", true);
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetTrigger("walk");
            animator.SetBool("isIdling", false);
            animator.SetBool("isWalking", true);
        }

        // Calculate movement relative to the camera's direction
        Vector3 inputDirection = new Vector3(moveX, 0f, moveZ).normalized;
        //Vector3 move = (cameraTransform.forward * inputDirection.z + cameraTransform.right * inputDirection.x);
        Vector3 move = new Vector3(inputDirection.x, 0f ,inputDirection.z);
        move.y = 0f; // Prevent unintended vertical movement

        // Rotate the character to face movement direction if moving
        if (inputDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        characterController.Move(move * moveSpeed * Time.deltaTime);
        //Debug.Log(transform.position.z / Time.time);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            animator.SetTrigger("jump");
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }


    

    
}
