using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesMovement : MonoBehaviour
{
    Transform[] tiles;
    public List<Transform> tilesList = new List<Transform>();

    private void Start()
    {
        FillList();
    }
    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        FillNodes();
        for (int i = 0; i < tilesList.Count; i++) { 
            Vector3 currentPos = tilesList[i].position;
            if (i > 0) {
                Vector3 prevPos = tilesList[i - 1].position;
                Gizmos.DrawLine(prevPos , currentPos);
            }
        }
    }*/
    void FillList() { 
        tilesList.Clear();
        tiles = GetComponentsInChildren<Transform>();

        foreach (Transform tile in tiles)
        {
            if (tile !=  this.transform && tile.tag == "Tile") { 
                tilesList.Add(tile);
            }
        }
    }
}
