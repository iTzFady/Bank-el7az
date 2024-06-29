using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private List<Transform> playersOnTile = new List<Transform>();
    private float gridSize = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersOnTile.Add(other.transform);
            AdjustPlayerPositions();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersOnTile.Remove(other.transform);
            AdjustPlayerPositions();
        }
    }

    private void AdjustPlayerPositions()
    {
        int gridColumns = 2; // Number of columns in the grid
        int gridRows = Mathf.CeilToInt(playersOnTile.Count / (float)gridColumns);

        for (int i = 0; i < playersOnTile.Count; i++)
        {
            int column = i % gridColumns;
            int row = i / gridColumns;
            Vector3 offset = new Vector3(column * gridSize, 0, row * gridSize);
            Vector3 targetPos = transform.position + offset - new Vector3((gridColumns - 1) * gridSize / 2, 0, (gridRows - 1) * gridSize / 2);
            //playersOnTile[i].GetComponent<PlayerMovement>().SetTargetPosition(targetPos);
        }
    }
}
