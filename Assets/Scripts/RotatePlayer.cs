using UnityEngine;

public class RotatePlayer : MonoBehaviour
{
    // Rotation angle
    public float rotationAngle = 90f;
    // Rotation duration in seconds
    public float rotationDuration = 1f;

    private Quaternion targetRotation;
    private bool isRotating = false;
    private float rotationTime;
    private Transform rotatingObject;

    // Method called when another collider enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        // Check if an object is already rotating
        if (!isRotating)
        {
            // Set the object to be rotated
            rotatingObject = other.transform;
            // Calculate the target rotation
            targetRotation = rotatingObject.rotation * Quaternion.Euler(0, rotationAngle, 0);
            // Start the rotation
            isRotating = true;
            rotationTime = 0;
        }
    }

    private void Update()
    {
        if (isRotating && rotatingObject != null && rotatingObject.GetComponent<PlayerMovement>().isMoving)
        {
            // Increment the rotation time
            rotationTime += Time.deltaTime;
            // Smoothly interpolate to the target rotation
            rotatingObject.rotation = Quaternion.Slerp(rotatingObject.rotation, targetRotation, rotationTime / rotationDuration);

            // Check if the rotation duration is reached
            if (rotationTime >= rotationDuration)
            {
                // End the rotation
                isRotating = false;
                // Ensure final rotation is exact
                rotatingObject.rotation = targetRotation;
                rotatingObject = null;
            }
        }
    }
}
