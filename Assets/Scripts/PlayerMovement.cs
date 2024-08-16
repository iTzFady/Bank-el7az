using System.Collections;
using Mirror;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private TilesMovement currentTile;
    [SyncVar] public int playerPostion;
    public int steps = 0;
    [SyncVar] public bool isMoving;
    [SyncVar] public int playerScore = 0;
    private BoxCollider boxCollider;
    [SyncVar] public bool loseTurn;
    [SyncVar] public bool inJail;
    public Penalty currentPenalty;
    public SyncHashSet<int> activatedTiles = new SyncHashSet<int>();

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        currentTile = FindObjectOfType<TilesMovement>();
    }
    private void Update()
    {
        if (steps != 0)
        {
            StartMoving();
        }
    }
    public void StartMoving()
    {
        StartCoroutine(Move());
    }
    IEnumerator Move()
    {
        GameManager.instance.isSomeonePlaying = true;
        if (isMoving)
        {
            yield break;
        }
        isMoving = true;
        while (steps != 0)
        {
            if (steps > 0)
            {
                playerPostion++;
                steps--;
            }
            if (steps < 0)
            {
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
        GameManager.instance.isSomeonePlaying = false;
        boxCollider.enabled = false;
        boxCollider.enabled = true;
    }
    bool MoveToNextTiles(Vector3 targetPos)
    {
        return targetPos != (transform.position = Vector3.MoveTowards(transform.position, targetPos, 5f * Time.deltaTime));
    }
}
