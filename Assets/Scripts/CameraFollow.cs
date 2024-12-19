using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // The player or object to follow

    [Header("Offset Settings")]
    public Vector3 offset = new Vector3(0, 5, -10); // Default camera offset

    [Header("Smoothness Settings")]
    public float followSpeed = 10f; // Speed of the camera following the target

    private void LateUpdate()
    {
        if (target == null) return;

        // Compute the desired position
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate to the desired position
        transform.position = desiredPosition;

        // Optionally align the camera to face forward with the target
        //transform.LookAt(target);
    }
}