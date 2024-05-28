using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    RollingDice rollingDice;
    public TilesMovement currentTile;
    int playerPostion;
    public int steps;
    bool isMoving;


    private void Awake()
    {
        rollingDice = FindObjectOfType<RollingDice>();
    }
    void Update() {
        if(!isMoving) {
            
            StartCoroutine(Move());
        }
        Debug.Log(steps);
    }
    IEnumerator Move() {
        if (isMoving) { 
            yield break;
        }
        isMoving = true;
        while (steps > 0)
        {
            //playerPostion++;
            //playerPostion %= currentTile.tilesList.Count;
            Vector3 nextPos = currentTile.tilesList[playerPostion + 1].position;
            while (MoveToNextTiles(nextPos)) { yield return null; }
            yield return new WaitForSeconds(.1f);
            steps--;
            playerPostion++;
        }
        isMoving = false;
    }
    bool MoveToNextTiles(Vector3 targetPos) {
        return targetPos != (transform.position = Vector3.MoveTowards(transform.position, targetPos, 5f * Time.deltaTime));
        
    }
}
