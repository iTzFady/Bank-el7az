using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public TilesMovement currentTile;
    int playerPostion;
    public int steps = 0;
    public bool isMoving;

    public void StartMoving() {
        StartCoroutine(Move());
    }

    IEnumerator Move() {
        if (isMoving) { 
            yield break;
        }
        isMoving = true;
        while (steps != 0)
        {
            if (steps > 0) {
                playerPostion++;
                steps--;
            } 
            if (steps < 0) {
                playerPostion--;
                steps++;
            }

            // Ensure the player position wraps around the tile list
            playerPostion = (playerPostion + currentTile.tilesList.Count) % currentTile.tilesList.Count;
            Vector3 nextPos = currentTile.tilesList[playerPostion].position;
            while (MoveToNextTiles(nextPos)) { yield return null; }
            // Adjust players' positions on the same tile
            yield return new WaitForSeconds(.1f);
        }
        isMoving = false;
    }
    bool MoveToNextTiles(Vector3 targetPos) {
        return targetPos != (transform.position = Vector3.MoveTowards(transform.position, targetPos, 5f * Time.deltaTime));
    }
}
