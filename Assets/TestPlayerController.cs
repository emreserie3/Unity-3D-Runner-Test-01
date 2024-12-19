using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    private CharacterController characterController;
    public float moveSpeed = 10f;
    private Vector3 movement;
    private float currentAngle = 0f;

    private Quaternion targetRotation;
    public float turnSpeed = 100f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        movement = Vector3.forward; // Start moving forward
    }

    void Update()
    {
        bool l = Input.GetKeyDown(KeyCode.LeftArrow);
        bool r = Input.GetKeyDown(KeyCode.RightArrow);

        // Handle rotation
        if (r)
        {
            currentAngle += 90f; // Turn 90 degrees to the right
            targetRotation *= Quaternion.Euler(0, 90, 0); // Rotate +90 degrees
        }
        if (l)
        {
            currentAngle -= 90f; // Turn 90 degrees to the left
            targetRotation *= Quaternion.Euler(0, -90, 0); // Rotate -90 degrees
        }

        // Normalize the angle
        currentAngle = Mathf.Repeat(currentAngle, 360f);

        // Rotate the player
        //transform.rotation = Quaternion.Euler(0, currentAngle, 0);
        // Smooth rotation towards targetRotation
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        

        // Calculate the new movement direction based on the angle
        float angleRad = Mathf.Deg2Rad * currentAngle;
        movement = new Vector3(Mathf.Sin(angleRad), 0, Mathf.Cos(angleRad));

        // !!!player teleports: moves from pivot 
        targetRotation = Quaternion.LookRotation(movement);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        
        // Apply movement
        Vector3 moveVector = movement * moveSpeed;
        characterController.Move(moveVector * Time.deltaTime);
    }
}