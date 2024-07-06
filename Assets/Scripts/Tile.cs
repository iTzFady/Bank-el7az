using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    public Marker[] markers = new Marker[4];
    [SerializeField] LayerMask playerMask;

    private void Awake()
    {
        markers = GetComponentsInChildren<Marker>();
        playerMask = LayerMask.GetMask("Player");
    }
    private void Update()
    {
        CheckingForPlayer();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the trigger is activated by a player
        {
            // Find an available marker for this player
            for (int i = 0; i < markers.Length; i++)
            {
                if (!markers[i].isUnavaliable) // Check if this marker is available
                {
                    // Assign this marker to the player and mark it as taken
                    MovePlayerToMarker(other.transform, markers[i].markerLocation);
                    markers[i].isUnavaliable = true;
                    markers[i].playerAssigned = other.GetComponent<PlayerMovement>();
                    Debug.Log("Debugging");
                    break; // Exit loop once a marker is assigned
                }
            }
        }
    }
    void CheckingForPlayer() {
        foreach (Marker marker in markers)
        {
            if (marker.isUnavaliable)
            {
                if (Physics.Raycast(marker.transform.position, Vector3.up , out RaycastHit hit, 4f , playerMask))
                {
                    marker.playerAssigned = null;
                    marker.isUnavaliable = false;
                    Debug.Log("Player Detected");
                }
            }
        }
    }
    void MovePlayerToMarker(Transform player, Transform marker)
    {
        Vector3 targetPosition = marker.position;
        player.transform.position = marker.transform.position;
    }
}
