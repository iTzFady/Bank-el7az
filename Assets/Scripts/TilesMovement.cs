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
    void FillList()
    {
        tilesList.Clear();
        tiles = GetComponentsInChildren<Transform>();

        foreach (Transform tile in tiles)
        {
            if (tile != this.transform && tile.tag == "Tile")
            {
                tilesList.Add(tile);
            }
        }
    }
}
