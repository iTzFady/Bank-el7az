using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //RollingDice rollingDice;
    public TilesMovement currentTile;
    int playerPostion;
    public int steps = 0;
    public bool isMoving;

    /*private void Awake()
    {
        rollingDice = FindObjectOfType<RollingDice>();
    }*/
    void Update() {
        /*if(!isMoving && rollingDice.num != 0) {
            steps = rollingDice.num;
            rollingDice.ResetNum();
            StartCoroutine(Move());
        }
        //Debug.Log(steps);*/
    }
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
            yield return new WaitForSeconds(.1f);
        }
        isMoving = false;
    }
    bool MoveToNextTiles(Vector3 targetPos) {
        return targetPos != (transform.position = Vector3.MoveTowards(transform.position, targetPos, 5f * Time.deltaTime));
    }
}
