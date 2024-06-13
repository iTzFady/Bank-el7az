using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform player;            // The player to follow
    public Transform dice;              // The dice to follow when rolling
    public Vector3 playerOffset;        // Offset from the player
    public Vector3 diceOffset;          // Offset from the dice
    public float smoothSpeed = 0.125f;  // Smooth speed of the camera
    public float transitionTime = 1.0f; // Time to transition between targets

    private Transform currentTarget;    // The current target to follow
    private Vector3 currentOffset;      // The current offset
    private bool transitioning = false; // Flag to check if the camera is transitioning
    private float initialYPosition;     // The initial Y position of the camera

    void Start()
    {
        currentTarget = player;
        currentOffset = playerOffset;
        initialYPosition = transform.position.y;
    }

    void FixedUpdate()
    {
        if (currentTarget != null && !transitioning)
        {
            Vector3 desiredPosition = currentTarget.position + currentOffset;

            // Lock the Y position if following the dice
            if (currentTarget == dice)
            {
                desiredPosition.y = initialYPosition;
            }

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            transform.LookAt(currentTarget);
        }
    }

    public void FollowDice()
    {
        if (!transitioning)
        {
            StartCoroutine(SmoothTransition(dice, diceOffset));
        }
    }

    public void FollowPlayer()
    {
        if (!transitioning)
        {
            StartCoroutine(SmoothTransition(player, playerOffset));
        }
    }

    private IEnumerator SmoothTransition(Transform newTarget, Vector3 newOffset)
    {
        transitioning = true;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = newTarget.position + newOffset;

        // Lock the Y position if transitioning to the dice
        if (newTarget == dice)
        {
            endPosition.y = initialYPosition;
        }

        float elapsedTime = 0f;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionTime);

            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, t);
            transform.position = currentPosition;

            transform.LookAt(Vector3.Lerp(currentTarget.position, newTarget.position, t));

            yield return null;
        }

        currentTarget = newTarget;
        currentOffset = newOffset;
        transitioning = false;
    }
}
