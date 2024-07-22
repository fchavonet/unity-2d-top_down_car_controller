using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Public variables.
    [Space(10)]
    public Transform target;

    [Space(10)]
    [Header("Camera Settings")]
    public float smoothSpeed = 0.125f;

    void FixedUpdate()
    {
        // Calculate the desired position of the camera.
        Vector3 desiredPosition = new(target.position.x, target.position.y, -10);

        // Smoothly interpolate between the current position and the desired position.
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // // Update the camera's position.
        transform.position = smoothedPosition;
    }
}
